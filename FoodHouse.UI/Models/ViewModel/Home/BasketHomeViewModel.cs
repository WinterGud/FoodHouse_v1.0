using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Home
{
	public class BasketHomeViewModel
	{
		public int PriceWithoutCoupon { get; set; }
	    public int PriceWithCoupon { get; set; }
		public List<ProductDto> Products { get; set; }
        public BasketHomeViewModel()
        {
			PriceWithoutCoupon = 0;
			PriceWithCoupon = 0;
        }
    }
}
