using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmberEnrollmentAPI.Models
{
    public class StudentCreateDTO
    {
       
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public int PhoneNumber { get; set; }
        

        //Relation
        public int ParishId { get; set; }
 
        //Relation
        public int ProgrammeId { get; set; }

        //Relation
        public int ShirtId { get; set; }


        public IFormFile? StudentIdImageFile { get; set; }

    }
}
