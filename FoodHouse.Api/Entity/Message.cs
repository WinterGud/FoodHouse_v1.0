using FoodHouse.Api.Entity;
using MongoDB.Bson;

namespace Furni.Api.Entities
{
	public class Message : IEntity
	{
		public ObjectId _id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string MessageAboutProblem { get; set; }
	}
}
