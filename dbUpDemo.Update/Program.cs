using DbUp;
using DbUp.Engine;
using DbUp.Support;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace dbUpDemo.Update
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server=localhost;Database=dbUpDemo;Trusted_Connection=True;";

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgradeEngineBuilder = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransactionPerScript()
                .LogToConsole();

            var upgrader = upgradeEngineBuilder.Build();

            var result = upgrader.PerformUpgrade();

            // Display the result
            if (result.Successful)
            {
                Environment.ExitCode = (int)ExitCode.Success;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Update successful!");
            }
            else
            {
                Environment.ExitCode = (int)ExitCode.Error;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.WriteLine("Update failed, check logs for more information.");
            }
        }
    }
}
