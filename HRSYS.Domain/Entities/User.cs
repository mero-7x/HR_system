using System.ComponentModel.DataAnnotations.Schema;

using HRSYS.Domain.Enums;

namespace HRSYS.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }


        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Role Role { get; set; }  

        public int? EmployeeId { get; set; }
        // [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        
        public bool IsActive { get; set; } 
        public DateTime CreatedAt { get; set; } 

        public Department? ManagedDepartment { get; set; }
    }
}
