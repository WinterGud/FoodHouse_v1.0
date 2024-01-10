using FoodHouse.UI.Configuration;
using FoodHouse.UI.Models;
using FoodHouse.UI.Models.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FoodHouse.UI.Dto;

namespace FoodHouse.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly Random _random;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _random = new();
            _httpClient = httpClientFactory.CreateClient("APIHTTPS");
        }

        public async Task<IActionResult> Index()
        {
            string url = "Reviews/GetAll";
            var reviews = await _httpClient.GetFromJsonAsync<List<ReviewDto>>(url);

            url = "Product/GetAll";
            var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>(url);

            if (products.Count() == 0)
            {
                url = "Product/Add";
                for (int i = 1; i <= 10; i++)
                {
                    var product = new ProductDto()
                    {
                        Image = Photo.Serealization,
                        Title = "Product " + i,
                        Description = "Descriprion for prroduct " + i,
                        Price = _random.Next(100, 200),
                        TypeProduct = (TypeProduct)_random.Next(0, 2),
                        Id = ""
                    };
                    products.Add(product);
                    await _httpClient.PostAsJsonAsync(url, product);
                }
            }

            if (reviews.Count() == 0)
            {
                url = "Reviews/Add";
                var product = new ReviewDto()
                {
                    Title = "Чудова їжа всі рекомндую",
                    Description = "Опис по замовчуванню...",
                    IsMan = true,
                    UserName = "Kozmenchuk Andrii",
                    Id = ""
                };
                
                var product1 = new ReviewDto()
                {
                    Title = "Чудова їжа всі рекомндую",
                    Description = "Опис по замовчуванню...",
                    IsMan = false,
                    UserName = "Kos Inna",
                    Id = ""
                };
                
                reviews.Add(product);
                reviews.Add(product1);
                
                await _httpClient.PostAsJsonAsync(url, product);
                await _httpClient.PostAsJsonAsync(url, product1);
            }


            var viewModel = new IndexHomeViewModel();
            viewModel.Reviews = reviews;
            viewModel.Products = products;

            var id = Request.Cookies["Id"];
            if (id != null)
            {
                viewModel.UserName = Request.Cookies["UserName"];
                viewModel.Sex = Request.Cookies["Sex"] == "female" ? true : false;
            }

            return View(viewModel);
        }

        public IActionResult Authorization()
        {
            if (Request.Cookies["Id"] != null)
            {
                return RedirectToAction("Error");
            }

            return View();
        }

        public IActionResult Reviews()
        {
            var name = Request.Cookies["UserName"];
            if (name == null)
            {
                return RedirectToAction("Login", "Autentificate");
            }

            ReviewsHomeViewModel viewModel = new();
            viewModel.UserName = name;
            viewModel.IsMan =
                Request.Cookies["Sex"] == "female"
                    ? true
                    : false; /// HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Reviews(ReviewsHomeViewModel viewModel)
        {
            ReviewDto dto = new ReviewDto()
            {
                Description = viewModel.Description,
                Title = viewModel.Title,
                IsMan = viewModel.IsMan,
                UserName = viewModel.UserName,
                Id = ""
            };

            string endpoint = "Reviews/Add";
            var response = await _httpClient.PostAsJsonAsync(endpoint, dto);


            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Basket()
        {
            var id = Request.Cookies["Id"];
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var endpoint = $"User/Get?id={id}";
            var data = await _httpClient.GetFromJsonAsync<UserDto>(endpoint);


            var viewModel = new BasketHomeViewModel();

            viewModel.Products = data.Products;

            foreach (var item in data.Products)
            {
                viewModel.PriceWithoutCoupon += (int)item.Price;

                if (Request.Cookies["Coupon"] != null)
                {
                    var proccent = Int32.Parse(Request.Cookies["Coupon"]);
                    viewModel.PriceWithCoupon +=
                        (int) (item.Price - (item.Price  / 100 * proccent));
                    continue;
                } 

                viewModel.PriceWithCoupon += (int)item.Price;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBacket(string id)
        {
            var userId = Request.Cookies["Id"];
            if (userId == null)
            {
            }

            var url = $"Product/Get?id={id}";
            var response = await _httpClient.GetFromJsonAsync<ProductDto>(url);

            url = $"User/Get?id={userId}";
            var user = await _httpClient.GetFromJsonAsync<UserDto>(url);

            user.Products.Add(response);

            url = "User/Update";
            await _httpClient.PutAsJsonAsync(url, user);

            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromBacket(IFormCollection collection)
        {
            var userId = Request.Cookies["Id"];
            if (userId == null)
            {
            }

            var url = $"User/Get?id={userId}";
            var user = await _httpClient.GetFromJsonAsync<UserDto>(url);

            user.Products = user.Products.Where(u => u.Id != collection["id"]).ToList();

            url = "User/Update";
            await _httpClient.PutAsJsonAsync(url, user);
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public async Task<IActionResult> DoOrder(BasketHomeViewModel viewModel)
        {
            var id = Request.Cookies["Id"];
            if (id == null)
            {
                return RedirectToAction("Login", "Autentificate");
            }

            var url = $"User/Get?id={id}";
            var user = await _httpClient.GetFromJsonAsync<UserDto>(url);

            if (user.Products.Count() == 0)
            {
                return RedirectToAction("Index");
            }

            var order = new OrderDto()
            {
                UserId = id,
                Products = user.Products,
                OrderStatus = OrderStatus.Progress,
                Total = (int)viewModel.PriceWithCoupon,
                Id = "",
                CourierId = ""
            };

            url = "Order/Add";
            var response = await _httpClient.PostAsJsonAsync(url, order);

            user.Products = new();
            url = "User/Update";
            await _httpClient.PutAsJsonAsync(url, user);
            
            Response.Cookies.Delete("Coupon");
            
            return RedirectToAction("Index");
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactHomeViewModel viewModel)
        {
            var data = new MessageDto()
            {
                Id = "",
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                MessageAboutProblem = viewModel.Message,
            };

            var endpoint = "Message/Add";
            var response = await _httpClient.PostAsJsonAsync<MessageDto>(endpoint, data);

            return RedirectToAction("Contact");
        }

        [HttpPost]
        public async Task<IActionResult> UseCoupon(IFormCollection collection)
        {
            var coupon = collection["coupon"].First();

            if (coupon == null)
                return RedirectToAction("Basket");
            

            var url = "Coupon/GetAll";
            var data = (await _httpClient.GetFromJsonAsync<List<CouponDto>>(url)).ToList().Where(u => u.Code == coupon);
            
            if(data.Count() == 0)
                return RedirectToAction("Basket");

            CookieOptions options = new();
            options.Expires = DateTimeOffset.Now.AddDays(1);
            
            Response.Cookies.Append("Coupon", data.First().Procent.ToString(), options);
            
            url = $"Coupon/Delete?id={data.First().Id}";
            await _httpClient.DeleteAsync(url);
            
            return RedirectToAction("Basket");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}