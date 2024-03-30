using Auth.Mongo.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Auth.Mongo.Services
{
    public class MongoUsersService
    {
        private const string MONGO_DB_KEY = "MongoDB";

        public readonly IMongoCollection<User> _usersCollection;
        public MongoUsersService(IConfiguration config)
        {
            var databaseSettings = config.GetSection(MONGO_DB_KEY).Get<UsersDatabaseSettings>();
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(databaseSettings.CollectionName);
        }

        public async Task<bool> InsertAsync(User user)
        {
            if (await HasUser(user.Username))
            {
                return false;
            }
            else
            {
                await _usersCollection.InsertOneAsync(user);
                return true;
            }
        }

        public async Task<bool> HasUser(string username)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(user => user.Username, username);
            return (await _usersCollection.FindAsync(filter)).Any();
        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(user => user.Username, username),
                Builders<User>.Filter.Eq(user => user.Password, password)
            );
            return (await _usersCollection.FindAsync(filter)).Any();
        }
    }
}
