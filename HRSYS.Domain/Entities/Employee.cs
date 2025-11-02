using HRSYS.Domain.Entities;
using HRSYS.Domain.Enums
;
namespace HRSYS.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? ManagerId { get; set; }
        public User? Managername { get; set; }
        public Degree CurrentDegree { get; set; }
        public int ServiceInYears { get; set; }
        public Gender Gender { get; set; }
        public decimal BaseSalary { get; set; }
        
        public User? User { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    }
}
