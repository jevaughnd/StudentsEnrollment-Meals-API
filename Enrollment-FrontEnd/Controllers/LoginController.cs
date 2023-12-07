using Enrollment_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Enrollment_FrontEnd.Controllers
{
    public class LoginController : Controller
    {
        const string AUTH_URL = "https://localhost:7293/api/AuthAPI";

        const string SESSION_AUTH = "AmberEnrollmentAPI";


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Retrieve JWT token from local storage
            string token = RetrieveTokenFromLocalStorage();
            if (string.IsNullOrEmpty(token))
            {
                //if the token is missing or expired
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet] //gets Login Page
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AUTH_URL);
                    string jsonContent = JsonConvert.SerializeObject(user);

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                    //send login to API
                    HttpResponseMessage response = await client.PostAsync($"{AUTH_URL}/login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

                        if (responseData.ContainsKey("status") && responseData["status"].ToString() == "succes")
                        {
                            if (responseData.ContainsKey("data"))
                            {

                                var token = responseData["data"].ToString();
                                HttpContext.Session.SetString(SESSION_AUTH, token);

                            }
                            TempData["message"] = "Login Successful"; // Succes message displayed in Home Index
                            return RedirectToAction("Index", "AddStudent");


                        }
                        else
                        {
                            //login faild
                            ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
                            return View(user);
                        }
                    }
                    else
                    {
                        //login faild
                        ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
                        return View(user);
                    }
                }
            }
            return View(user);
        }

        // Reference / called in index 
        private string RetrieveTokenFromLocalStorage()
        {
            string token = HttpContext.Session.GetString(SESSION_AUTH)!;
            return token;
        }

    }
}
