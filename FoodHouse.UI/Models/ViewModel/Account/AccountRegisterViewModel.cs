using System.ComponentModel.DataAnnotations;

namespace FoodHouse.UI.Models.ViewModel.Account
{
	public class AccountRegisterViewModel
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Number {  get; set; }
		[Required]	
		public string Sex { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string PasswordConfirm { get; set; }


		public string FirstNameMessage { get; set; }
		public string LastNameMessage { get; set; }
		public string NumberMessage { get; set; }
		public string EmailMessage { get; set; }
		public string PasswordMessage {  get; set; }

	}
}




