using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmberEnrollmentAPI.Models
{
    public class SideOption
    {
        [Key]
        public int Id { get; set; }

        //Relation
        //public int MealTypeId { get; set; }
        //[ForeignKey("MealTypeId")]
        //public virtual MealType? MealType { get; set; }



        [Column(TypeName = "varchar(20)")]
        public string SideName { get; set; }
    }
}
