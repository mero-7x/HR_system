using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs
{
    public class LeaveDto
    {
        
        public LeaveType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
