using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs
{
    public class EmployeeCreateDto
{
    public string FullName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public decimal BaseSalary { get; set; }
    public int ServiceInYears { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string CurrentDegree { get; set; } = string.Empty;
    // public Role Role { get; set; }

    //User Acc
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

}

