using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Intermediate.Models;
using Intermediate.Data;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Microsoft.Extensions.Options;

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

            string computersJson = File.ReadAllText("ComputersSnake.json");

            Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
                cfg.CreateMap<ComputerSnake, Computer>()
                    .ForMember(dest => dest.ComputerId, 
                        options => options.MapFrom(source => source.computer_id))
                    .ForMember(dest => dest.Motherboard,
                        options => options.MapFrom(source => source.motherboard))
                    .ForMember(dest => dest.HasWifi,
                        options => options.MapFrom(source => source.has_wifi))
                    .ForMember(dest => dest.HasLTE,
                        options => options.MapFrom(source => source.has_lte))
                    .ForMember(dest => dest.ReleaseDate,
                        options => options.MapFrom(source => source.release_date))
                    .ForMember(dest => dest.VideoCard,
                        options => options.MapFrom(source => source.video_card))
                    .ForMember(dest => dest.CPUCores,
                        options => options.MapFrom(source => source.cpu_cores))
                    .ForMember(dest => dest.Price,
                        options => options.MapFrom(source => source.price * .8m));
            }));

            IEnumerable<ComputerSnake>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);

            if(computersSystem != null)
            {
                IEnumerable<Computer> res = mapper.Map<IEnumerable<Computer>> (computersSystem);

                foreach (Computer computer in res)
                {
                    Console.WriteLine(computer.Motherboard + ": " + computer.Price);
                }
            }

            // string computersJSON = File.ReadAllText("Computers.json");

            // Console.WriteLine(computersJSON);

            //     JsonSerializerOptions options = new JsonSerializerOptions()
            //     {
            //         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            //     };

            //     IEnumerable<Computer>? computersNewtonSoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJSON);

            //     IEnumerable<Computer>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJSON, options);


            //     if (computersNewtonSoft != null)
            //     {
            //         foreach (Computer computer in computersNewtonSoft)
            //         {
            //             string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //                 Motherboard,
            //                 HasWifi,
            //                 HasLTE,
            //                 ReleaseDate,
            //                 Price,
            //                 VideoCard
            //             ) VALUES ('" + EscapeSingleQuote(computer.Motherboard) 
            //                     + "','" + computer.HasWifi
            //                     + "','" + computer.HasLTE
            //                     + "','" + computer.ReleaseDate?.ToString("yyyy-MM-dd")
            //                     + "','" + computer.Price
            //                     + "','" + EscapeSingleQuote(computer.VideoCard)
            //             + "')";

            //             dapper.ExecuteSql(sql);
            //         }
            //     }

            //     JsonSerializerSettings settings = new JsonSerializerSettings()
            //     {
            //         ContractResolver = new CamelCasePropertyNamesContractResolver()
            //     };

            //     string computersCopy = JsonConvert.SerializeObject(computersNewtonSoft, settings);

            //     File.WriteAllText("computersCopyNewtonsoft.txt", computersCopy);

            //     string computersCopySys = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);

            //     File.WriteAllText("computersCopySys.txt", computersCopySys);
        }

        // static string EscapeSingleQuote(string input)
        // {
        //     string output = input.Replace("'", "''");

        //     return output;
        // }
    }
}