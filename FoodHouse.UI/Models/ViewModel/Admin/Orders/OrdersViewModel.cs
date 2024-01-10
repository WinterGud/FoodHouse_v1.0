
using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Admin.Orders
{
    public class OrdersViewModel
    {
        public string UserName { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
