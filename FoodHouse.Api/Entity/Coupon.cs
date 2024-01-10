using FoodHouse.Api.Entity;
using MongoDB.Bson;

namespace Furni.Api.Entities
{
	public class Coupon : IEntity
	{
		public ObjectId _id { get; set; }
		public string CouponCode {  get; set; }
		public int Procent {  get; set; }
    }
}
