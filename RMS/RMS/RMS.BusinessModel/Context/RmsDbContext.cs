using System.Data.Entity;
using RMS.BusinessModel.Entities;

namespace RMS.BusinessModel.Context
{
    public class RmsDbContext : DbContext
    {
        public IDbSet<FoodCategory> FoodCategories { get; set; }
        public IDbSet<FoodItem> FoodItems { get; set; }

        public RmsDbContext() : base("DefaultConnection")
        {
            
        }

        public static RmsDbContext GetDbContext()
        {
           return new RmsDbContext();
        }
    }
}