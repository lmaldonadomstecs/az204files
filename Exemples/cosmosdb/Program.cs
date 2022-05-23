using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace cosmosdb {
    public class Humain {
        public string id;
        public string name;
        public int age;
        public string pays;
    }

    public class Program {
        Humain h1 = new Humain {
            id = System.Guid.NewGuid().ToString(),
            name = "Kevin",
            age = 35,
            pays = "Brésil"
        };

        var function = @"function(document) {
            
            }";

        // Replace <documentEndpoint> with the information created earlier
        private static readonly string EndpointUri = "https://cosmosdblmaldonado.documents.azure.com:443/";
        // Set variable to the Primary Key from earlier.
        private static readonly string PrimaryKey = "6b7mpOXP5adUXGC5mSpuBB7gveknLQdSfgPNEx5ymdY6Pz8ON9CcowmifDKPPH3hQ1DGO15s2M8acxwSeckkHw==";
        // The Cosmos client instance
        private CosmosClient cosmosClient;
        // The database we will create
        private Database database;
        // The container we will create.
        private Container container;
        // The names of the database and container we will create
        private string databaseId = "az204Database1";
        private string containerId = "az204Container1";


        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                await p.CosmosAsync();
            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of program, press any key to exit.");
                Console.ReadKey();
            }
        }

        public async Task CosmosAsync()
        {
            // Create a new instance of the Cosmos Client
            // this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            this.cosmosClient = new CosmosClient("AccountEndpoint=https://cosmosdblmaldonado.documents.azure.com:443/;AccountKey=6b7mpOXP5adUXGC5mSpuBB7gveknLQdSfgPNEx5ymdY6Pz8ON9CcowmifDKPPH3hQ1DGO15s2M8acxwSeckkHw==;");

            // Runs the CreateDatabaseAsync method
            await this.CreateDatabaseAsync();

            // Run the CreateContainerAsync method
            await this.CreateContainerAsync();
        }
        private async Task CreateDatabaseAsync()
        {
            // Create a new database using the cosmosClient
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/nom");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);


            await this.container.CreateItemAsync<Humain>(h1);



        }
    }
}