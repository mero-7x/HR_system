namespace HRSYS.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }


        public int? ManagerId { get; set; }        
        public int? ManagerEmployeeId { get; set; } 
        public User? Manager { get; set; }
        public Employee? ManagerEmployee { get; set; }  



        // Navigation Property
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
