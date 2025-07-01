using System.Reflection;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();
    
var environmentName = config["environment"];

var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

var myConfig = new ConfigurationBuilder()
    .SetBasePath(basePath) // Ensures the path is where the DLL/EXE lives
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    .Build();


Console.WriteLine(myConfig["ConnectionStrings:Postgres"]);