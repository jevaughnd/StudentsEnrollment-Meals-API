using AmberEnrollmentAPI.Models;
using Enrollment_FrontEnd.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace Enrollment_FrontEnd.Controllers
{
    public class MeatController : Controller
    {
        const string BASE_URL = "https://localhost:7293/api/MeatAPI";

        public IActionResult Index()
        {

            var meatlist = new List<MeatOption>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/MeatOption").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    meatlist = JsonConvert.DeserializeObject<List<MeatOption>>(data);

                }
          
            }

            return View(meatlist);
        }


        //CREATE: GET
        [HttpGet]
        public IActionResult Create()
        {
            MeatOption meatoption = new MeatOption();

            using (HttpClient client = new HttpClient()) 
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }

            return View(meatoption);
        }

        //CREATE: POST
        [HttpPost]
        public IActionResult Create(MeatOption meatOption, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var meat = new MeatOption
            {
                Id = id,
                MeatName = meatOption.MeatName,
            };

            var json = JsonConvert.SerializeObject(meat);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage meatResponse = client.PostAsync($"{BASE_URL}/MeatOptionPost", data).Result;
            if (meatResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Create Meat Option");
                return View(meat);
            }
        }


         //------------------------------------------------------------
        //EDIT: GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            MeatOption meatoption = new MeatOption();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //Meet By {id}
                HttpResponseMessage meatResponse = client.GetAsync($"{BASE_URL}/{id}").Result;
                if ( meatResponse.IsSuccessStatusCode)
                {
                    var meatData = meatResponse.Content.ReadAsStringAsync().Result;
                    meatoption = JsonConvert.DeserializeObject<MeatOption>(meatData)!;
                }

                var meat = new MeatOption
                {
                    Id = meatoption.Id,
                    MeatName = meatoption.MeatName,
                };
            }

            return View(meatoption);
        }




        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(MeatOption meatOption)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var meat = new MeatOption
            {
                Id = meatOption.Id,
                MeatName = meatOption.MeatName,
            };



            var json = JsonConvert.SerializeObject(meat);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage meatResponse = client.PutAsync($"{BASE_URL}/MeatOptionPut", data).Result;
            if (meatResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Meat Option");
                return View(meat);
            }
        }


        //DETAIL
        public IActionResult Detail(int id)
        {
            MeatOption meatOption = new MeatOption();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    meatOption = JsonConvert.DeserializeObject<MeatOption>(data);

                }
            }
            return View(meatOption);
        }


        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            MeatOption meatOption = new MeatOption();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    meatOption = JsonConvert.DeserializeObject<MeatOption>(data);

                }
            }
            return View(meatOption);
        }



        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, MeatOption meatOption)
        {
            //MeatOption meatOption = new MeatOption();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    meatOption = JsonConvert.DeserializeObject<MeatOption>(data);

                }
            }
            return RedirectToAction("Index");
        }




    }
}
