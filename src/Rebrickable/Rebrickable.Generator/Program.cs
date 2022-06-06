using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

var document = await OpenApiDocument.FromUrlAsync("https://rebrickable.com/api/v3/swagger/?format=openapi");
var clientSettings = new CSharpClientGeneratorSettings
{
    ClassName = "{controller}Client",
    CSharpGeneratorSettings =
    {
        Namespace = "RebrickableApi",
    },
    InjectHttpClient = true,
    ExceptionClass = "RebrickableApiException",
    OperationNameGenerator = new MultipleClientsFromPathSegmentsOperationNameGenerator(),
    ClientClassAccessModifier = "public ",
    
};

var clientGenerator = new CSharpClientGenerator(document, clientSettings);
var generatedClientFile = clientGenerator.GenerateFile();
//generatedClientFile = generatedClientFile.Replace(" virtual ", " ");
//generatedClientFile = generatedClientFile.Replace("protected ", "private ");

Console.Write(generatedClientFile);