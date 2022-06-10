using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

var apiUrl = args.Length > 0 ? args[0] : "https://rebrickable.com/api/v3/swagger/?format=openapi";
var key = args.Length > 1 ? args[1] : "";

var document = await OpenApiDocument.FromUrlAsync(apiUrl);
var clientSettings = new CSharpClientGeneratorSettings
{
    ClassName = "RebrickableClient",
    CSharpGeneratorSettings =
    {
        Namespace = "RebrickableApi",
    },
    InjectHttpClient = true,
    ExceptionClass = "RebrickableApiException",
    OperationNameGenerator = new SingleClientFromOperationIdOperationNameGenerator(),
    ClientClassAccessModifier = "public sealed ",
    GenerateOptionalParameters = true
};

var clientGenerator = new CSharpClientGenerator(document, clientSettings);
var generatedClientFile = clientGenerator.GenerateFile();
generatedClientFile = generatedClientFile.Replace(" virtual ", " ");
generatedClientFile = generatedClientFile.Replace("protected ", "private ");

generatedClientFile = generatedClientFile.Replace("public async System.Threading.Tasks.Task ", "public async System.Threading.Tasks.Task<string> ");
generatedClientFile = generatedClientFile.Replace("public System.Threading.Tasks.Task ", "public System.Threading.Tasks.Task<string> ");
generatedClientFile = generatedClientFile.Replace("return;", "return await response_.Content.ReadAsStringAsync();");
generatedClientFile = generatedClientFile.Replace("using System = global::System;", "using System = global::System; using Newtonsoft.Json;");

var charList = generatedClientFile.ToList();
var opening = false;

for (int index = 0; index < charList.Count;)
{
    if (charList[index] == 'g' && charList[index + 1] == '>' && charList[index + 2] == ' ')
    {
        opening = true;
    }

    if (opening && charList[index] == '_')
    {
        charList[index + 1] = char.ToUpper(charList[index + 1]);
        charList.RemoveAt(index);
    }

    if (charList[index] == '(')
    {
        opening = false;
    }

    index++;
}

generatedClientFile = string.Join("", charList);
//generatedClientFile = generatedClientFile.Replace("private string BaseUrl", "private string _baseUrl");
//generatedClientFile = generatedClientFile.Replace("get { return BaseUrl; }", "get { return _baseUrl; }");
//generatedClientFile = generatedClientFile.Replace("set { BaseUrl = value; }", "set { _baseUrl = value; }");

generatedClientFile = generatedClientFile
    .Replace(
        $"public async System.Threading.Tasks.Task<string> LegoSetsListAsync",
        $"public async System.Threading.Tasks.Task<LegoSetsListAsyncResponse> LegoSetsListAsync");

var legoSetsListAsyncIndex = generatedClientFile.LastIndexOf("LegoSetsListAsync");
var search = generatedClientFile.Substring(legoSetsListAsyncIndex);
var rest = ReplaceFirst(search, "return await response_.Content.ReadAsStringAsync();", "return JsonConvert.DeserializeObject<LegoSetsListAsyncResponse>(await response_.Content.ReadAsStringAsync());");
generatedClientFile = generatedClientFile.Substring(0, legoSetsListAsyncIndex) + rest;

string ReplaceFirst(string text, string search, string replace)
{
    int pos = text.IndexOf(search);
    if (pos < 0)
    {
        return text;
    }
    return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
}

//var lastClosingBracket = generatedClientFile.LastIndexOf('}');
//generatedClientFile = generatedClientFile.Substring(0, lastClosingBracket) + "{responses} } " + generatedClientFile.Substring(lastClosingBracket);

//var httpClient = new HttpClient();
//httpClient.BaseAddress = new Uri("https://rebrickable.com");
//httpClient.DefaultRequestHeaders.Add("Authorization", $"key {key}");

//JsonClassGenerator generator = new JsonClassGenerator();
//generator.UsePascalCase = true;
//generator.SingleFile = true;
//generator.AttributeUsage = JsonPropertyAttributeUsage.Always;
//generator.OutputType = OutputTypes.ImmutableRecord;
//string error = null;

//var responses = string.Empty;

//var legoSetsListAsyncResponse = await new RebrickableApi.RebrickableClient(httpClient).LegoSetsListAsync(page: 2);
//var legoSetsListAsyncResponseRecords = generator.GenerateClasses(legoSetsListAsyncResponse, out error).ToString();
//legoSetsListAsyncResponseRecords = legoSetsListAsyncResponseRecords.Replace("Root", "LegoSetsListAsync");

//generatedClientFile = generatedClientFile
//    .Replace(
//        $"public async System.Threading.Tasks.Task<string> {nameof(RebrickableApi.RebrickableClient.LegoSetsListAsync)}",
//        $"public async System.Threading.Tasks.Task<{nameof(RebrickableApi.RebrickableClient.LegoSetsListAsync)}> {nameof(RebrickableApi.RebrickableClient.LegoSetsListAsync)}");

//generatedClientFile = generatedClientFile.Replace("return await response.Content.ReadAsStringAsync();", "return await response.Content.ReadAsStringAsync();");

//responses += legoSetsListAsyncResponseRecords;
//responses += System.Environment.NewLine;

//generatedClientFile = generatedClientFile.Replace("{responses}", responses);

Console.Write(generatedClientFile);