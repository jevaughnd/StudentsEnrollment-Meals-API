using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmberEnrollmentAPI.Models
{
    public class BeverageOption
    {
        [Key]
        public int Id { get; set; }

      


        [Column(TypeName = "varchar(20)")]
        public string BeverageName { get; set; }
    }
}
