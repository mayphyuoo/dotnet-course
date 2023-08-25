using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Intermediate.Models;
using Intermediate.Data;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Intermediate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DataContextDapper dapper = new DataContextDapper(config);

            // string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //     Motherboard,
            //     HasWifi,
            //     HasLTE,
            //     ReleaseDate,
            //     Price,
            //     VideoCard
            // ) VALUES ('" + myComputer.Motherboard 
            //         + "','" + myComputer.HasWifi
            //         + "','" + myComputer.HasLTE
            //         + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
            //         + "','" + myComputer.Price
            //         + "','" + myComputer.VideoCard
            // + "')";

            // overwrites
            // File .WriteAllText("log.txt", "\n" + sql + "\n");

            // using StreamWriter openFile = new("log.txt", append: true);

            // openFile.WriteLine("\n" + sql + "\n");

            // openFile.Close();

            string computersJSON = File.ReadAllText("Computers.json");

            // Console.WriteLine(computersJSON);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            IEnumerable<Computer>? computersNewtonSoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJSON);

            IEnumerable<Computer>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJSON, options);


            if (computersNewtonSoft != null)
            {
                foreach (Computer computer in computersNewtonSoft)
                {
                    string sql = @"INSERT INTO TutorialAppSchema.Computer (
                        Motherboard,
                        HasWifi,
                        HasLTE,
                        ReleaseDate,
                        Price,
                        VideoCard
                    ) VALUES ('" + EscapeSingleQuote(computer.Motherboard) 
                            + "','" + computer.HasWifi
                            + "','" + computer.HasLTE
                            + "','" + computer.ReleaseDate?.ToString("yyyy-MM-dd")
                            + "','" + computer.Price
                            + "','" + EscapeSingleQuote(computer.VideoCard)
                    + "')";

                    dapper.ExecuteSql(sql);
                }
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string computersCopy = JsonConvert.SerializeObject(computersNewtonSoft, settings);

            File.WriteAllText("computersCopyNewtonsoft.txt", computersCopy);

            string computersCopySys = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);

            File.WriteAllText("computersCopySys.txt", computersCopySys);
        }

        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");

            return output;
        }
    }
}