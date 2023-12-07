using AmberEnrollmentAPI.Models;
using Enrollment_FrontEnd.Models;
using Enrollment_FrontEnd.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Enrollment_FrontEnd.Controllers
{
    public class OrderMealController : Controller
    {
        const string BASE_URL = "https://localhost:7293/OrderMealAPI";
        
        const string STUDENT_ENDPOINT = "Student";
        const string PROGRAMME_ENDPOINT = "Programme";
        const string MEALTYPE_ENDPOINT = "MealType";


        
        //food
        const string Meat_Option_ENDPOINT = "MeatOption";
        const string Starch_Option_ENDPOINT = "StarchOption";
        const string Side_Option_ENDPOINT = "SideOption";
        const string Beverage_Option_ENDPOINT = "BeverageOption";





        public IActionResult Index()
        {
            
            var orderList = new List<MealOrder>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/MealOrder").Result;

                if(response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    orderList = JsonConvert.DeserializeObject<List<MealOrder>>(data);
                }
            }
            return View(orderList);

        }

      




        //---------------------------------------------------
        //CREATE: GET
        [HttpGet]
        public IActionResult Create()
        {
            MealOrder mealOrder = new MealOrder(); //golobal

            List<Student> studentList = new List<Student>();
            List<Programme> programmeList = new List<Programme>();
            List<MealType> mealTypeList= new List<MealType>();

            //food lists
            List<MeatOption> meatOptionList = new List<MeatOption>();
            List<StarchOption> starchOptionList = new List<StarchOption>();
            List<SideOption> sideOptionList = new List<SideOption>();
            List<BeverageOption> beverageOptionList = new List<BeverageOption>();
            
            
            using (HttpClient httpClient = new HttpClient()) 
            {
                httpClient.BaseAddress = new Uri($"{BASE_URL}"); ///
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Student
                HttpResponseMessage stuResponse = httpClient.GetAsync($"{BASE_URL}/{STUDENT_ENDPOINT}").Result;
                if (stuResponse.IsSuccessStatusCode)
                {
                    var stuData = stuResponse.Content.ReadAsStringAsync().Result;
                    studentList = JsonConvert.DeserializeObject<List<Student>>(stuData)!;
                }


                //Programme
                HttpResponseMessage proResponse = httpClient.GetAsync($"{BASE_URL}/{PROGRAMME_ENDPOINT}").Result;
                if (proResponse.IsSuccessStatusCode)
                {
                    var proData = proResponse.Content.ReadAsStringAsync().Result;
                    programmeList = JsonConvert.DeserializeObject<List<Programme>>(proData)!;
                }


                //MealType
                HttpResponseMessage mealResponse = httpClient.GetAsync($"{BASE_URL}/{MEALTYPE_ENDPOINT}").Result;
                if (mealResponse.IsSuccessStatusCode)
                {
                    var mealData = mealResponse.Content.ReadAsStringAsync().Result;
                    mealTypeList = JsonConvert.DeserializeObject<List<MealType>>(mealData)!;
                }



                //MeatOption
                HttpResponseMessage meatResponse = httpClient.GetAsync($"{BASE_URL}/{Meat_Option_ENDPOINT}").Result;
                if(meatResponse.IsSuccessStatusCode)
                {
                    var meatData = meatResponse.Content.ReadAsStringAsync().Result;
                    meatOptionList = JsonConvert.DeserializeObject<List<MeatOption>>(meatData)!;
                }

                //StarchOption
                HttpResponseMessage starchResponse = httpClient.GetAsync($"{BASE_URL}/{Starch_Option_ENDPOINT}").Result;
                if (starchResponse.IsSuccessStatusCode)
                {
                    var starchData = starchResponse.Content.ReadAsStringAsync().Result;
                     starchOptionList = JsonConvert.DeserializeObject<List<StarchOption>>(starchData)!;
                }


                
                //SideOption
                HttpResponseMessage sideResponse = httpClient.GetAsync($"{BASE_URL}/{Side_Option_ENDPOINT}").Result;
                if (sideResponse.IsSuccessStatusCode)
                {
                    var sideData = sideResponse.Content.ReadAsStringAsync().Result;
                    sideOptionList = JsonConvert.DeserializeObject<List<SideOption>>(sideData)!;
                }


                //BeverageOption
                HttpResponseMessage beverageResponse = httpClient.GetAsync($"{BASE_URL}/{Beverage_Option_ENDPOINT}").Result;
                if (beverageResponse.IsSuccessStatusCode)
                {
                    var beverageData = beverageResponse.Content.ReadAsStringAsync().Result;
                    beverageOptionList = JsonConvert.DeserializeObject<List<BeverageOption>>(beverageData)!;
                }






                //Shows Drop Down List Items
                var viewModel = new MealOrderVM
                {

                    //Student
                    StudentSelectList = studentList.Select(stu => new SelectListItem
                    {
                        Text = stu.FullName,
                        Value = stu.Id.ToString(),
                    }).ToList(),

                    //Programme
                    ProgrammeSelectList = programmeList.Select(prog => new SelectListItem
                    {
                        Text = prog.ProgrammeName,
                        Value = prog.Id.ToString(),
                    }).ToList(),


                    //MealType
                    MealItemsList = mealTypeList.Select(mealTypeList => new SelectListItem
                    {
                        Text = mealTypeList.MealTypeName,
                        Value = mealTypeList.Id.ToString(),
                    }).ToList(),

                    


                    //MeatOption
                    MeatItemsList = meatOptionList.Select(mItem => new SelectListItem
                    {
                        Text = mItem.MeatName,
                        Value = mItem.Id.ToString(),
                    }).ToList(),

                    //StarchOption
                    StarchItemsList = starchOptionList.Select(sItem => new SelectListItem
                    {
                        Text = sItem.StarchName,
                        Value = sItem.Id.ToString(),
                    }).ToList(),

                    //SideOption
                    SideItemsList = sideOptionList.Select(sItem => new SelectListItem
                    {
                        Text = sItem.SideName,
                        Value = sItem.Id.ToString(),
                    }).ToList(),

                    //BeverageOption
                    BeverageItemsList = beverageOptionList.Select(bItem => new SelectListItem
                    {
                        Text= bItem.BeverageName,
                        Value = bItem.Id.ToString(),
                    }).ToList()


                };
                return View(viewModel);
                 
            }

        }



        //CREATE: POST
        [HttpPost]
        public  IActionResult Create(MealOrderVM mealOrderVm, int id) /// the create method is similar to the update method below in EDIT method
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();


   

            var mealOrder = new MealOrder
            {
                Id = id,
                Date = mealOrderVm.Date,
                StudentId = mealOrderVm.SelectedStudentId,
                ProgrammeId = mealOrderVm.SelectedProgrammeId,
                MealTypeId = mealOrderVm.SelectedMealItemId,
                
                MeatOptionId = mealOrderVm.SelectedMeatItemId,
                StarchOptionId = mealOrderVm.SelectedStarchItemId,
                SideOptionId = mealOrderVm.SelectedSideItemId,
                BeverageOptionId = mealOrderVm.SelectedBeverageItemId,
                
               
            };


            var json = JsonConvert.SerializeObject(mealOrder);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            HttpResponseMessage mealResponse = client.PostAsync($"{BASE_URL}/MealOrder-Post", data).Result; ///Updates use put request 
            if (mealResponse.IsSuccessStatusCode)
            {
                return View(mealOrderVm);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Order Meal");
                return View(mealOrderVm);
            }
        }    ///End Create Method//------------------










        //----------------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            MealOrder mealOrder = new MealOrder(); //golobal

            List<Student> studentList = new List<Student>();
            List<Programme> programmeList = new List<Programme>();
            List<MealType> mealTypeList = new List<MealType>();

            //food lists
            List<MeatOption> meatOptionList = new List<MeatOption>();
            List<StarchOption> starchOptionList = new List<StarchOption>();
            List<SideOption> sideOptionList = new List<SideOption>();
            List<BeverageOption> beverageOptionList = new List<BeverageOption>();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"{BASE_URL}"); ///
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Student
                HttpResponseMessage stuResponse = httpClient.GetAsync($"{BASE_URL}/{STUDENT_ENDPOINT}").Result;
                if (stuResponse.IsSuccessStatusCode)
                {
                    var stuData = stuResponse.Content.ReadAsStringAsync().Result;
                    studentList = JsonConvert.DeserializeObject<List<Student>>(stuData)!;
                }

                //Programme
                HttpResponseMessage proResponse = httpClient.GetAsync($"{BASE_URL}/{PROGRAMME_ENDPOINT}").Result;
                if (proResponse.IsSuccessStatusCode)
                {
                    var proData = proResponse.Content.ReadAsStringAsync().Result;
                    programmeList = JsonConvert.DeserializeObject<List<Programme>>(proData)!;
                }


                //MealType
                HttpResponseMessage mealResponse = httpClient.GetAsync($"{BASE_URL}/{MEALTYPE_ENDPOINT}").Result;
                if (mealResponse.IsSuccessStatusCode)
                {
                    var mealData = mealResponse.Content.ReadAsStringAsync().Result;
                    mealTypeList = JsonConvert.DeserializeObject<List<MealType>>(mealData)!;
                }



                //MeatOption
                HttpResponseMessage meatResponse = httpClient.GetAsync($"{BASE_URL}/{Meat_Option_ENDPOINT}").Result;
                if (meatResponse.IsSuccessStatusCode)
                {
                    var meatData = meatResponse.Content.ReadAsStringAsync().Result;
                    meatOptionList = JsonConvert.DeserializeObject<List<MeatOption>>(meatData)!;
                }

                //StarchOption
                HttpResponseMessage starchResponse = httpClient.GetAsync($"{BASE_URL}/{Starch_Option_ENDPOINT}").Result;
                if (starchResponse.IsSuccessStatusCode)
                {
                    var starchData = starchResponse.Content.ReadAsStringAsync().Result;
                    starchOptionList = JsonConvert.DeserializeObject<List<StarchOption>>(starchData)!;
                }



                //SideOption
                HttpResponseMessage sideResponse = httpClient.GetAsync($"{BASE_URL}/{Side_Option_ENDPOINT}").Result;
                if (sideResponse.IsSuccessStatusCode)
                {
                    var sideData = sideResponse.Content.ReadAsStringAsync().Result;
                    sideOptionList = JsonConvert.DeserializeObject<List<SideOption>>(sideData)!;
                }


                //BeverageOption
                HttpResponseMessage beverageResponse = httpClient.GetAsync($"{BASE_URL}/{Beverage_Option_ENDPOINT}").Result;
                if (beverageResponse.IsSuccessStatusCode)
                {
                    var beverageData = beverageResponse.Content.ReadAsStringAsync().Result;
                    beverageOptionList = JsonConvert.DeserializeObject<List<BeverageOption>>(beverageData)!;
                }



                //Meal order List --------------------
                HttpResponseMessage mOrderResponse = httpClient.GetAsync($"{BASE_URL}/{id}").Result;
                if (mOrderResponse.IsSuccessStatusCode)
                {
                    var mOrderData = mOrderResponse.Content.ReadAsStringAsync().Result;
                    mealOrder = JsonConvert.DeserializeObject<MealOrder>(mOrderData)!;
                }


               
                //Shows Drop Down List Items
                var viewModel = new MealOrderVM
                {
                    Id = mealOrder.Id,
                    Date = mealOrder.Date,
                    SelectedStudentId = mealOrder.StudentId,
                    SelectedProgrammeId = mealOrder.ProgrammeId,
                    SelectedMealItemId = mealOrder.MealTypeId,

                    SelectedMeatItemId = mealOrder.MeatOptionId,
                    SelectedStarchItemId = mealOrder.StarchOptionId,
                    SelectedSideItemId = mealOrder.SideOptionId,
                    SelectedBeverageItemId = mealOrder.BeverageOptionId,


                     //Student
                    StudentSelectList = studentList.Select(stu => new SelectListItem
                    {
                        Text = stu.FullName,
                        Value = stu.Id.ToString(),
                    }).ToList(),

                    //Programme
                    ProgrammeSelectList = programmeList.Select(prog => new SelectListItem
                    {
                        Text = prog.ProgrammeName,
                        Value = prog.Id.ToString(),
                    }).ToList(),




                    //MealType
                    MealItemsList = mealTypeList.Select(mealTypeList => new SelectListItem
                    {
                        Text = mealTypeList.MealTypeName,
                        Value = mealTypeList.Id.ToString(),
                    }).ToList(),




                    //MeatOption
                    MeatItemsList = meatOptionList.Select(mItem => new SelectListItem
                    {
                        Text = mItem.MeatName,
                        Value = mItem.Id.ToString(),
                    }).ToList(),

                    //StarchOption
                    StarchItemsList = starchOptionList.Select(sItem => new SelectListItem
                    {
                        Text = sItem.StarchName,
                        Value = sItem.Id.ToString(),
                    }).ToList(),

                    //SideOption
                    SideItemsList = sideOptionList.Select(sItem => new SelectListItem
                    {
                        Text = sItem.SideName,
                        Value = sItem.Id.ToString(),
                    }).ToList(),

                    //BeverageOption
                    BeverageItemsList = beverageOptionList.Select(bItem => new SelectListItem
                    {
                        Text = bItem.BeverageName,
                        Value = bItem.Id.ToString(),
                    }).ToList()


                };
                return View(viewModel);

            }

        }






        //EDIT:POST

        [HttpPost]
        public IActionResult Edit(MealOrderVM mealOrderVm)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{BASE_URL}"); ///
            httpClient.DefaultRequestHeaders.Accept.Clear();
                


            //Shows Drop Down List Items
            var mealOrder = new MealOrder
            {
                Id = mealOrderVm.Id,
                Date = mealOrderVm.Date,
                StudentId = mealOrderVm.SelectedStudentId,
                ProgrammeId = mealOrderVm.SelectedProgrammeId,
                MealTypeId = mealOrderVm.SelectedMealItemId,

                MeatOptionId = mealOrderVm.SelectedMeatItemId,
                StarchOptionId = mealOrderVm.SelectedStarchItemId,
                SideOptionId = mealOrderVm.SelectedSideItemId,
                BeverageOptionId = mealOrderVm.SelectedBeverageItemId,

            };

            var json = JsonConvert.SerializeObject(mealOrder);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage mealResponse = httpClient.PutAsync($"{BASE_URL}/MealOrder-Put", data).Result; //Updates use put request 
            if (mealResponse.IsSuccessStatusCode)
            {
                return View("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to  Update Order ");
                return View(mealOrderVm);
            }

        }



        //DETAIL------------------------------------------------------
        [HttpGet]
        public IActionResult Detail(int id)
        {

            MealOrder MlOrder = new MealOrder();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    MlOrder = JsonConvert.DeserializeObject<MealOrder>(data);
                }
            }
            return View(MlOrder);

        }




        //DELETE:GET------------------------------------------------------
        [HttpGet]
        public IActionResult Delete(int id)
        {

            MealOrder MlOrder = new MealOrder();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    MlOrder = JsonConvert.DeserializeObject<MealOrder>(data);
                }
            }
            return View(MlOrder);

        }



        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, MealOrder mlOrder)
        {


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    mlOrder = JsonConvert.DeserializeObject<MealOrder>(data);
                }
            }
            return RedirectToAction("Index");

        }


        //------------------------------------------------------------------

        //to display menu imgs
        //-------------------
        const string Menu_URL = "https://localhost:7293/MenuAPI"; //swagger request url
        public IActionResult MenuDisplay()
        {
            var menuList = new List<Menu>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Menu_URL);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.GetAsync($"{Menu_URL}/Menu").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    menuList = JsonConvert.DeserializeObject<List<Menu>>(data);
                }
            }
            return View(menuList);
        }
    }
}
