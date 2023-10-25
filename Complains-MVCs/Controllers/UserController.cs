using Complains_MVCs.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;

namespace Complains_MVCs.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController()
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
    [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FileComp comp)
        {
            try
            {
                // Serialize the FileComp object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(comp), Encoding.UTF8, "application/json");

                // Send a POST request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsync("api/files/upload", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally, you can handle a successful response here
                    // For example, you can redirect to a different view or action
                    return RedirectToAction("GetUserComplains");
                }
                else
                {
                    // Handle the case where the API request is not successful
                    // You might want to log an error or show an error message to the user
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., network errors, API unavailable, etc.
                // Log the exception or return an error view
                return View("Error");
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterReq req)
        {
            try
            {
                // Serialize the FileComp object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                // Send a POST request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsync("api/Registers/Login", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally, you can handle a successful response here
                    // For example, you can redirect to a different view or action
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle the case where the API request is not successful
                    // You might want to log an error or show an error message to the user
                    ModelState.AddModelError(string.Empty, "Login failed. Please try again later.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., network errors, API unavailable, etc.
                // Log the exception or return an error view
                return View("Error");
            }
        }

        //    [HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Rigester(User user)
        //{
        //    string data = JsonConvert.SerializeObject(user);
        //    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        //    HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "api/Registers/Register", content).Result;



        //    //here you have to go to login page after rigesteration





        //    if (response.IsSuccessStatusCode)
        //    {
        //        HttpContext.Session.SetString("UserId", user.Id.ToString());
        //        HttpContext.Session.SetString("UserName", user.UserName);

        //        HttpContext.Session.SetString("Email", user.Email);
        //        HttpContext.Session.SetString("Password", user.Password);
        //        HttpContext.Session.SetString("Role", user.TypeOfUser.ToString());





        //        TempData["Success"] = "Registration successful!";
        //        // Redirect to the login page
        //        return RedirectToAction("Login");
        //    }
        //    // Handle registration failure
        //    return View();
        //}


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Serialize the RegisterReq object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                    // Send a POST request to the API endpoint for registration
                    HttpResponseMessage response = await _httpClient.PostAsync("api/Registers/Register", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Registration was successful; you can redirect to a different action
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        // Handle the case where the registration request is not successful
                        // You might want to log an error or show an error message to the user
                        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., network errors, API unavailable, etc.
                    // Log the exception or return an error view
                    ModelState.AddModelError(string.Empty, "An error occurred during registration.");
                }
            }

            return View();
        }

       

        [HttpGet]
        public async Task<IActionResult> GetUserComplains(int Id)
        {
            List<FileComp> users = new List<FileComp>();
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/GetUserComplains/{Id}");



            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<FileComp>>(data);
            }
            return View(users);
        }
    }
}
