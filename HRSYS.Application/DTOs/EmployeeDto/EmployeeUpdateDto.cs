using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs
{

    public class EmployeeUpdateDto
    {
        public int id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public decimal BaseSalary { get; set; }
        public int ServiceInYears { get; set; }
        public string CurrentDegree { get; set; } = string.Empty;

        public int TotalSalary
        {
            get
            {
                var degree = Enum.TryParse<Degree>(CurrentDegree, out var parsed) ? parsed : Degree.NoDegree;
                var rate = degree switch
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
    }


}
