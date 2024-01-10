using System.ComponentModel.DataAnnotations;

namespace FoodHouse.UI.Models.ViewModel.Account
{
	public class AccountLoginViewModel
	{
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		public string EmailMessage { get; set; }
		public string PasswordMessage {  get; set; }
	}
}
