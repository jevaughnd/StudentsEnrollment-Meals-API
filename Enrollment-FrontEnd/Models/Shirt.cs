using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models
{
    public class Shirt
    {
        public int Id { get; set; }


        [Display(Name = "Shirt Size")]
        public string SizeName { get; set; }



        [Display(Name = "Shirt Mesurment")]
        public string ShirtMesurments { get; set; }
    }
}
