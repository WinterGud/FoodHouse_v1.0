namespace FoodHouse.UI.Settings
{
	public static class RegexPatterns
	{
		public static string PasswordRegexPattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()-=_+{};':"",.<>?/\\]).{8,}$";
		public static string PhoneNumberRegexPatten = "^(0|\\+380)\\d{9,14}$";
		public static string NameRegexPattern = "^[a-zA-Z]{2,13}$";

		public static string AddMessage(bool isValidate, string message)
		{
			if (!isValidate)
				return message;
			return "";
		}
	}
}
