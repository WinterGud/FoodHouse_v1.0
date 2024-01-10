using FoodHouse.Api.Entity;
using FoodHouse.Api.Repository.GenericRepository;
using FoodHouse.UI.Dto;
using FoodHouse.UI.Models.ViewModel.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FoodHouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        IGenericRepository<Reviews> reviewsRepository;

        public ReviewsController(IGenericRepository<Reviews> genericRepository)
        {
            reviewsRepository = genericRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(ReviewDto reviewsView)
        {
            Reviews review = new Reviews()
            {
                UserName = reviewsView.UserName,
                Description = reviewsView.Description,
                Title = reviewsView.Title,
                IsMan = reviewsView.IsMan
            };
            await reviewsRepository.Add(review);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IEnumerable<ReviewDto>> GetAll()
        {
            var data = reviewsRepository.GetAll().ToList();

            var reviews = new List<ReviewDto>();
            foreach (var item in data)
            {
                reviews.Add(new ReviewDto
                {
                    Id = item._id.ToString(),
                    UserName = item.UserName,
                    Description = item.Description,
                    Title = item.Title,
                    IsMan = item.IsMan
                });
            }
            return reviews;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ReviewDto> Get(ObjectId id)
        {
            var data =  await reviewsRepository.Get(id);

            if (data != null)
            {
                var review = new ReviewDto
                {
                    Id = data._id.ToString(),
                    UserName = data.UserName,
                    Description = data.Description,
                    Title = data.Title,
                    IsMan = data.IsMan
                };
                return review;
            }

            return new ReviewDto();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(string id)
        {
            await reviewsRepository.Delete(ObjectId.Parse(id));
        }

        [HttpPut]
        [Route("Edit")]
        public async Task Update(ReviewDto user)
        {
            Reviews review = new()
            {
                _id = ObjectId.Parse(user.Id),
                Title = user.Title,
                Description = user.Description,
                IsMan = user.IsMan,
                UserName = user.UserName
            };
            await reviewsRepository.Update(review);
        }
    }
}