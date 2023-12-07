using Enrollment_FrontEnd.Models;
using Enrollment_FrontEnd.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Enrollment_FrontEnd.Controllers
{
    public class AddStudentController : Controller
    {

        const string BASE_URL = "https://localhost:7293/StudentEnrollment"; //swagger link

        const string PARISH_ENDPOINT = "Parish";
        const string PROGRAMME_ENDPOINT = "programme";
        const string SHIRT_ENDPOINT = "shirt";
        


        //INDEX
        public IActionResult Index()
        {
            var studentList = new List<Student>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/Student").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    studentList = JsonConvert.DeserializeObject<List<Student>>(data);
                }
            }
            return View(studentList);
        }





        //------------------------------------------------
        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            Student student = new Student(); //Global variable
            List<Parish> parList = new List<Parish>();
            List<Programme> progList = new List<Programme>();
            List<Shirt> shirtList = new List<Shirt>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //parish
                HttpResponseMessage parResponse = client.GetAsync($"{BASE_URL}/{PARISH_ENDPOINT}").Result;
                if (parResponse.IsSuccessStatusCode)
                {
                    var parData = parResponse.Content.ReadAsStringAsync().Result;
                    parList = JsonConvert.DeserializeObject<List<Parish>>(parData)!;
                }

                //program
                HttpResponseMessage progResponse = client.GetAsync($"{BASE_URL}/{PROGRAMME_ENDPOINT}").Result;
                if (progResponse.IsSuccessStatusCode)
                {
                    var progData = progResponse.Content.ReadAsStringAsync().Result;
                    progList = JsonConvert.DeserializeObject<List<Programme>>(progData)!;
                }

                //shirt
                HttpResponseMessage shirtResponse = client.GetAsync($"{BASE_URL}/{SHIRT_ENDPOINT}").Result;
                if (shirtResponse.IsSuccessStatusCode)
                {
                    var shirtData = shirtResponse.Content.ReadAsStringAsync().Result;
                    shirtList = JsonConvert.DeserializeObject<List<Shirt>>(shirtData)!;
                }

                //for Drop down List in view model
                var viewModel = new EnrollVM
                {
                    //parish
                    ParishSelectlist = parList.Select(par => new SelectListItem
                    {
                        Text = par.ParishName,
                        Value = par.Id.ToString(),
                    }).ToList(),

                    //programme
                    ProgrammeSelectList = progList.Select(prog => new SelectListItem
                    {
                        Text = prog.ProgrammeName,
                        Value = prog.Id.ToString(),
                    }).ToList(),

                    //shirt
                    ShirtSelectList = shirtList.Select(shirt => new SelectListItem
                    {
                        Text = shirt.SizeName,
                        Value = shirt.Id.ToString(),

                    }).ToList()
                };
                return View(viewModel);
            }
        }
        




        //CREATE:POST
        [HttpPost]
        public async Task <IActionResult> Create(EnrollVM enrollVm, int id) /// the create method is similar to the update method below in EDIT method
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();


            //modified for image file upload
            if (!ModelState.IsValid) return View(enrollVm);

            var student = new StudentCreateDTO
            {
                //Id = id,
                FullName = enrollVm.FullName,
                EmailAddress = enrollVm.EmailAddress,
                PhoneNumber = enrollVm.PhoneNumber,
                ParishId = enrollVm.SelectedParishId,
                ProgrammeId = enrollVm.SelectedProgrammeId,
                ShirtId = enrollVm.SelectedShirtId,
                StudentIdImageFile = enrollVm.StudentIdImageFile
            };


            //var json = JsonConvert.SerializeObject(student);
            //var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            //HttpResponseMessage response = client.PutAsync($"{BASE_URL}/studentPut", data).Result; //Updates use put request 



            //Call the Method to Send the DTO with the info to the API
            var response = await SendStudentToApi(student);

            if (response.IsSuccessStatusCode)
            {
                return View(enrollVm);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Enroll Student");
                return View(enrollVm);
            }
        } ///End Create Method




        //Create:File Post //------------------
        private async Task<HttpResponseMessage> SendStudentToApi(StudentCreateDTO studentDto)
        {
            using (var httpClient = new HttpClient())
            {
                
                using (var formData = new MultipartFormDataContent())
                {
                    // Set the Content-Type header to "multipart/form-data"
                    formData.Headers.ContentType.MediaType = "multipart/form-data";


                    // Add student data to the request
                    formData.Add(new StringContent(studentDto.FullName), "FullName");
                    formData.Add(new StringContent(studentDto.EmailAddress.ToString()), "EmailAddress");
                    formData.Add(new StringContent(studentDto.PhoneNumber.ToString()), "PhoneNumber");
                    formData.Add(new StringContent(studentDto.ParishId.ToString()), "ParishId");
                    formData.Add(new StringContent(studentDto.ProgrammeId.ToString()), "ProgrammeId");
                    formData.Add(new StringContent(studentDto.ShirtId.ToString()), "ShirtId");

                    // Add the file to the request
                    if (studentDto.StudentIdImageFile != null && studentDto.StudentIdImageFile.Length > 0)
                    {
                        formData.Add(new StreamContent(studentDto.StudentIdImageFile.OpenReadStream())
                        {
                            Headers = { ContentLength = studentDto.StudentIdImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(
                                    studentDto.StudentIdImageFile.ContentType)
                            }

                        }, "StudentIdImageFile", studentDto.StudentIdImageFile.FileName);
                    }


                    //Send to API Code
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                    // Send the data to the API
                    return await httpClient.PostAsync($"{BASE_URL}/filePost", formData);
                }

            }

        }








        //----------------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student student = new Student(); //Global variable
            List<Parish> parList = new List<Parish>();
            List<Programme> progList = new List<Programme>();
            List<Shirt> shirtList = new List<Shirt>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //parish
                HttpResponseMessage parResponse = client.GetAsync($"{BASE_URL}/{PARISH_ENDPOINT}").Result;
                if (parResponse.IsSuccessStatusCode)
                {
                    var parData = parResponse.Content.ReadAsStringAsync().Result;
                    parList = JsonConvert.DeserializeObject<List<Parish>>(parData)!;
                }

                //programme
                HttpResponseMessage progResponse = client.GetAsync($"{BASE_URL}/{PROGRAMME_ENDPOINT}").Result;
                if (progResponse.IsSuccessStatusCode)
                {
                    var progData = progResponse.Content.ReadAsStringAsync().Result;
                    progList = JsonConvert.DeserializeObject<List<Programme>>(progData)!;
                }

                //shirt
                HttpResponseMessage shirtResponse = client.GetAsync($"{BASE_URL}/{SHIRT_ENDPOINT}").Result;
                if (shirtResponse.IsSuccessStatusCode)
                {
                    var shirtData = shirtResponse.Content.ReadAsStringAsync().Result;
                    shirtList = JsonConvert.DeserializeObject<List<Shirt>>(shirtData)!;
                }

                //student
                HttpResponseMessage studResponse = client.GetAsync($"{BASE_URL}/{id}").Result; // {id} shows values
                if (studResponse.IsSuccessStatusCode)
                {
                    var data = studResponse.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    student = JsonConvert.DeserializeObject<Student>(data)!;
                }


                
                var viewModel = new EnrollVM
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    EmailAddress = student.EmailAddress,
                    PhoneNumber = student.PhoneNumber,
                    SelectedParishId = student.ParishId,
                    SelectedProgrammeId = student.ProgrammeId,
                    SelectedShirtId = student.ShirtId,

                    //for Drop down List in view model
                    //parish
                    ParishSelectlist = parList.Select(par => new SelectListItem
                    {
                        Text = par.ParishName,
                        Value = par.Id.ToString(),
                    }).ToList(),

                    //programme
                    ProgrammeSelectList = progList.Select(prog => new SelectListItem
                    {
                        Text = prog.ProgrammeName,
                        Value = prog.Id.ToString(),
                    }).ToList(),

                    //shirt
                    ShirtSelectList = shirtList.Select(shirt => new SelectListItem
                    {
                        Text = shirt.SizeName,
                        Value = shirt.Id.ToString(),

                    }).ToList()
                };
                return View(viewModel);
            }
        }
        


        //EDIT:POST  //This is the method that UPDATES/Edits the record
        [HttpPost]
        public IActionResult Edit(EnrollVM enrollVm)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var stud = new Student
            {
                Id = enrollVm.Id,
                FullName = enrollVm.FullName,
                EmailAddress = enrollVm.EmailAddress,
                PhoneNumber = enrollVm.PhoneNumber,
                ParishId = enrollVm.SelectedParishId,
                ProgrammeId = enrollVm.SelectedProgrammeId,
                ShirtId = enrollVm.SelectedShirtId,
            };


            var json = JsonConvert.SerializeObject(stud);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage studResponse = client.PutAsync($"{BASE_URL}/studentPut", data).Result; //Updates use put request 
            if (studResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Student");
                return View(enrollVm);
            }
        }








        //DETAIL:GET //---------------------------------------------------------
        public IActionResult Detail(int id)
        {
            Student student = new Student();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}"); ///string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

             
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    student = JsonConvert.DeserializeObject<Student>(data);
                }
            }
            return View(student);

        } ///Similar to index but use Uri($"{BASE_URL}/{id}") for specific record, & change model from list to object
       




        //DELETE:GET --------------------------------------------------------------------
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Student student = new Student();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}"); ///string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    student = JsonConvert.DeserializeObject<Student>(data);
                }
            }
            return View(student);
        } ///delete get: exact same as detail get
        


        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, Student student) ///Similar to detail, but includes Student model
        {
            //Student student = new Student();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}"); //string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    student = JsonConvert.DeserializeObject<Student>(data);
                }
            }
            return RedirectToAction("Index");
        }




    }
}
