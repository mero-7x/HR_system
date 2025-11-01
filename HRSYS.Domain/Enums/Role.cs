
using System.Text.Json.Serialization;
namespace HRSYS.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Employee = 0,
        Manager = 1,
        HR = 2 ,
        Pending= 3
    }
}
