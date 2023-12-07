using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models.ViewModel
{
    public class EnrollVM
    {

        public int Id { get; set; }


        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }


        [Display(Name = "Phone Number")]
        public int PhoneNumber { get; set; }



        //Relation
        [Display(Name = "Parish")]
        public int ParishId { get; set; }


        //Relation
        [Display(Name = "Programme")]
        public int ProgrammeId { get; set; }


        //Relation
        [Display(Name = "Shirt Size")]
        public int ShirtId { get; set; }



        // select list values
        public List<SelectListItem>? ParishSelectlist { get; set; }
        public List<SelectListItem>? ProgrammeSelectList { get; set; }
        public List<SelectListItem>? ShirtSelectList { get; set;}


        //------------------------
        public int SelectedParishId { get; set; }
        public int SelectedProgrammeId { get; set; }
        public int SelectedShirtId { get;set; }


        //-------------
        public IFormFile? StudentIdImageFile { get; set; }


    }
}
