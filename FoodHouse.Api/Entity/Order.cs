using MongoDB.Bson;

namespace FoodHouse.Api.Entity
{
    public class Order : IEntity
    {
        public ObjectId _id { get; set; }
        public ObjectId CourierId { get; set; }
        public ObjectId UserId { get; set; }
        public decimal Total { get; set; }
        public List<Product> Products { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public enum OrderStatus
    {
        Progress,
        Delivery,
        Arrived
    }
}