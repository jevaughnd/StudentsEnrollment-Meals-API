using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enrollment_FrontEnd.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }


        [Display(Name = "Phone Number")]
        public int PhoneNumber { get; set; }



        //Relation
        public int ParishId { get; set; }
        public virtual Parish Parish { get; set; }



        //Relation
        public int ProgrammeId { get; set; }
        public virtual Programme Programme { get; set; }



        //Relation
        public int ShirtId { get; set; }
        public virtual Shirt Shirt { get; set; }



        //------file
        public string? StudentIdImageFilePath { get; set; }
    }
}
