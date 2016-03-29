using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier.Models
{
    public class AttachmentsModel
    {
        [JsonProperty("color")]
        public string Color;

        [JsonProperty("text")]
        public string Text;

        [JsonProperty("fallback")]
        public string Fallback;
    }
}
