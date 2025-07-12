using MongoDB.Driver;
using Admin.Api.Model;
using Admin.Api.Configurations;
using Microsoft.Extensions.Options;
using Admin.Api.Repositories.Interfaces;

namespace Admin.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _usersCollection = mongoDatabase.GetCollection<User>(mongoDbSettings.Value.CollectionName);
    }
    
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _usersCollection.Find(_ => true).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _usersCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();
    }

    public async Task CreateUserAsync(User user)
    {
        await _usersCollection.InsertOneAsync(user);
    }

    public async Task UpdateUserAsync(string id, User user)
    {
        await _usersCollection.ReplaceOneAsync(x => x.Id == id, user);
    }

    public async Task DeleteUserAsync(string id)
    {
        await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }
}