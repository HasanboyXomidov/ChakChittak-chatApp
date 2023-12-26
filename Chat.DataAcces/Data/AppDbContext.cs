using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DataAcces.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        #region dbset

        public DbSet<ChatMessage> messages { get; set; }


        #endregion

        static public string _connectionString = "Server=localhost;Database=mychat_db;User Id=root;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, new MySqlServerVersion(new Version(8, 0, 11)));
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }
    }
}
