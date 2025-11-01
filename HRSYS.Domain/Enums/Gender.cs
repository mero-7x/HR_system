using System.Text.Json.Serialization;


namespace HRSYS.Domain.Enums
{ [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
