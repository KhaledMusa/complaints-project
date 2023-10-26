using Complains_MVCs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Complains_MVCs.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5159/")
            };
        }
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    List<FileComp> users = new List<FileComp>();
        //    string userId = HttpContext.Session.GetString("UserId");


        //    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"api/Users/Get/?userId={userId}").Result;



        //    if (response.IsSuccessStatusCode)
        //    {


        //        string data = response.Content.ReadAsStringAsync().Result;
        //        users = JsonConvert.DeserializeObject<List<FileComp>>(data);
        //    }
        //    return View(users);
        //}
        [HttpGet]
        public async Task<IActionResult> Index(int Id)
        {
            List<FileComp> users = new List<FileComp>();
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/Getcomplaints/{Id}");



            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<FileComp>>(data);
            }
            return View(users);
        }
    }
}
