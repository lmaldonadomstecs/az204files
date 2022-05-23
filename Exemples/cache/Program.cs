using StackExchange.Redis;
using System.Threading.Tasks;

namespace Cache {
    public class Program {
        // connection string to your Redis Cache 
        static string connectionString = "redislmaldonado.redis.cache.windows.net:6380,password=R7RnQ6NuchW5G5218WBw8kKnF6B3BcrhpAzCaFHlbxk=,ssl=True,abortConnect=False";

        static async Task Main(string[] args)
        {
            // The connection to the Azure Cache for Redis is managed by the ConnectionMultiplexer class.
            using (var cache = ConnectionMultiplexer.Connect(connectionString))
            {
                IDatabase db = cache.GetDatabase();




                // Snippet below executes a PING to test the server connection
                // var result = await db.ExecuteAsync("ping");
                // Console.WriteLine($"PING = {result.Type} : {result}");
                // Call StringSetAsync on the IDatabase object to set the key "test:key" to the value "100"
                bool setValue = await db.StringSetAsync("Universite:addresse", "Rue des chats qui danses 110");
                Console.WriteLine($"SET: {setValue}");
                bool setValue2 = await db.StringSetAsync("Universite:Population", 10000);
                Console.WriteLine($"SET: {setValue2}");
                bool setValue3 = await db.StringSetAsync("Universite:professeurs:1:nom", "Jean");
                Console.WriteLine($"SET: {setValue3}");
                // StringGetAsync takes the key to retrieve and return the value
                string getValue = await db.StringGetAsync("Universite:addresse");
                Console.WriteLine($"GET: {getValue}");
                string getValue2 = await db.StringGetAsync("Universite:Population");
                Console.WriteLine($"GET: {getValue2}");
                string getValue3 = await db.StringGetAsync("Universite");
                Console.WriteLine($"GET: {getValue3}");
            }
        }
    }
}
