namespace HRSYS.Application.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public bool? IsActive { get; set; }
         public DateTime CreatedAt { get; set; } 

    }
}