using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlatereTracker.Database;

namespace AlatereTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DB db = new DB(Path.Combine(Path.GetFullPath("."), "db", "okabe.dbc"));

            await db.Connect();
            Console.WriteLine(db.Config);

            while (true) {
                try
                {
                    Table result = await db.Manager.Query(Console.ReadLine());
                    foreach (var row in result.Rows)
                    {
                        Console.WriteLine(string.Join("\n", row.Select(v => v ?? "NULL")));
                    }
                }
                catch {
                    Console.WriteLine("ERROR!");
                }
                

                Console.ReadKey();
                Console.Clear();
            }

           

            Console.Read();
        }
    }
}
