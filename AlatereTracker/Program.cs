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
                var result = await db.Manager.Query("select date value,reason from mood");
                
                foreach (var r in result)
                {
                    Console.WriteLine(r.Key + "\n" + string.Join("\n", r.Value.Data));
                }

                Console.ReadKey();
                Console.Clear();
            }

           

            Console.Read();
        }
    }
}
