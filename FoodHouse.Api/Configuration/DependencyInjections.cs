using FoodHouse.Api.Entity;
using Furni.Api.Entities;

namespace FoodHouse.Api.Configuration;

public static class DependencyInjections
{
    public static void AddDependency(this IServiceCollection service)
    {
        service.AddMongo()
            .AddMongoRepository<User>("users")
            .AddMongoRepository<Order>("orders")
            .AddMongoRepository<Reviews>("reviews")
            .AddMongoRepository<Product>("products")
            .AddMongoRepository<Message>("messages")
            .AddMongoRepository<Coupon>("coupons");
    }
}