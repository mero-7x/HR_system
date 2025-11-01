using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs
{
    public class EmployeeDto

    {
        public int id { get; set; } 
        public string FullName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public decimal BaseSalary { get; set; }
        public int ServiceInYears { get; set; }
        public int TotalSalary
        {
            get
            {
                
                var rate = CurrentDegree switch
                {
                    Degree.NoDegree => 0.05m,
                    Degree.HighSchool => 0.10m,
                    Degree.Bachelor => 0.20m,
                    Degree.Master => 0.30m,
                    Degree.PhD => 0.40m,
                    _ => 0m
                };
                var bonus = (ServiceInYears / 4) * 100m;
                var allowance = BaseSalary * rate;
                return (int)(BaseSalary + allowance + bonus);
            }
        }


        public Gender Gender { get; set; }     // Male / Female
        public Degree CurrentDegree { get; set; }  // Bachelor / Master / PhD
        public Role? Role { get; set; }          // Employee / Manager / HR


        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
