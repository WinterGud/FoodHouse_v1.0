using FoodHouse.Api.Entity;
using FoodHouse.Api.Repository.GenericRepository;
using FoodHouse.UI.Dto;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FoodHouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(UserDto userDto)
        {
            User user = new User(TypeUser.Client)
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Number = userDto.Number,
                Sex = userDto.Sex,
                Password = userDto.Password,
                Products = new()
            };

            foreach(var item in userDto.Products)
            {
                user.Products.Add(new()
                {
                    _id = ObjectId.Parse(item.Id),
                    Description = item.Description,
                    Title = item.Title,
                    Image = item.Image,
                    Price = item.Price,
                    TypeProduct = (Api.Entity.TypeProduct)(int)item.TypeProduct,
                });
            }
            await _userRepository.Add(user);
        }

        [HttpGet]
        [Route("GetAll")]
        public List<UserDto> GetAll()
        {
            var data = _userRepository.GetAll().ToList();

            var users = new List<UserDto>();
            foreach (var item in data)
            {
				var user = ConvertUserToUserDto(item);

				users.Add(user);
			}

            return users;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<UserDto> Get(string id)
        {
            var item = await _userRepository.Get(ObjectId.Parse(id));

            UserDto user = new();
            if (item != null)
            {
                user = ConvertUserToUserDto(item);
			}

            return user;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(string id)
        {
            await _userRepository.Delete(ObjectId.Parse(id));
        }

        [HttpPut]
        [Route("Update")]
        public async Task Update(UserDto userView)
        {
            User user = new User(TypeUser.Client)
            {
                _id = ObjectId.Parse(userView.Id),
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Sex = userView.Sex,
                Email = userView.Email,
                Password = userView.Password,
                Number = userView.Number,
                Products = new()
            };


			foreach (var item in userView.Products)
			{
				user.Products.Add(new()
				{
					_id = ObjectId.Parse(item.Id),
					Description = item.Description,
					Title = item.Title,
					Image = item.Image,
					Price = item.Price,
					TypeProduct = (Api.Entity.TypeProduct)(int)item.TypeProduct,
				});
			}

			await _userRepository.Update(user);
        }
		private UserDto ConvertUserToUserDto(User item)
		{
            var user = new UserDto()
            {
                Id = item._id.ToString(),
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Password = item.Password,
                UserType = item.TypeUser.ToString(),
                Number = item.Number,
                Sex = item.Sex,
                Products = new()
			};

			foreach (var item2 in item.Products)
			{
				user.Products.Add(new()
				{
					Id = item2._id.ToString(),
					Description = item2.Description,
					Title = item2.Title,
					Image = item2.Image,
					Price = item2.Price,
					TypeProduct = (UI.Dto.TypeProduct)(int)item2.TypeProduct,
				});
			}

			return user;
		}
	}
}