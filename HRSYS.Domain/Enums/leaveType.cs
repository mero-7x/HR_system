
using System.Text.Json.Serialization;


namespace HRSYS.Domain.Enums
{ [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LeaveType
    {
        Annual = 0,
        Sick = 1,
        Unpaid = 2,
        Personal = 3
    }
}
