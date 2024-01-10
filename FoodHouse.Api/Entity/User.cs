using FoodHouse.UI.Dto;
using MongoDB.Bson;

namespace FoodHouse.Api.Entity
{
    public class User : IEntity
    {
        public ObjectId _id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Number { get; set; }
		public string Sex { get; set; }
		public TypeUser TypeUser { get; set; }
        public List<Product> Products { get; set; }
        public User(TypeUser typeUser)
        {
            TypeUser = typeUser;
        }

    }
    public enum TypeUser
    {
        Client,
        Courier,
        Admin
    }
}
