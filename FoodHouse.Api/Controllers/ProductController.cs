using FoodHouse.Api.Entity;
using FoodHouse.Api.Repository.GenericRepository;
using FoodHouse.UI.Dto;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TypeProduct = FoodHouse.Api.Entity.TypeProduct;

namespace FoodHouse.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IGenericRepository<Product> _userRepository;

		public ProductController(IGenericRepository<Product> userRepository)
		{
			_userRepository = userRepository;
		}
		[HttpPost]
		[Route("Add")]
		public async Task Add(ProductDto productAdd)
		{
			Product product = new()
			{
				Image = productAdd.Image,
				Price = productAdd.Price,
				Title = productAdd.Title,
				Description = productAdd.Description,
				TypeProduct = (FoodHouse.Api.Entity.TypeProduct)((int)productAdd.TypeProduct) 
			};
			await _userRepository.Add(product);
		}
		[HttpGet]
		[Route("GetAll")]
		public async Task<List<ProductDto>> GetAll()
		{
			var data = _userRepository.GetAll().ToList();

			var products = new List<ProductDto>();

			foreach (var item in data)
			{
				products.Add(new()
				{
					Id = item._id.ToString(),	
					Image = item.Image,
					Title = item.Title,
					Price = item.Price,
					Description = item.Description,
					TypeProduct = (UI.Dto.TypeProduct)((int)item.TypeProduct)
				});
			}
			return products;
		}

		[HttpGet]
		[Route("Get")]
		public async Task<ProductDto> Get(string id)
		{
			var data = await _userRepository.Get(ObjectId.Parse(id));

			ProductDto product = new();
			if (data != null)
			{
				product.Price = data.Price;
				product.Image = data.Image;
				product.Title = data.Title;
				product.Id = data._id.ToString();
				product.Description = data.Description;
				product.TypeProduct = (UI.Dto.TypeProduct)((int)data.TypeProduct);
			}

			return product;
		}

		[HttpDelete]
		[Route("Delete")]
		public async Task Delete(string id)
		{
			await _userRepository.Delete(ObjectId.Parse(id));
		}

		[HttpPut]
		[Route("Update")]
		public async Task Update(ProductDto product)
		{
			var user = await Get(product.Id);

			Product prod = new Product()
			{
				_id = ObjectId.Parse(product.Id),
				Image = product.Image,
				Price = product.Price,
				Title = product.Title,
				Description = product.Description,
				TypeProduct = (FoodHouse.Api.Entity.TypeProduct)((int)product.TypeProduct)
			};
			await _userRepository.Update(prod);
		}
	}
}
