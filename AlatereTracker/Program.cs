using System;
using System.IO;
using System.Threading.Tasks;
using AlatereTracker.Database;

namespace AlatereTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DB db = new DB(Path.Combine(Path.GetFullPath("."), "db", "alatere.dbc"));

            await db.Connect();
            dynamic result = db.Manager.Query("");

            Console.WriteLine(db.Config);
            Console.WriteLine(result);

            Console.Read();
        }
    }
}
