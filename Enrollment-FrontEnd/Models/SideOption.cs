using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AmberEnrollmentAPI.Models
{
    public class SideOption
    {
        public int Id { get; set; }



        [Display(Name = "Side Option Name")]
        public string SideName { get; set; }
    }
}