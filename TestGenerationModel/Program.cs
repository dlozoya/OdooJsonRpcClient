using Microsoft.Extensions.Configuration;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace TestGenerationModel
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter table name: ");
            var tableName = Console.ReadLine();
            if (string.IsNullOrEmpty(tableName))
            {
                Console.WriteLine("Table name is required");
                return;
            }

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>() // Carga los secretos vinculados al proyecto
                .Build();
            //read odoo config from secrets.json

            var _odooConfig = new OdooConfig(
                    apiUrl: configuration["OdooConfig:url"],
                    dbName: configuration["OdooConfig:db"],
                    userName: configuration["OdooConfig:username"],
                    password: configuration["OdooConfig:password"]
                    );
            var _odooClient = new OdooClient(_odooConfig);
            var loginResponse = await _odooClient.LoginAsync();
            if (loginResponse.Succeed)
            {
                Console.WriteLine("Login success");
                var modelResult = await _odooClient.GetModelAsync(tableName);
                var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
            }
            else
            {
                Console.WriteLine("Login failed");
            }

        }
    }
}
