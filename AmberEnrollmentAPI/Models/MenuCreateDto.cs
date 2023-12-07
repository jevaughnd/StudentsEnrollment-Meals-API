using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmberEnrollmentAPI.Models
{
    public class MenuCreateDto
    {
      
        public string ItemName { get; set; }

        //relation
        public int ItemCategoryId { get; set; }

        //relation
        public int MealTypeId { get; set; }

        public IFormFile? MenuIdImageFile { get; set; } 
    }
}
