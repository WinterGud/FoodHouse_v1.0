using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Admin.Coupons
{
	public class CouponsViewModel
	{
		public string UserName { get; set; }
		public List<CouponDto> Coupons { get; set; }
	}
}
