using FoodHouse.UI.Models.ViewModel.Account;
using FoodHouse.UI.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Controllers
{
    public class AutentificateController : Controller
    {
        private readonly HttpClient httpClient;

        public AutentificateController(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("APIHTTPS");
        }

        public IActionResult Register()
        {
            if (Request.Cookies["Id"] != null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel viewModel)
        {
            bool number = Regex.IsMatch(viewModel.Number, RegexPatterns.PhoneNumberRegexPatten);
            bool password = Regex.IsMatch(viewModel.Password, RegexPatterns.PasswordRegexPattern);
            bool lastName = Regex.IsMatch(viewModel.LastName, RegexPatterns.NameRegexPattern);
            bool firstName = Regex.IsMatch(viewModel.FirstName, RegexPatterns.NameRegexPattern);

            viewModel.FirstNameMessage = string.Empty;
            viewModel.LastNameMessage = string.Empty;
            viewModel.NumberMessage = string.Empty;
            viewModel.EmailMessage = string.Empty;
            viewModel.PasswordMessage = string.Empty;


            string endpoint = "User/GetAll";

            var dataAboutAllUsers = await httpClient.GetFromJsonAsync<List<UserDto>>(endpoint);

            var data = dataAboutAllUsers.Where(u => u.Email == viewModel.Email);

            if (number && password && lastName && firstName && data.Count() == 0)
            {
                if (viewModel.Password == viewModel.PasswordConfirm)
                {
                    UserDto user = new UserDto()
                    {
                        Id = "",
                        Email = viewModel.Email,
                        Number = viewModel.Number,
                        Password = viewModel.Password,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Sex = viewModel.Sex,
                        UserType = "Client",
                        Products = new List<ProductDto>(),
                    };

                    endpoint = "User/Add";

                    var response = await httpClient.PostAsJsonAsync(endpoint, user);

                    if (response.IsSuccessStatusCode)
                    {
                        endpoint = "User/GetAll";
                        dataAboutAllUsers = await httpClient.GetFromJsonAsync<List<UserDto>>(endpoint);
                        var userData = dataAboutAllUsers.Where(u => u.Email == viewModel.Email).First();

                        CookieOptions cookie = new CookieOptions();
                        cookie.Expires = DateTime.Now.AddMinutes(60);

                        Response.Cookies.Append("Id", userData.Id, cookie);
                        Response.Cookies.Append("UserName", $"{userData.FirstName} {userData.LastName}", cookie);
                        Response.Cookies.Append("UserType", user.UserType, cookie);
                        Response.Cookies.Append("Sex", viewModel.Sex.ToString(), cookie);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            viewModel.NumberMessage =
                RegexPatterns.AddMessage(number, "Phone number must be 10-15 digits long and start with 0");
            viewModel.PasswordMessage = RegexPatterns.AddMessage(password,
                "Password must be 8+ chars and use EN lang, include uppercase and number");
            viewModel.FirstNameMessage = RegexPatterns.AddMessage(firstName, "First Name must be 2-13 chars");
            viewModel.LastNameMessage = RegexPatterns.AddMessage(lastName, "Last Name must be 2-13 chars");
            viewModel.PasswordMessage =
                RegexPatterns.AddMessage(viewModel.Password == viewModel.PasswordConfirm, " ") == ""
                    ? viewModel.PasswordMessage
                    : "Passwords do not match";
            viewModel.EmailMessage =
                RegexPatterns.AddMessage(data.First().Email != viewModel.Email, "This email already register");

            return View(viewModel);
        }

        public IActionResult Login()
        {
            if (Request.Cookies["Id"] != null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel viewModel)
        {
            viewModel.EmailMessage = string.Empty;
            viewModel.PasswordMessage = string.Empty;


            string endpoint = "User/GetAll";

            var dataAboutAllUsers = await httpClient.GetFromJsonAsync<List<UserDto>>(endpoint);

            var data = dataAboutAllUsers.Where(u => u.Email == viewModel.Email).First();


            if (data == null)
            {
                viewModel.EmailMessage = "This email not register";

                return View(viewModel);
            }

            if (viewModel.Password == data.Password)
            {
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddMinutes(60);

                Response.Cookies.Append("Id", data.Id, cookie);
                Response.Cookies.Append("UserName", $"{data.FirstName} {data.LastName}", cookie);
                Response.Cookies.Append("UserType", data.UserType, cookie);
                Response.Cookies.Append("Sex", data.Sex.ToString(), cookie);
                return RedirectToAction("Index", "Home");
            }

            viewModel.PasswordMessage = RegexPatterns.AddMessage(viewModel.Password == data.Password, "Wrong password");

            return View(viewModel);
        }


        public IActionResult Logout()
        {
            Response.Cookies.Delete("Id");
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("UserType");
            Response.Cookies.Delete("Sex");
            return Redirect("/");
        }
    }
}