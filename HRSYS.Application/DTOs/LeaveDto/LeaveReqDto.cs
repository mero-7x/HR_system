using System;
using HRSYS.Domain.Enums;   

namespace HRSYS.Application.DTOs

{
    public class LeaveReqDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }

        public LeaveType Type { get; set; }
       
    }
}