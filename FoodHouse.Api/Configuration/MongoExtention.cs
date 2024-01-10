using FoodHouse.Api.Entity;
using FoodHouse.Api.Repository.GenericRepository;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using FoodHouse.Api.Settings;

namespace FoodHouse.Api.Configuration
{
	public static class MongoExtention
	{
		
		public static IServiceCollection AddMongo(this IServiceCollection services)
		{

			services.AddSingleton(serviceProvider =>
			{ 
				var configuration = serviceProvider.GetService<IConfiguration>();
				var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
				var mongoSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
				var mongoClient = new MongoClient(mongoSettings.ConnectionString());
				return mongoClient.GetDatabase(serviceSettings.ServiceName);
			});
			return services;
		}

		public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
		{
			services.AddSingleton<IGenericRepository<T>>(serviceProvider =>
			{
				var database = serviceProvider.GetService<IMongoDatabase>();
				return new GenericRepository<T>(database, collectionName);
			});

			return services;
		}
		

	}
}
