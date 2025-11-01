namespace HRSYS.Application.DTOs

{
    public class UserGetAllDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string FullName { get; set; }  = null!;
    public string Role { get; set; }  = null!;
    public bool IsActive { get; set; } 
}

}