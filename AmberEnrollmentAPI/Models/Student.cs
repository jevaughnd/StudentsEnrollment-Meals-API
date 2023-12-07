using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmberEnrollmentAPI.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string EmailAddress { get; set; }


        [Column(TypeName = "varchar(15)")]
        public int PhoneNumber { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? StudentIdImageFilePath { get; set; } = String.Empty;



        //Relation
        public int ParishId { get; set; }
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }



        //Relation
        public int ProgrammeId { get; set; }
        [ForeignKey("ProgrammeId")]
        public virtual Programme? Programme { get; set;}



        //Relation
        public int ShirtId { get; set; }
        [ForeignKey("ShirtId")]
        public virtual Shirt? Shirt { get;set; }
    }
}
