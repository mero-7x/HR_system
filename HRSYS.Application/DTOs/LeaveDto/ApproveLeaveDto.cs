
using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs

{
    public class ApproveLeaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore ]
        public int Id { get; set; }
        public LeaveStatus Status { get; set; }
         public string? RejectReason { get; set; }

        
    }
}