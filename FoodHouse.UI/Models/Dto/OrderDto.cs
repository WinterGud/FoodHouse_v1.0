namespace FoodHouse.UI.Dto;

public class OrderDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string CourierId { get; set; }
    public int Total { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public List<ProductDto> Products { get; set; }
}

public enum OrderStatus
{
    Progress,
    Delivery,
    Arrived
}