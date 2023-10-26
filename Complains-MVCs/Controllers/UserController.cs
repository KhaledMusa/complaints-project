using Complains_MVCs.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;
using NuGet.Protocol.Plugins;

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
        //[HttpGet]
        //public async Task<IActionResult> Index(int Id)
        //{
        //    var userObjectJson = HttpContext.Session.GetString("UserObject");

        //    List<FileComp> users = new List<FileComp>();
            

        //    HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/Getcomplaints/{Id}");


        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = await response.Content.ReadAsStringAsync();
        //       // users = JsonConvert.DeserializeObject<List<FileComp>>(data);
        //        var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);
        //        ViewBag.UserObject= userObjectJson;
        //        return View(userObject);

        //    }
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> Index(int Id)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            HttpResponseMessage response=null ; // Declare the variable here



            if (!string.IsNullOrEmpty(userObjectJson))
            {
                // Deserialize the JSON string to extract the user ID
                var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);



                // Access the user ID
                int userId = userObject.Id;
                string role = userObject.TypeOfUser;



                if (role == "user")
                {
                    response = await _httpClient.GetAsync($"api/Users/GetUserComplains/{userId}");
                }
                else
                {
                    response = await _httpClient.GetAsync($"api/Users/Getcomplaints/{userId}");
                }
                if (response.IsSuccessStatusCode)
                {

                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var complaintsData = JsonConvert.DeserializeObject<List<FileComp>>(jsonContent);



                    // Pass userObjectJson and complaintsData directly to the view
                    ViewBag.UserObjectJson = userObjectJson;
                    return View(complaintsData);
                }



                // Handle unsuccessful API response
                // Returning a simple message for illustration purposes
                return View();
            }



            // Handle the case where userObjectJson is empty or null (no user data in the session)
            // Redirect to a login page or handle the error as appropriate for your application
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            if (!string.IsNullOrEmpty(userObjectJson))
            {
                
                var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);

                ViewBag.UserObjectJson = userObject.Id;
                    return View();
                // Example of passing it to the view
                
            }
            return RedirectToAction("Index");

        }




        [HttpPost]

        public async Task<IActionResult> Create(FileComp comp)
        {

            var jsonContent = new StringContent(JsonConvert.SerializeObject(comp), Encoding.UTF8, "application/json");
            // Send a POST request 
            HttpResponseMessage response = await _httpClient.PostAsync("api/files/Create", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var userData = await response.Content.ReadAsStringAsync();
                var userObject = JsonConvert.DeserializeObject<FileComp>(userData);
                

                // Set the UserId to the user's ID


                return Ok(userObject);
                
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
           var userObjectJson= HttpContext.Session.GetString("userObjectJson");
            if(!string.IsNullOrEmpty(userObjectJson))
            {
                return RedirectToAction("Index");
                
            }

            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(RegisterReq req)
        //{
        //    try
        //    {

        //        var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");


        //        HttpResponseMessage response = await _httpClient.PostAsync("api/Registers/Login", jsonContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            /
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Login failed. Please try again later.");
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return View("Error");
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> Login(RegisterReq req)
        {

            if (ModelState.IsValid)
            {
               
                
                // Serialize the RegisterReq object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                // Send a POST request 
                HttpResponseMessage response = await _httpClient.PostAsync("api/Registers/Login", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var userData = await response.Content.ReadAsStringAsync();
                    var userObject = JsonConvert.DeserializeObject<User>(userData);
                    // Serialize the user object to JSON
                    var userJson = JsonConvert.SerializeObject(userObject);
                    // Store the JSON string in the session
                    HttpContext.Session.SetString("UserObject", userJson);
                    // Redirect to the Index action
                    return RedirectToAction("Index");
                }



                else
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
                    return View();
                }
            }
            return View();
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

       

        //[HttpGet]
        //public async Task<IActionResult> GetUserComplains(int Id)
        //{
        //    List<FileComp> users = new List<FileComp>();
        //    HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/GetUserComplains/{Id}");



        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = await response.Content.ReadAsStringAsync();
        //        users = JsonConvert.DeserializeObject<List<FileComp>>(data);
        //    }
        //    return View(users);
        //}

        [HttpGet]
        public async Task<IActionResult> GetProfile(int Id)
        {
            User users = new User();
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/GetProfile/{Id}");



            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<User>(data);
            }
            return View(users);
        }
        [HttpGet]
        public IActionResult UpdateComp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComp(int Id, FileComp req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Serialize the RegisterReq object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                    // Send a POST request to the API endpoint for registration
                    HttpResponseMessage response = await _httpClient.PostAsync($"api/FileComps/UpdateComp/{Id}", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Registration was successful; you can redirect to a different action
                        return RedirectToAction("GetUserComplains");
                    }
                    else
                    {
                        // Handle the case where the registration request is not successful
                        // You might want to log an error or show an error message to the user
                        ModelState.AddModelError(string.Empty, "Complaint Edit failed. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., network errors, API unavailable, etc.
                    // Log the exception or return an error view
                    ModelState.AddModelError(string.Empty, "An error occurred during Edit Complaint .");
                }
            }

            return View();
        }
    }
}
