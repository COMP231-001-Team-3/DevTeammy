using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace teammy.Models
{
    class DataContext
    {
        private MongoClient client = new MongoClient("mongodb+srv://admin:t64K4vJ55FiHjFB@cluster0.hssub.mongodb.net/");
        public IMongoDatabase teammyDB { get; }

        public DataContext() 
        {
            teammyDB = client.GetDatabase("teammy");
        }
    }
}
