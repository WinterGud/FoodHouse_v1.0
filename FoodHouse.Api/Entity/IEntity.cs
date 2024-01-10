using MongoDB.Bson;

namespace FoodHouse.Api.Entity
{
	public interface IEntity
	{
		ObjectId _id { get; set; }
	}
}
