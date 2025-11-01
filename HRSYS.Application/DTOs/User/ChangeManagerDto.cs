namespace HRSYS.Application.DTOs
{
    public class ChangeManagerDto
    {
        public int DepartmentId { get; set; }
        public int NewUserId { get; set; }
        // public int NewEmployeeId { get; set; }

        public string? Reason { get; set; }
    }
}
