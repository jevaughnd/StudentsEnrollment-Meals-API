using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmberEnrollmentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentEnrollmentController : ControllerBase
    {

        private readonly ApplicationDbContext _cxt;

        public StudentEnrollmentController(ApplicationDbContext _context)
        {
            this._cxt = _context;
        }


        //STUDENT END POINTS
        [HttpGet("student")]
        public IActionResult GetStudents()
        {
            var pupils = _cxt.Students.Include(b => b.Parish)
                                      .Include(b => b.Programme)
                                      .Include(b => b.Shirt)
                                      .ToList();
            if (pupils == null)
            {
                return BadRequest();
            }

            //construct the img url for each student
            var baseUrl = "https://localhost:7293/images/";

            foreach (var student in pupils)
            {
                student.StudentIdImageFilePath = baseUrl + student.StudentIdImageFilePath;
              
            }
            return Ok(pupils);

        }


        //----------------------------
        //Finds Record where id is = to the result of the firstOrDefault query
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var pupil = _cxt.Students.Include(b => b.Parish)
                                     .Include(b => b.Programme)
                                     .Include(b => b.Shirt)
                                     .FirstOrDefault(x => x.Id == id); //gets individual students by id

            if (pupil == null)
            {
                return NotFound();
            }
            return Ok(pupil);
        }


        //To Create Student Record
        //----------------------
        [HttpPost("studentPost")]
        public IActionResult CreateStudent([FromBody] Student values)
        {
            _cxt.Students.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStudentById), new { id = values.Id }, values);
        }


        //-------------------------------
        //To Create ImageFile   //StudentCreateDTO
        [HttpPost("filePost")]
        public async Task<IActionResult> Create([FromForm] StudentCreateDTO model)
        {
            if (ModelState.IsValid)
            {   //take file from DTO
                var studentImageFile = model.StudentIdImageFile;

                if(studentImageFile != null && studentImageFile.Length > 0)
                {
                    //generate a unique file name
                    var uniqueFileName = Guid.NewGuid() + "_" + studentImageFile.FileName;

                    //define the final file path on the API server
                    var apiFilePath = Path.Combine("api","server", "StudentUploads", uniqueFileName);

                    //Save file to server
                    using (var stream = new FileStream(apiFilePath, FileMode.Create))
                    {
                        await studentImageFile.CopyToAsync(stream);
                    }


                    //Store the file path in the database along with other student details
                    var student = new Student
                    {
                        FullName = model.FullName,
                        EmailAddress = model.EmailAddress,
                        PhoneNumber = model.PhoneNumber,
                        ParishId = model.ParishId,
                        ProgrammeId = model.ProgrammeId,
                        ShirtId = model.ShirtId,
                        StudentIdImageFilePath = apiFilePath != String.Empty ? apiFilePath: "",
                    };

                    //Save The Student to the database
                    _cxt.Students.Add(student);
                    await _cxt.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetStudentById), new {id = student.Id },student);
                }
            }
            return BadRequest(ModelState);
        }

        //-----------------------------------
        //Retrieve a link to the uploaded file
        [HttpGet("files/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            // Construct the full path to the file based on the provided 'fileName'
            string filePath = Path.Combine("api", "server", "StudentUploads", fileName);


            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Or handle the case where the file doesn't exist
            }

            // Determine the content type based on the file's extension
            string contentType = GetContentType(fileName);


            // Return the image file as a FileStreamResult with the appropriate content type
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, contentType); // Adjust the content type as needed

        }
        private string GetContentType(string fileName)
        {
            // Determine the content type based on the file's extension
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream"; // Default to binary data
            }
        }







        //---------------------
        //Editing Student Record
        [HttpPut("studentPut")]
        public IActionResult UpdateStudent([FromBody] Student values)
        {
            //var pupil = _cxt.Students.FirstOrDefault(x => x.Id == id);

            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.Students.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStudentById), new { id = values.Id }, values);
        }


        //Deleting A Student Record
        //------------------------
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var pupil = _cxt.Students.Include(b => b.Parish).Include(b => b.Programme)
                                                            .Include(b => b.Shirt)
                                                            .FirstOrDefault(x => x.Id == id); //gets individual students by id

            if (pupil == null)
            {
                return NotFound();
            }

            _cxt.Students.Remove(pupil); _cxt.SaveChanges();
            return Ok(pupil);
        }










        //=========================================================================================================================//
        //PARISH END POINTS
        //-----------------------

        [HttpGet]
        [Route("Parish")]
        public IActionResult GetParishes()
        {
            var parItem = _cxt.Parishes.ToList();
            if (parItem == null)
            {
                return BadRequest();
            }
            return Ok(parItem);
        }
        //-----------------------

        //Finds Record where id is = to the result of the firstOrDefault query
        [HttpGet]
        [Route("Parish/{Id}")]
        public IActionResult GetParishById(int id)
        {
            var parItem = _cxt.Parishes.FirstOrDefault(x => x.Id == id); //gets individual parish by Id
            if (parItem == null)
            {
                return NotFound();
            }
            return Ok(parItem);
        }

        //-----------------------
        [HttpPost]
        [Route("ParishPost")]
        public IActionResult CreateParish([FromBody] Parish values)
        {
            _cxt.Parishes.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetParishById), new { id = values.Id }, values);
        }

        //-----------------------
        [HttpPut]
        [Route("ParishPut")]
        public IActionResult UpdateParish(int id, [FromBody] Parish values)
        {
            var parItem = _cxt.Parishes.FirstOrDefault(x => x.Id == id);
            if (parItem == null)
            {
                return NotFound();
            }
            _cxt.Parishes.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetParishById), new { id = values.Id }, values);
        }
        








        //=========================================================================================================================//
        //PROGRAMME END POINTS
        //-----------------------
        [HttpGet("programme")]
        public IActionResult GetProgrammes()
        {
            var progItem = _cxt.Programmes.ToList();
            if (progItem == null)
            {
                return BadRequest();
            }
            return Ok(progItem);
        }

        //-----------------------
        //Finds Record where id is = to the result of the firstOrDefault query
        [HttpGet("programme/{Id}")]
        public IActionResult GetProgrammeById(int id)
        {
            var progItem = _cxt.Programmes.FirstOrDefault(x => x.Id == id); //gets individual programme by Id

            if (progItem == null)
            {
                return NotFound();
            }
            return Ok(progItem);
        }

        //-----------------------
        [HttpPost("programmePost")]
        public IActionResult CreateProgramme([FromBody] Programme values)
        {
            _cxt.Programmes.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetProgrammeById), new { id = values.Id }, values);
        }

        //-----------------------
        [HttpPut("programmePut")]
        public IActionResult UpdateProgramme(int id, [FromBody] Programme values)
        {
            var progItem = _cxt.Programmes.FirstOrDefault(x => x.Id == id);
            if (progItem == null)
            {
                return NotFound();
            }
            _cxt.Programmes.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetProgrammeById), new { id = values.Id }, values);
        }






        //=========================================================================================================================//
        //SHIRT END POINTS

        //-----------------------
        [HttpGet("shirt")]
        public IActionResult GetShirt()
        {
            var shirtItem = _cxt.Shirts.ToList();
            if (shirtItem == null)
            {
                return BadRequest();
            }
            return Ok(shirtItem);
        }

        //-----------------------
        //Finds Record where id is = to the result of the firstOrDefault query
        [HttpGet("shirt/{Id}")]
        public IActionResult GetShirtById(int id)
        {
            var shirtItem = _cxt.Shirts.FirstOrDefault(x => x.Id == id); //gets individual shirt by Id

            if (shirtItem == null)
            {
                return NotFound();
            }
            return Ok(shirtItem);
        }

        //-----------------------
        [HttpPost("shirtPost")]
        public IActionResult CreateShirt([FromBody] Shirt values)
        {
            _cxt.Shirts.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetShirtById), new { id = values.Id }, values);
        }

        //-----------------------
        [HttpPut("shirtPut")]
        public IActionResult UpdateShirt(int id, [FromBody] Shirt values)
        {
            var shirtItem = _cxt.Shirts.FirstOrDefault(x => x.Id == id);
            if (shirtItem == null)
            {
                return NotFound();
            }
            _cxt.Shirts.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetShirtById), new { id = values.Id }, values);
        }
        //---------------------------------------------------------------------------
    }
}
