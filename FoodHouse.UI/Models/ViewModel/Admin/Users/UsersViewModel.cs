using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Models.ViewModel.Admin.Users
{
    public class UsersViewModel
    {
        public string UserName { get; set; }
        public string TypeUser { get; set; }
        public List<UserDto> Users { get; set; }
    }
}