using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VDSReconciliation.Models
{
    public class MessageProcessingInput
    {
        [JsonPropertyName("Message")]
        public required AuthorityEventMessage Message { get; set; }
        [JsonPropertyName("TopicName")]
        public required string TopicName { get; set; }
    }
}
