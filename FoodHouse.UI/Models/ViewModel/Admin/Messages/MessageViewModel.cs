using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Admin.Messages;

public class MessageViewModel
{
    public string UserName { get; set; }
    public List<MessageDto> Messages { get; set; }
}