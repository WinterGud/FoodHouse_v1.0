using FoodHouse.Api.Entity;
using FoodHouse.Api.Repository.GenericRepository;
using FoodHouse.UI.Dto;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OrderStatus = FoodHouse.Api.Entity.OrderStatus;
using TypeProduct = FoodHouse.Api.Entity.TypeProduct;

namespace FoodHouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericRepository<Order> _orderRepository;

        public OrderController(IGenericRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(OrderDto orderDto)
        {
            var order = new Order()
            {
                UserId = ObjectId.Parse(orderDto.UserId),
                OrderStatus = (OrderStatus)((int)orderDto.OrderStatus),
                Products = new(),
                Total = orderDto.Total 
            };
            foreach (var product in orderDto.Products)
            {
                order.Products.Add(new Product()
                {
                    _id = ObjectId.Parse(product.Id),
                    Price = product.Price,
                    Image = product.Image,
                    Title = product.Title,
                    Description = product.Description
                });
            }

            await _orderRepository.Add(order);
        }

        [HttpGet]
        [Route("GetAll")]
        public List<OrderDto> GetAll()
        {
            var data = _orderRepository.GetAll().ToList();

            var orders = new List<OrderDto>();

            foreach (var item in data)
            {
                var order = new OrderDto()
                {
                    CourierId = item.CourierId.ToString(),
                    Id = item._id.ToString(),
                    UserId = item.UserId.ToString(),
                    Products = new(),
                    Total = (int)item.Total,
                    OrderStatus = (UI.Dto.OrderStatus)((int)item.OrderStatus)
                };
                foreach (var product in item.Products)
                {
                    var prod = new ProductDto();
                    
                    prod.Id = product._id.ToString();
                    prod.Price = product.Price;
                    prod.Image = product.Image;
                    prod.Title = product.Title;
                    prod.Description = product.Description;
                    prod.TypeProduct = (UI.Dto.TypeProduct)((int)product.TypeProduct);

                    order.Products.Add(prod);
                }

                orders.Add(order);
            }

            return orders;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<OrderDto> Get(string id)
        {
            var item = await _orderRepository.Get(ObjectId.Parse(id));
            var order = new OrderDto()
            {
                CourierId = item.CourierId.ToString(),
                Id = item._id.ToString(),
                UserId = item.UserId.ToString(),
                Products = new(),
                Total = (int)item.Total,
                OrderStatus = (UI.Dto.OrderStatus)((int)item.OrderStatus)
            };
            foreach (var product in item.Products)
            {
                var prod = new ProductDto();
                    
                prod.Id = product._id.ToString();
                prod.Price = product.Price;
                prod.Image = product.Image;
                prod.Title = product.Title;
                prod.Description = product.Description;
                prod.TypeProduct = (UI.Dto.TypeProduct)((int)product.TypeProduct);

                order.Products.Add(prod);
            }
            
            return order;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(string id)
        {
            await _orderRepository.Delete(ObjectId.Parse(id));
        }

        [HttpPut]
        [Route("Update")]
        public async Task Update(OrderDto orderDto)
        {
            var order = new Order()
            {
                _id = ObjectId.Parse(orderDto.Id),
                Total = orderDto.Total,
                CourierId = ObjectId.Parse(orderDto.CourierId),
                UserId = ObjectId.Parse(orderDto.UserId),
                OrderStatus = (OrderStatus)((int)orderDto.OrderStatus),
                Products = new()
            };
            foreach (var product in orderDto.Products)
            {
                order.Products.Add(new Product()
                {
                    _id = ObjectId.Parse(product.Id),
                    Price = product.Price,
                    Image = product.Image,
                    Title = product.Title,
                    Description = product.Description,
                    TypeProduct = TypeProduct.Cold
                });
            }

            await _orderRepository.Update(order);
        }
    }
}