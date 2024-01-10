using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Admin.Balance;

public class BalanceViewModel
{
    public string UserName { get; set; }
    public int Total { get; set; }
    public List<OrderDto> Orders { get; set; }
}