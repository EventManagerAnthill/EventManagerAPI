using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Study.EventManager.Model.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CompanyTrialVersionEnum
    {
        ReadyToUseTrial = 1,
        AlreadyUsedTrial = 2
    }
}
