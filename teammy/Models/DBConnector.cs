using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace teammy.Models
{
    public class DBConnector
    {
        public static MongoClient client;
        public static IMongoDatabase Connect()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://admin:uCbzyjpqoPrwqgGX@cluster0.hssub.mongodb.net/teammy?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            settings.LinqProvider = LinqProvider.V3;
            client = new MongoClient(settings);
            return client.GetDatabase("teammy");
        }

    }
}
