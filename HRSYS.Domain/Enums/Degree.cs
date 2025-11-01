using System.Text.Json.Serialization;

namespace HRSYS.Domain.Enums
{ [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Degree
    {
        NoDegree = 5,
        HighSchool = 10,
        Bachelor = 20,
        Master = 30,
        PhD = 40
    }
}
