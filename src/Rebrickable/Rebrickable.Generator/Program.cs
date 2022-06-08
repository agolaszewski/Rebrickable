using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

var document = await OpenApiDocument.FromUrlAsync("https://rebrickable.com/api/v3/swagger/?format=openapi");
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
};

var clientGenerator = new CSharpClientGenerator(document, clientSettings);
var generatedClientFile = clientGenerator.GenerateFile();
generatedClientFile = generatedClientFile.Replace(" virtual ", " ");
generatedClientFile = generatedClientFile.Replace("protected ", "private ");

var charList = generatedClientFile.ToList();
for (int index = 0; index < charList.Count;)
{
    if (charList[index] == '_')
    {
        charList[index + 1] = char.ToUpper(charList[index + 1]);
        charList.RemoveAt(index);
    }

    index++;
}

generatedClientFile = string.Join("", charList);
generatedClientFile = generatedClientFile.Replace("private string BaseUrl", "private string _baseUrl");
generatedClientFile = generatedClientFile.Replace("get { return BaseUrl; }", "get { return _baseUrl; }");
generatedClientFile = generatedClientFile.Replace("set { BaseUrl = value; }", "set { _baseUrl = value; }");

Console.Write(generatedClientFile);