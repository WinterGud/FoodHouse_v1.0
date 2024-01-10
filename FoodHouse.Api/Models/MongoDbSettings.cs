using MongoDB.Bson.Serialization.Serializers;

namespace FoodHouse.Api.Settings
{
	public class MongoDbSettings
	{
		public string Host { get; init; }
		public int Port { get; init; }
		public string ConnectionString() => $"mongodb://{Host}:{Port}";
	}
}
