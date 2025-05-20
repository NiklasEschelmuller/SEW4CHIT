// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("TestDB");
var collection = database.GetCollection<BsonDocument>("TestCollection");

var list = await collection.Find(new BsonDocument()).ToListAsync();

foreach (var document in list)
{
    foreach (var kvp in document)
    {
        Console.WriteLine(kvp.Name + ":" + kvp.Value);
    }
}


foreach (var document in list)
{
    //Console.WriteLine(document);
    string doc = document.ToString();
    dynamic person = JsonConvert.DeserializeObject(doc);
    Console.WriteLine("\n----------");
    foreach (var person1 in  person)
    {
        foreach (var pobertyInfo in person1.GetType().GetProperties() )
        {
            if(pobertyInfo.Name == "Name" || pobertyInfo.Name == "Value" )
            {
                string n = pobertyInfo.Name;
                var probertyName = pobertyInfo.Name;
                var probertyValue = pobertyInfo.GetValue(person1);
                Console.WriteLine($"{probertyName} ={probertyValue}");
            }
            
        }
    }
}
