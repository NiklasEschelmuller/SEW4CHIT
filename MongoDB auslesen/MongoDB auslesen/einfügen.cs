/*using MongoDB.Bson;
using MongoDB.Driver;

var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("TestDB");
var collection = database.GetCollection<BsonDocument>("TestCollection");

var testData = new[]
{
    new BsonDocument { { "name", "Alice" }, { "age", 28 } },
    new BsonDocument { { "name", "Bob" }, { "age", 34 } },
    new BsonDocument { { "name", "Charlie" }, { "age", 22 } }
};

await collection.InsertManyAsync(testData);

Console.WriteLine("Testdaten wurden eingefügt.");
*/