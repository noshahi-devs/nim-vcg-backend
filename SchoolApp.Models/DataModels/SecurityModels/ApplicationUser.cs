using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models.DataModels.SecurityModels
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string? Name { get; set; }
        public IList<string> Role { get; set; } = [];
        public string Status { get; set; } = "Active";
        public string? CreatedOn { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public class UserRoleDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public IList<string>? Permissions { get; set; } = new List<string>();
    }

    public class AssignRoleDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public IList<string> Role { get; set; } = [];
    }

}
