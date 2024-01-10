namespace FoodHouse.UI.Models.ViewModel.Admin.Dishes
{
    public class ProductEditViewModel
    {
        public string UserName { get; set; }
        public string StringId { get; set; }
		public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Type { get; set; }
    }
}
