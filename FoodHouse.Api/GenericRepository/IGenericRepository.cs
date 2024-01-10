using MongoDB.Bson;

namespace FoodHouse.Api.Repository.GenericRepository
{
	public interface IGenericRepository<T>
	{

		IEnumerable<T> GetAll();
		Task Add(T user);
		Task Update(T user);
		Task<T> Get(ObjectId id);
		Task Delete(ObjectId objectId);
	}
}
