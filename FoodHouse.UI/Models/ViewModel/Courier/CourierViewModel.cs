using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Courier;

public class CourierViewModel
{
    public string UserName { get; set; }
    public string Id { get; set; }
    public List<OrderDto> Orders { get; set; }
}