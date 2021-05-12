using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Model.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventTypes
    {
        Public = 1,
        Private = 2
    }
}
