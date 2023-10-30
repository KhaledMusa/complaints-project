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
using System.Net;

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


        [HttpGet]
        public async Task<IActionResult> Index(int Id)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            HttpResponseMessage response = null; // Declare the variable here



            if (userObjectJson !=null)
            {
                // Deserialize the JSON string to extract the user ID
                var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);



                // Access the user ID
                Id = userObject.Id;
                string role = userObject.TypeOfUser;



                if (role == "User")
                {
                    response = await _httpClient.GetAsync($"api/Users/GetUserComplains/{Id}");
                }
                else
                {
                    response = await _httpClient.GetAsync($"api/Users/Getcomplaints/{Id}");
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

                ViewBag.UserObjectJson = userObjectJson;
                return View();
                // Example of passing it to the view

            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Create(FileComp complaint)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);

            if (complaint.fileUp != null && complaint.fileUp.Length > 0 && ModelState.IsValid)
            {
                try
                {
                    // Save image to a specific directory within the project
                    string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    string uniqueId = Guid.NewGuid().ToString().Substring(0, 5);

                    // Save the file
                    string fileName = uniqueId + Path.GetExtension(complaint.fileUp.FileName);
                    string filePath = Path.Combine(uploadDirectory, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await complaint.fileUp.CopyToAsync(fileStream);
                    }

                    // Set the FileName property of the model to the file name
                    complaint.fileName = fileName;
                    complaint.UserId = userObject.Id;

                    // Serialize the complaint object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(complaint), Encoding.UTF8, "application/json");

                    // Send a POST request 
                    HttpResponseMessage response = await _httpClient.PostAsync("api/files/Create", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Extract data from response if necessary
                        var responseData = await response.Content.ReadAsStringAsync();

                        return RedirectToAction("Index"); // Replace "SuccessView" with your actual success view name
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // Handle validation errors if the API returns a 400 Bad Request
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, errorResponse);
                        return View(); // Display the view with validation errors
                    }
                    else
                    {
                        // Handle other API errors
                        ModelState.AddModelError(string.Empty, "API request failed with status code: " + response.StatusCode);
                        return View(); // Display a view with a general error message
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions here
                    ModelState.AddModelError(string.Empty, "Failed to create. Error: " + ex.Message);
                    return View();
                }
            }
            else
            {
                // Handle invalid model state or missing file
                ModelState.AddModelError(string.Empty, "Invalid model state or file is missing.");
                return View();
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(FileComp complaint)
        //{
        //    var userObjectJson = HttpContext.Session.GetString("UserObject");
        //    if (!string.IsNullOrEmpty(userObjectJson))
        //    {
        //        var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);
        //        complaint.UserId = userObject.Id; // Set the user's ID in the FileComp object
        //    }

        //    var jsonContent = new StringContent(JsonConvert.SerializeObject(complaint), Encoding.UTF8, "application/json");
        //    // Send a POST request 
        //    HttpResponseMessage response = await _httpClient.PostAsync("api/files/Create", jsonContent);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var userData = await response.Content.ReadAsStringAsync();
        //        var userObject = JsonConvert.DeserializeObject<FileComp>(userData);
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}




        [HttpGet]
        public IActionResult Login()
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            if (userObjectJson != null)
            {
                return RedirectToAction("Index");

            }

            return View();
        }


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




        [HttpGet]
        public IActionResult Register()
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            if (userObjectJson != null)
            {
                return RedirectToAction("Index");



            }
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
        public async Task<IActionResult> GetProfile(int Id)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            HttpResponseMessage response = null; // Declare the variable here



            if (!string.IsNullOrEmpty(userObjectJson))
            {
                // Deserialize the JSON string to extract the user ID
                var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);



                // Access the user ID
                int userId = userObject.Id;
                response = await _httpClient.GetAsync($"api/Users/GetProfile/{userId}");


                if (response.IsSuccessStatusCode)
                {

                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var UserData = JsonConvert.DeserializeObject<User>(jsonContent);



                    // Pass userObjectJson and complaintsData directly to the view
                    ViewBag.UserObjectJson = userObjectJson;
                    UserData.Id=userId;
                    return View(UserData);
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateComp(int id)
        {
            
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            if (!string.IsNullOrEmpty(userObjectJson))
            {

                var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);
                
                ViewBag.UserObjectJson = userObjectJson;
                        return View();
                // Example of passing it to the view

            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult<FileComp>> UpdateComp(FileComp file)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);
            file.UserId = userObject.Id;

            
                try
                {
                    

                   
                    if (!string.IsNullOrEmpty(userObjectJson))
                    {
                        // Deserialize the JSON string to extract the user ID
                        


                        file.UserId = userObject.Id;



                        // Serialize the RegisterReq object to JSON
                        var jsonContent = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");

                        // Send a POST request to the API endpoint for registration
                        var response = await _httpClient.PutAsync("api/files/UpdateComp", jsonContent);

                        if (response.IsSuccessStatusCode)
                        {
                            // Registration was successful; you can redirect to a different action
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            // Handle the case where the registration request is not successful
                            // You might want to log an error or show an error message to the user
                            ModelState.AddModelError(string.Empty, "Complaint Edit failed. Please try again.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., network errors, API unavailable, etc.
                    // Log the exception or return an error view
                    ModelState.AddModelError(string.Empty, "An error occurred during Edit Complaint .");
                }
            

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Remove the session variable
            return RedirectToAction("Login");
        }

        //[HttpGet]
        //public async Task<IActionResult> CheckedComp(int id)
        //{
        //    var userObjectJson = HttpContext.Session.GetString("UserObject");

        //    if (!string.IsNullOrEmpty(userObjectJson))
        //    {

                
        //        var userfile = JsonConvert.DeserializeObject<FileComp>(userObjectJson);

        //        ViewBag.UserObjectJson = userObjectJson;

        //        // Example of passing it to the view
        //        return View();
        //    }
        //    return RedirectToAction("Index");

        //}
        //[HttpPost]
        //public async Task<ActionResult<FileComp>> CheckedComp(FileComp file)
        //{
        //    var userObjectJson = HttpContext.Session.GetString("UserObject");
        //    var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);
            
            
        //    file.Status = "Accepted";
            


        //    try
        //    {



        //        if (!string.IsNullOrEmpty(userObjectJson))
        //        {
        //            // Deserialize the JSON string to extract the user ID



                    



        //            // Serialize the RegisterReq object to JSON
        //            var jsonContent = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");

        //            // Send a POST request to the API endpoint for registration
        //            var response = await _httpClient.PutAsync("api/files/Accepted", jsonContent);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Registration was successful; you can redirect to a different action
        //                return RedirectToAction("Index");
        //            }
        //            else
        //            {
        //                // Handle the case where the registration request is not successful
        //                // You might want to log an error or show an error message to the user
        //                ModelState.AddModelError(string.Empty, "Complaint Edit failed. Please try again.");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions, e.g., network errors, API unavailable, etc.
        //        // Log the exception or return an error view
        //        ModelState.AddModelError(string.Empty, "An error occurred during Edit Complaint .");
        //    }


        //    return View();
        //}
    }
}
