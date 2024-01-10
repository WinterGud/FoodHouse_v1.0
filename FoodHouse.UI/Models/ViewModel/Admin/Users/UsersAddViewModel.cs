using System.ComponentModel.DataAnnotations;

namespace FoodHouse.UI.Models.ViewModel.Admin.Users
{
    public class UsersAddViewModel
    {
		public string UserName { get; set; }
		public string TypeUser { get; set; }

		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Number { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public string StringId { get; set; }


		public string FirstNameMessage { get; set; }
		public string LastNameMessage { get; set; }
		public string NumberMessage { get; set; }
		public string EmailMessage { get; set; }
		public string PasswordMessage { get; set; }
	}
}
