using System.Data;

using Dapper;
using Intermediate.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Intermediate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true;";

            IDbConnection dbConnection= new SqlConnection(connectionString);

            string sqlCommand = "SELECT GETDATE()";

            DateTime rightNow = dbConnection.QuerySingle<DateTime>(sqlCommand);

            Console.WriteLine(rightNow.ToString());

            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060"
            };

            string sql = @"INSERT INTO TutorialAppSchema.Computer (
                Motherboard,
                HasWifi,
                HasLTE,
                ReleaseDate,
                Price,
                VideoCard
            ) VALUES ('" + myComputer.Motherboard 
                    + "','" + myComputer.HasWifi
                    + "','" + myComputer.HasLTE
                    + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
                    + "','" + myComputer.Price
                    + "','" + myComputer.VideoCard
            + "')";

            Console.WriteLine(sql);

            int result = dbConnection.Execute(sql);

            Console.WriteLine(result);

            // Console.WriteLine(myComputer.Motherboard);
            // Console.WriteLine(myComputer.ReleaseDate);
        }
    }
}