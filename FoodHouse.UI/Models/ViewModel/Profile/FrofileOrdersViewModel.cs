
using FoodHouse.UI.Dto;
using FoodHouse.UI.Models.ViewModel.Admin.Orders;

namespace FoodHouse.UI.Models.ViewModel.Profile
{
    public class FrofileOrdersViewModel
    {
        public string UserName { get; set; }
        public string TypeUser { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
