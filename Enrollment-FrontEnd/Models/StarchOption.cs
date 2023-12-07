using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AmberEnrollmentAPI.Models
{
    public class StarchOption
    {
        public int Id { get; set; }




        [Display(Name = "Starch Option Name")]
        public string StarchName { get; set; }
    }
}