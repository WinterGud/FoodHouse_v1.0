
namespace FoodHouse.UI.Dto;

public class UserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Number { get; set; }
    public string Sex { get; set; }
    public string UserType { get; set; }
    public List<ProductDto> Products { get; set; }
}
