using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransit
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocaldb;Database=ExcelDb;Trusted_Connection=True");
        }
        public DbSet<ExcelData> ExcelDatas { get; set; }
    }
    public class ExcelData
    {
        [Key]
        public string Id { get; set; }
        public string data { get; set; }
    }
}
