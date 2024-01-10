using FoodHouse.Api.Entity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodHouse.Api.Repository.GenericRepository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : IEntity
	{
		private readonly IMongoCollection<T> mongoCollection;
		public GenericRepository(IMongoDatabase database, string collectionName)
		{
			mongoCollection = database.GetCollection<T>(collectionName);
		}
		public async Task Add(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			await mongoCollection.InsertOneAsync(entity);
		}

		public async Task Delete(ObjectId user)
		{
			await mongoCollection.DeleteOneAsync(u => u._id == user);
		}

		public async Task<T> Get(ObjectId id)
		{
			return await mongoCollection.Find(u => u._id == id).FirstOrDefaultAsync();
		}

		public IEnumerable<T> GetAll()
		{
			return mongoCollection.Find(new BsonDocument()).ToList();
		}

		public async Task Update(T user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			await mongoCollection.ReplaceOneAsync(u => u._id == user._id, user);
		}
	}
}
