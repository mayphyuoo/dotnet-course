using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Intermediate.Models;
using Intermediate.Data;

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
            DataContextEF entityFramework = new DataContextEF(config);

            DateTime rightNow = dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");

            // Console.WriteLine(rightNow.ToString());

            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060"
            };

            // entityFramework.Add(myComputer);
            // entityFramework.SaveChanges();

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

            // Console.WriteLine(sql);

            // bool result = dapper.ExecuteSql(sql);

            // Console.WriteLine(result);

            string sqlSelect = @"
            SELECT 
                Computer.ComputerId,
                Computer.Motherboard,
                Computer.HasWifi,
                Computer.HasLTE,
                Computer.ReleaseDate,
                Computer.Price,
                Computer.VideoCard
            from TutorialAppSchema.Computer";

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

            Console.WriteLine("'ComputerId','Motherboard','HasWifi','HasLTE','ReleaseDate','Price','VideoCard'");

            foreach(Computer comp in computers)
            {
                Console.WriteLine("'" + comp.ComputerId
                    + "','" + comp.Motherboard
                    + "','" + comp.HasWifi
                    + "','" + comp.HasLTE
                    + "','" + comp.ReleaseDate.ToString("yyyy-MM-dd")
                    + "','" + comp.Price
                    + "','" + comp.VideoCard
                + "'");
            }

            IEnumerable<Computer>? computersEf = entityFramework.Computer?.ToList<Computer>();

            if(computersEf != null)
            {
                Console.WriteLine("'ComputerId','Motherboard','HasWifi','HasLTE','ReleaseDate','Price','VideoCard'");

                foreach(Computer comp in computersEf)
                {
                    Console.WriteLine("'" + comp.ComputerId
                        + "','" + comp.Motherboard
                        + "','" + comp.HasWifi
                        + "','" + comp.HasLTE
                        + "','" + comp.ReleaseDate.ToString("yyyy-MM-dd")
                        + "','" + comp.Price
                        + "','" + comp.VideoCard
                    + "'");
                }
            }

            // Console.WriteLine(myComputer.Motherboard);
            // Console.WriteLine(myComputer.ReleaseDate);
        }
    }
}