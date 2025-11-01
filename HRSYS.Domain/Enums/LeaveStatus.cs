

using System.Text.Json.Serialization;
namespace HRSYS.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LeaveStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }
}
