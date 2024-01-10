using FoodHouse.UI.Models.ViewModel.Admin.Dishes;
using FoodHouse.UI.Models.ViewModel.Admin.Orders;
using FoodHouse.UI.Models.ViewModel.Admin.Users;
using FoodHouse.UI.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using FoodHouse.UI.Dto;
using FoodHouse.UI.Models.ViewModel.Admin.Balance;
using FoodHouse.UI.Models.ViewModel.Admin.Coupons;
using FoodHouse.UI.Models.ViewModel.Admin.Messages;

namespace FoodHouse.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("APIHTTPS");
        }


        public async Task<IActionResult> Users()
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var viewModel = new UsersViewModel();

            string url = "User/GetAll";
            viewModel.Users = await _httpClient.GetFromJsonAsync<List<UserDto>>(url);
            viewModel.UserName = Request.Cookies["UserName"];

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var id = collection["userId"];
            string url = $"User/Get?id={id}";
            var data = await _httpClient.GetFromJsonAsync<UserDto>(url);


            UsersEditViewModel viewModel = new UsersEditViewModel()
            {
                UserName = Request.Cookies["UserName"],
                TypeUser = Request.Cookies["UserType"],
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                Number = data.Number,
                StringId = data.Id,
                Password = data.Password
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UsersEditViewModel viewModel)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
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


            if (number && password && lastName && firstName)
            {
                var user = new UserDto()
                {
                    Id = viewModel.StringId,
                    Email = viewModel.Email,
                    Number = viewModel.Number,
                    Password = viewModel.Password,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Products = new List<ProductDto>(),
                };

                var endpoint = "User/Update";

                var response = await _httpClient.PutAsJsonAsync(endpoint, user);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Users", "Admin");
                }
            }

            viewModel.NumberMessage =
                RegexPatterns.AddMessage(number, "Phone number must be 10-15 digits long and start with 0");
            viewModel.PasswordMessage = RegexPatterns.AddMessage(password,
                "Password must be 8+ chars and use EN lang, include uppercase and number");
            viewModel.FirstNameMessage = RegexPatterns.AddMessage(firstName, "First Name must be 2-13 chars");
            viewModel.LastNameMessage = RegexPatterns.AddMessage(lastName, "Last Name must be 2-13 chars");

            return RedirectToAction("EditUser");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }


            var id = collection["userId"];
            var adminId = Request.Cookies["Id"];

            string url = $"User/Delete?id={id}";

            await _httpClient.DeleteAsync(url);

            if (id == adminId)
            {
                return RedirectToAction("Logout", "Autentificate");
            }

            return RedirectToAction("Users", "Admin");
        }

        public async Task<IActionResult> Products()
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var viewModel = new ProductsViewModel();

            viewModel.UserName = Request.Cookies["UserName"];
            var url = "Product/GetAll";
            viewModel.Products = await _httpClient.GetFromJsonAsync<List<ProductDto>>(url);

            return View(viewModel);
        }

        public IActionResult AddProduct()
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            ProductAddViewModel viewModel = new ProductAddViewModel();
            viewModel.UserName = Request.Cookies["UserName"];

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductAddViewModel viewModel, IFormFile Image)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }


            if (Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                Image.OpenReadStream().CopyTo(memoryStream);

                ProductDto product = new ProductDto()
                {
                    Title = viewModel.Title,
                    Price = viewModel.Price,
                    Id = "",
                    Image = Convert.ToBase64String(memoryStream.ToArray()),
                    TypeProduct = (TypeProduct)viewModel.Type,
                    Description = viewModel.Description
                };

                string url = "Product/Add";
                await _httpClient.PostAsJsonAsync(url, product);

                return RedirectToAction("Products", "Admin");
            }
            else
            {
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }


            var id = collection["Id"];
            string url = $"Product/Get?id={id}";
            var data = await _httpClient.GetFromJsonAsync<ProductDto>(url);

            ProductEditViewModel viewModel = new ProductEditViewModel()
            {
                Description = data.Description,
                Title = data.Title,
                Price = data.Price,
                Image = data.Image,
                UserName = Request.Cookies["UserName"] ?? "",
                StringId = data.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductEditViewModel viewModel, IFormFile Image)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            if (Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                Image.OpenReadStream().CopyTo(memoryStream);

                ProductDto product = new ProductDto()
                {
                    Title = viewModel.Title,
                    Price = viewModel.Price,
                    Id = viewModel.StringId,
                    Image = Convert.ToBase64String(memoryStream.ToArray()),
                    TypeProduct = (TypeProduct)viewModel.Type,
                    Description = viewModel.Description
                };

                string url = "Product/Update";
                await _httpClient.PutAsJsonAsync(url, product);

                return RedirectToAction("Products", "Admin");
            }
            else
            {
                return RedirectToAction("EditProduct", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var id = collection["Id"];

            string url = $"Product/Delete?id={id}";
            await _httpClient.DeleteAsync(url);

            return RedirectToAction("Products", "Admin");
        }

        public async Task<IActionResult> Orders()
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var viewModel = new OrdersViewModel();
            viewModel.UserName = Request.Cookies["UserName"];

            var url = "Order/GetAll";
            viewModel.Orders = await _httpClient.GetFromJsonAsync<List<OrderDto>>(url);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var id = collection["Id"];

            string url = $"Order/Delete?id={id}";
            await _httpClient.DeleteAsync(url);

            return RedirectToAction("Orders", "Admin");
        }


        public async Task<IActionResult> Coupons()
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var viewModel = new CouponsViewModel();

            var url = "Coupon/GetAll";
            var coupons = await _httpClient.GetFromJsonAsync<List<CouponDto>>(url);

            viewModel.Coupons = coupons;

            return View(viewModel);
        }

        public IActionResult AddCoupon()
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var viewModel = new CouponAddViewModel();
            viewModel.UserName = Request.Cookies["UserName"];

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCoupon(CouponAddViewModel viewModel)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            viewModel.UserName = Request.Cookies["UserName"];

            var procent = 100;
            if (viewModel.Procent > procent)
            {
                return View(viewModel);
            }

            CouponDto coupon = new CouponDto()
            {
                Id = "",
                Code = viewModel.Coupon,
                Procent = viewModel.Procent,
            };

            string url = "Coupon/Add";
            await _httpClient.PostAsJsonAsync(url, coupon);


            return RedirectToAction("Coupons", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCoupon(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var id = collection["id"];
            string url = $"Coupon/Delete?id={id}";

            await _httpClient.DeleteAsync(url);

            return RedirectToAction("Coupons", "Admin");
        }

        public async Task<IActionResult> Messages()
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var url = "Message/GetAll";
            var messages = await _httpClient.GetFromJsonAsync<List<MessageDto>>(url);

            var viewModel = new MessageViewModel()
            {
                Messages = messages,
                UserName = Request.Cookies["UserName"]
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendAnswear(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var viewModel = new SendAnswearViewModel()
            {
                UserName = Request.Cookies["UserName"] ?? "" ,
                Email = collection["email"],
                Id = collection["Id"]
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendAnswearUser(SendAnswearViewModel viewModel)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            viewModel.UserName = "";
            var url = "Message/SendAnswear";
            var response = await _httpClient.PostAsJsonAsync(url, viewModel);

            return RedirectToAction("Messages", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(IFormCollection collection)
        {
            var typeUser = Request.Cookies["UserType"];
            if (typeUser != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }

            var url = $"Message/Delete?id={collection["id"]}";
            var response = await _httpClient.DeleteAsync(url);
            
            return RedirectToAction("Messages");
        }

        public async Task<IActionResult> Balance()
        {
            BalanceViewModel viewModel = new BalanceViewModel();

            var url = "Order/GetAll";
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>(url);
            viewModel.Orders = response.Where(u => u.OrderStatus == OrderStatus.Arrived).ToList();
            viewModel.UserName = Request.Cookies["UserName"];

            viewModel.Total = 0;
            foreach (var item in viewModel.Orders)
            {
                viewModel.Total += item.Total;
            }
            
            return View(viewModel);
        }
    }
}