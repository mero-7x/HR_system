using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs

{
    public class RejectLeaveDto
    {
        public int Id { get; set; }
        public LeaveStatus Status { get; set; }
        public string? Reason { get; set; }

        
    }
}