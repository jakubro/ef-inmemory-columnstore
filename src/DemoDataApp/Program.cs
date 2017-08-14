using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DemoDataApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<CodeFirst.DemoDataDbContext>());
            Database.SetInitializer(new NullDatabaseInitializer<DatabaseFirst.DemoDataDbContext>());

            Console.WriteLine(GetHeader("Code-first: Starting"));
            LogOnException(ExecuteQueryUsingCodeFirst);

            Console.WriteLine(GetHeader("Database-first: Starting"));
            LogOnException(ExecuteQueryUsingDatabaseFirst);

            Console.WriteLine(GetHeader("No EF: Starting"));
            LogOnException(ExecuteQueryWithoutEF);

            Console.WriteLine("Press key to exit ...");
            Console.ReadKey();
        }

        private static string GetHeader(string message)
        {
            var sb = new StringBuilder();
            var separator = string.Concat(Enumerable.Repeat("=", 100));

            sb.AppendLine();
            sb.AppendLine(separator);
            sb.AppendLine($"== {message} ".PadRight(100, '='));
            sb.AppendLine(separator);
            return sb.ToString();
        }

        private static void LogOnException(Action action)
        {
            try
            {
                action();
                Console.WriteLine("Result = OK");
            }
            catch (Exception e)
            {
                Console.WriteLine("Result = FAIL");
                Console.WriteLine(string.Join(Environment.NewLine,
                    $"  Type: {e.GetType()}",
                    $"  HResult: {e.HResult}",
                    $"  Message: {e.Message}"));
            }
        }

        private static void ExecuteQueryUsingCodeFirst()
        {
            using (var context = new CodeFirst.DemoDataDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var query = context.DemoDatas.Select(t => t);
                var result = query.ToList().Count;
                Debug.Assert(result == 0);
            }
        }

        private static void ExecuteQueryUsingDatabaseFirst()
        {
            using (var context = new DatabaseFirst.DemoDataDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var query = context.DemoDatas.Select(t => t);
                var result = query.ToList().Count;
                Debug.Assert(result == 0);
            }
        }

        private static void ExecuteQueryWithoutEF()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["demoDataDb:codeFirst"].ConnectionString;

            var query = @"SELECT
    [Extent1].[Id] AS [Id],
    [Extent1].[Date] AS [Date]
    FROM [dbo].[DemoDatas] AS [Extent1]";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                Console.WriteLine(command.ExecuteNonQuery());
            }
        }
    }
}