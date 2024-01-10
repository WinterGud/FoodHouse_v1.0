using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Home
{
    public class IndexHomeViewModel
    {
        public string UserName { get; set; }
        public bool Sex { get; set; }
        public List<ProductDto> Products { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
