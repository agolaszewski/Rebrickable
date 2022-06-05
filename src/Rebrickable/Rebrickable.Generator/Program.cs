using NSwag;
using NSwag.CodeGeneration.CSharp;

var document = await OpenApiDocument.FromUrlAsync("https://rebrickable.com/api/v3/swagger/?format=openapi");
var clientSettings = new CSharpClientGeneratorSettings
{
    
    ClassName = "RebrickableApiClient",
    CSharpGeneratorSettings =
    {
        Namespace = "Rebrickable.Api",
    },
    InjectHttpClient = true,
    ExceptionClass = "RebrickableApiException",
    ClientClassAccessModifier = "public",


};

var clientGenerator = new CSharpClientGenerator(document, clientSettings);
var generatedClientFile = clientGenerator.GenerateFile();
//generatedClientFile = generatedClientFile.Replace(" virtual ", " ");
//generatedClientFile = generatedClientFile.Replace("protected ", "private ");

Console.Write(generatedClientFile);

