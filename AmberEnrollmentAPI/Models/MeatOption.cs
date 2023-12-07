using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmberEnrollmentAPI.Models
{
    public class MeatOption
    {
        [Key]
        public int Id { get; set; }

    

        [Column(TypeName = "varchar(20)")]
        public string MeatName { get; set; }
    }
}
