using FoodHouse.Api.Entity;
using FoodHouse.Api.Repository.GenericRepository;
using FoodHouse.UI.Dto;
using Furni.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Furni.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IGenericRepository<Coupon> _couponRepository;

        public CouponController(IGenericRepository<Coupon> userRepository)
        {
            _couponRepository = userRepository;
        }


        [HttpPost]
        [Route("Add")]
        public async Task Add(CouponDto couponDto)
        {
            Coupon coupon = new()
            {
                CouponCode = couponDto.Code,
                Procent = couponDto.Procent
            };
            await _couponRepository.Add(coupon);
        }

        [HttpGet]
        [Route("GetAll")]
        public List<CouponDto> GetAll()
        {
            var data = _couponRepository.GetAll().ToList();

            var coupons = new List<CouponDto>();

            foreach (var item in data)
            {
                coupons.Add(new CouponDto()
                {
                    Id = item._id.ToString(),
                    Code = item.CouponCode,
                    Procent = item.Procent
                });
            }

            return coupons;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<CouponDto> Get(string id)
        {
            var data = await _couponRepository.Get(ObjectId.Parse(id));

            var coupon = new CouponDto()
            {
                Id = data._id.ToString(),
                Code = data.CouponCode,
                Procent = data.Procent
            };

            return coupon;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(string id)
        {
            await _couponRepository.Delete(ObjectId.Parse(id));
        }

        [HttpPut]
        [Route("Edit")]
        public async Task Update(CouponDto orderDto)
        {
            Coupon coupon = new()
            {
                _id = ObjectId.Parse(orderDto.Id),
                CouponCode = orderDto.Code,
                Procent = orderDto.Procent
            };

            await _couponRepository.Update(coupon);
        }
    }
}