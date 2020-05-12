using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using noSQL.Helpers;

namespace noSQL.Common
{
    public class MongoDatabase : DatabaseBase
    {
        public MongoClient dbClient { get; private set; }
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> collection;

        public MongoDatabase(string address)
        {
            dbClient = new MongoClient(address);
        }

        public void setDatabase(string databaseName)
        {
            database = dbClient.GetDatabase(databaseName);
        }

        public void setCollection(string collectionName)
        {
            collection = database.GetCollection<BsonDocument>(collectionName);
        }
        public List<BsonDocument> getFilteredDocuments(FilterDefinition<BsonDocument> filter)
        {
            return collection.Find(filter).ToList();
        }
        public List<BsonDocument> getAllDocuments()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        public FilterDefinition<BsonDocument> getSimpleFilter(string field, string value)
        {
            return Builders<BsonDocument>.Filter.Eq(field, value);
        }
        public FilterDefinition<BsonDocument> getFilterForValidation(string login, string password)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("login", login) & builder.Eq("password", password);
            return filter;
        }
        public ReturnInfo CreateNewUser(string login, string password, UserRoles role)
        {
            if (getFilteredDocuments(getSimpleFilter("login", login)).Count != 0)
                return new ReturnInfo(false, "User exist");

            List<BsonElement> userDetails = new List<BsonElement> {
                    new BsonElement("login", login), new BsonElement("password", password), new BsonElement("role", role) };

            var newUser = new BsonDocument(userDetails);

            InsertUser(newUser);
            return new ReturnInfo(true);
        }

        private ReturnInfo InsertUser(BsonDocument user)
        {
            try
            {
                collection.InsertOne(user);
            }
            catch (Exception e)
            {
                return new ReturnInfo(false, e.Message);
            }

            return new ReturnInfo(true);
        }

        public int? GetUserRole(BsonDocument user)
        {
            BsonValue output;
            bool result = user.TryGetValue("role", out output);
            int? returnValue;
            try
            {
                returnValue = output.AsInt32;
            }
            catch
            {
                returnValue = null;
            }

            return returnValue;
        }
    }
}
