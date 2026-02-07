using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.DAL.SchoolContext
{

    /*
     IDesignTimeDbContextFactory:

     It enables Entity Framework Core tools to create instances of a DbContext class for design-time operations, such as:
        Generating database migrations
        Scaffolding CRUD (Create, Read, Update, Delete) controllers and views
     */

    public class DbContextFactory : IDesignTimeDbContextFactory<SchoolDbContext>
    {
        public SchoolDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<SchoolDbContext>();
            optionBuilder.UseSqlServer("Server=93.127.141.27,14330;Database=SchoolSystemDb;User Id=sa;Password=Noshahi@000;TrustServerCertificate=True;");

            return new SchoolDbContext(optionBuilder.Options);

        }
    }
}
