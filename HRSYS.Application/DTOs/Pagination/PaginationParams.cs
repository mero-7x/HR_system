using HRSYS.Domain.Enums;

namespace HRSYS.Application.DTOs.Pagination
{
    public class PaginationParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsActive { get; set; }
        public string? SortBy { get; set; } = "Id";
        public bool Desc { get; set; } = false;
        public Role? Role { get; set; }
    }
}
