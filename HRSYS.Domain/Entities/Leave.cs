using HRSYS.Domain.Enums;

namespace HRSYS.Domain.Entities
{
    public class Leave
    {
        public int Id { get; set; }
        public LeaveType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; } 
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; } 
        
        public int? ByManagerId { get; set; }
        public User? ByManager { get; set; }
        public DateTime? ManagerApprovalDate { get; set; }
          public DateTime? ManagerRejectDate { get; set; }
        public string? ManagerComments { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
