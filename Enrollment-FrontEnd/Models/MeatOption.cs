using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AmberEnrollmentAPI.Models
{
    public class MeatOption
    {
        public int Id { get; set; }



        [Display(Name = "Meat Option Name")]
        public string MeatName { get; set; }
    }
}