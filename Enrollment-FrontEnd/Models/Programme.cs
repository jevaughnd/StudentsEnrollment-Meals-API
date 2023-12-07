using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models
{
    public class Programme
    {
        public int Id { get; set; }


        [Display(Name = "Programme Name")]
        public string ProgrammeName { get; set; }



        [Display(Name = "Programme Description")]
        public string ProgrammeDescription { get; set; }
    }
}
