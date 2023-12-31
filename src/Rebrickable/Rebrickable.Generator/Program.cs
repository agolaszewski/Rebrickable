using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

var apiUrl = args.Length > 0 ? args[0] : "https://rebrickable.com/api/v3/swagger/?format=openapi";

var document = await OpenApiDocument.FromUrlAsync(apiUrl);
var clientSettings = new CSharpClientGeneratorSettings
{
    ClassName = "RebrickableClient",
    CSharpGeneratorSettings =
    {
        Namespace = "Rebrickable.Api",
    },
    InjectHttpClient = true,
    ExceptionClass = "RebrickableApiException",
    OperationNameGenerator = new SingleClientFromOperationIdOperationNameGenerator(),
    ClientClassAccessModifier = "public sealed ",
    GenerateOptionalParameters = true,
    GenerateClientInterfaces = true
};

var clientGenerator = new CSharpClientGenerator(document, clientSettings);
var generatedClientFile = clientGenerator.GenerateFile();
generatedClientFile = generatedClientFile.Replace(" virtual ", " ");
generatedClientFile = generatedClientFile.Replace("protected ", "private ");

generatedClientFile = generatedClientFile.Replace(".Task ", ".Task<string> ");
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

generatedClientFile = ReplaceSignature("LegoSetsListAsync", generatedClientFile);
generatedClientFile = ReplaceSignature("LegoThemesListAsync", generatedClientFile);
generatedClientFile = ReplaceSignature("LegoSetsReadAsync", generatedClientFile);
generatedClientFile = ReplaceSignature("LegoSetsMinifigsListAsync", generatedClientFile);
generatedClientFile = ReplaceSignature("LegoMinifigsSetsListAsync", generatedClientFile);

generatedClientFile = ReplaceReturn("LegoSetsListAsync", generatedClientFile);
generatedClientFile = ReplaceReturn("LegoThemesListAsync", generatedClientFile);
generatedClientFile = ReplaceReturn("LegoSetsReadAsync", generatedClientFile);
generatedClientFile = ReplaceReturn("LegoSetsMinifigsListAsync", generatedClientFile);
generatedClientFile = ReplaceReturn("LegoMinifigsSetsListAsync", generatedClientFile);

string ReplaceFirst(string text, string search, string replace)
{
    int pos = text.IndexOf(search);
    if (pos < 0)
    {
        return text;
    }
    return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
}

string ReplaceReturn(string method, string generated)
{
    var index = generated.LastIndexOf(method);
    var search = generated.Substring(index);
    var rest = ReplaceFirst(search, "return await response_.Content.ReadAsStringAsync();", $"return JsonConvert.DeserializeObject<{method}Response>(await response_.Content.ReadAsStringAsync());");
    return generated.Substring(0, index) + rest;
}

string ReplaceSignature(string method, string generated)
{
    generated = generated
        .Replace(
            $"public async System.Threading.Tasks.Task<string> {method}",
            $"public async System.Threading.Tasks.Task<{method}Response> {method}");

    generated = generated
        .Replace(
            $"System.Threading.Tasks.Task<string> {method}",
            $"System.Threading.Tasks.Task<{method}Response> {method}");

    return generated;
}

File.WriteAllText("..//..//..//..//Rebrickable.Api//RebrickableApiClient.cs", generatedClientFile);