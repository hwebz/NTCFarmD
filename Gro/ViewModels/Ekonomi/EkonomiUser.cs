using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gro.ViewModels.Ekonomi
{
    public class EkonomiUserModel
    {
        public string CustomerNumber { get; set; }
        public string Username { get; set; }
        public DateTime UtcTime { get; set; }

        [JsonProperty(PropertyName = "Rights")]
        public List<EkonomiUserRightsModel> Rights { get; set; }

    }
}