using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AmberEnrollmentAPI.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        //student enroll
        public DbSet<Student> Students { get; set; }
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<Shirt> Shirts { get; set; }

        //student meal
        public DbSet<Menu> Menus { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<MealOrder> MealOrders { get; set; }

        //Food
        public DbSet<MeatOption> MeatOptions { get; set; }
        public DbSet<StarchOption> StarchOptions { get; set; }
        public DbSet<SideOption> SideOptions { get; set; }
        public DbSet<BeverageOption> BeverageOptions { get; set; }
    }
}
