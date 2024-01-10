using FoodHouse.UI.Configuration;
using FoodHouse.UI.Models;
using FoodHouse.UI.Models.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FoodHouse.UI.Dto;
using FoodHouse.UI.Models.ViewModel.Courier;

namespace FoodHouse.UI.Controllers
{
    public class CourierController : Controller
    {
        private readonly ILogger<CourierController> _logger;
        private readonly HttpClient _httpClient;

        public CourierController(ILogger<CourierController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("APIHTTPS");
        }

        public async Task<IActionResult> Index()
        {
            CourierViewModel viewModel = new();
            var url = "Order/GetAll";
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>(url);

            viewModel.Orders = response.Where(u => u.OrderStatus != OrderStatus.Arrived).ToList();
            viewModel.UserName = Request.Cookies["UserName"];
            viewModel.Id = Request.Cookies["Id"];
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrder(IFormCollection collection)
        {
            var id = collection["Id"];
            var url = $"Order/Get?id={id}";
            var response = await _httpClient.GetFromJsonAsync<OrderDto>(url);
            
            response.OrderStatus = (OrderStatus)Int32.Parse(collection["orderStatus"]);
            url = "Order/Update";
            await _httpClient.PutAsJsonAsync(url,response);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TakeOrder(IFormCollection collection)
        {
            var id = Request.Cookies["UserType"];
            if (id != "Courier")
            {
                return RedirectToAction("Index", "Home");
            }
            var idOrder = collection["Id"];
            var url = $"Order/Get?id={idOrder}";
            var response = await _httpClient.GetFromJsonAsync<OrderDto>(url);
            
            response.CourierId = Request.Cookies["Id"];
            url = "Order/Update";
            var tyui = await _httpClient.PutAsJsonAsync(url, response);
            
            return RedirectToAction("Index");
        }
    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}