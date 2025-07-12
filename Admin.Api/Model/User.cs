using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Admin.Api.Model;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
        
    [BsonElement("username")]
    public string UserName { get; set; } = string.Empty;
        
    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;
        
    [BsonElement("role")]
    public string Role { get; set; } = string.Empty;  
}