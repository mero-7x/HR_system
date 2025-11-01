using HRSYS.Domain.Entities;
using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs
{
    public class ActivateUserDto
    {
        public int UserId { get; set; }
        public string fullname { get; set; } = null!;        
         public int DepartmentId { get; set; }
        public decimal BaseSalary { get; set; }
        public int ServiceInYears { get; set; }
        public Gender Gender { get; set; } 
        public Degree Degree { get; set; } 
        public Role Role { get; set; }               
    }
}
