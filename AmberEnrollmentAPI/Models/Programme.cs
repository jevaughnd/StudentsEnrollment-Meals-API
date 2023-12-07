using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmberEnrollmentAPI.Models
{
    public class Programme
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string ProgrammeName { get; set; }



        [Column(TypeName = "varchar(200)")]
        public string ProgrammeDescription { get; set; }
    }
}
