using System.Text.RegularExpressions;
using FoodHouse.UI.Dto;
using FoodHouse.UI.Models.ViewModel.Profile;
using FoodHouse.UI.Settings;
using Microsoft.AspNetCore.Mvc;

namespace FoodHouse.UI.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly HttpClient _httpClient;

    public ProfileController(ILogger<ProfileController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("APIHTTPS");
    }

    public async Task<IActionResult> Index()
    {
        var id = Request.Cookies["Id"];
        if (id == null)
        {
            return RedirectToAction("Login", "Autentificate");
        }

        var url = "Order/GetAll";
        var orders = await _httpClient.GetFromJsonAsync<List<OrderDto>>(url);

        FrofileOrdersViewModel viewModel = new();
        viewModel.Orders = orders.Where(u => u.UserId == id && u.OrderStatus != OrderStatus.Arrived).ToList();
        viewModel.UserName = Request.Cookies["UserName"];
        viewModel.TypeUser = Request.Cookies["UserType"];

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrder(IFormCollection collection)
    {
        var id = Request.Cookies["Id"];
        if (id == null)
        {
            return RedirectToAction("Login", "Autentificate");
        }

        var idOrder = collection["Id"];
        var url = $"Order/Delete?id={idOrder}";
        var response = await _httpClient.DeleteAsync(url);
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Settings()
    {
        var id = Request.Cookies["Id"];

        if (id == null)
        {
            return RedirectToAction("Login", "Autentificate");
        }

        var userId = Request.Cookies["Id"];
        var url = $"User/Get?id={userId}";
        var response = await _httpClient.GetFromJsonAsync<UserDto>(url);


        ProfileSettingsViewModel viewModel = new ProfileSettingsViewModel()
        {
            UserName = Request.Cookies["UserName"],
            TypeUser = response.UserType,
            FirstName = response.FirstName,
            LastName = response.LastName,
            Email = response.Email,
            Number = response.Number,
            StringId = response.Id
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Settings(ProfileSettingsViewModel viewModel)
    {
        viewModel.TypeUser = Request.Cookies["UserType"];
        if (viewModel.TypeUser == null)
        {
            return RedirectToAction("Login", "Autentificate");
        }

        viewModel.UserName = Request.Cookies["UserName"];


        bool number = Regex.IsMatch(viewModel.Number, RegexPatterns.PhoneNumberRegexPatten);
        bool password = Regex.IsMatch(viewModel.Password, RegexPatterns.PasswordRegexPattern);
        bool lastName = Regex.IsMatch(viewModel.LastName, RegexPatterns.NameRegexPattern);
        bool firstName = Regex.IsMatch(viewModel.FirstName, RegexPatterns.NameRegexPattern);

        viewModel.FirstNameMessage = string.Empty;
        viewModel.LastNameMessage = string.Empty;
        viewModel.NumberMessage = string.Empty;
        viewModel.EmailMessage = string.Empty;
        viewModel.PasswordMessage = string.Empty;


        string endpoint = "User/Get";

        if (number && password && lastName && firstName)
        {
            UserDto user = new UserDto()
            {
                Id = viewModel.StringId,
                Email = viewModel.Email,
                Number = viewModel.Number,
                Password = viewModel.Password,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                UserType = viewModel.TypeUser,
                Products = new(),
                Sex = Request.Cookies["Sex"],
            };

            endpoint = "User/Update";

            var response = await _httpClient.PutAsJsonAsync(endpoint, user);

            if (response.IsSuccessStatusCode)
            {
                endpoint = "User/GetAll";
                var dataAboutAllUsers = await _httpClient.GetFromJsonAsync<List<UserDto>>(endpoint);
                var userData = dataAboutAllUsers.Where(u => u.Email == viewModel.Email).First();

                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddMinutes(60);

                Response.Cookies.Append("Id", userData.Id, cookie);
                Response.Cookies.Append("UserName", $"{userData.FirstName} {userData.LastName}", cookie);

                return RedirectToAction("Index", "Profile");
            }
        }

        viewModel.NumberMessage =
            RegexPatterns.AddMessage(number, "Phone number must be 10-15 digits long and start with 0");
        viewModel.PasswordMessage = RegexPatterns.AddMessage(password,
            "Password must be 8+ chars and use EN lang, include uppercase and number");
        viewModel.FirstNameMessage = RegexPatterns.AddMessage(firstName, "First Name must be 2-13 chars");
        viewModel.LastNameMessage = RegexPatterns.AddMessage(lastName, "Last Name must be 2-13 chars");
        
        return View(viewModel);
    }
}