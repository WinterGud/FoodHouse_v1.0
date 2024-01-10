namespace FoodHouse.UI.Dto;

public class ProductDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public TypeProduct TypeProduct { get; set; }
    public string Image { get; set; }
}
public enum TypeProduct
{
	Hot,
	Cold,
	Desert
} 