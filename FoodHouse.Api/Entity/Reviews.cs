using MongoDB.Bson;

namespace FoodHouse.Api.Entity
{
	public class Reviews : IEntity
	{
		public ObjectId _id { get; set; }
		public string UserName {  get; set; } 
		public string Title {  get; set; }
 		public string Description { get; set; }
		public bool IsMan {  get; set; }

	}
}
