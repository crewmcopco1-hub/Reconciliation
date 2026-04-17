using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using VDSReconciliation.Models;

namespace VDSReconciliation.Functions
{
    public class PublishAuthorityMessageActivity
    {
        private readonly ServiceBusClient _client;
        private readonly ILogger<PublishAuthorityMessageActivity> _logger;

        public PublishAuthorityMessageActivity(ServiceBusClient client, ILogger<PublishAuthorityMessageActivity> logger)
        {
            _client = client;
            _logger = logger;
        }

        [Function("PublishAuthorityMessageActivity")]
        public async Task Run(
            [ActivityTrigger] MessageProcessingInput input)
        {
            var msg = input.Message;
            try
            {

                var topicName = input.TopicName;
                topicName = string.IsNullOrWhiteSpace(topicName) ? "ApplicationAccessRaw" : topicName;

                var sender = _client.CreateSender(topicName);

                var json = JsonSerializer.Serialize(msg);

                var sbMessage = new ServiceBusMessage(json)
                {
                    MessageId = msg.MessageId,
                    CorrelationId = msg.CorrelationId
                };

                sbMessage.ApplicationProperties["Source"] = msg.Source;

                await sender.SendMessageAsync(sbMessage);

                _logger.LogInformation(
                "{Component} - {Source} - {UserId} - {LocationId} - {ApplicationId}: Message published successfully. {@Info}",
                "VDSReconciliation",
                "PublishAuthorityMessageActivity",
                msg.Object.Attributes.user_id,
                msg.Object.Attributes.location_id,
                msg.Object.Attributes.application_id,
                new
                {
                    Status = "Success",
                    id = msg.MessageId,
                    CorrelationId = msg.CorrelationId,
                    TimeStampUTC = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "{Component} - {Source} - {UserId} - {LocationId} - {ApplicationId}: Failed to publish message. {@Info}",
                    "VDSReconciliation",
                    "PublishAuthorityMessageActivity",
                    msg.Object.Attributes.user_id,
                    msg.Object.Attributes.location_id,
                    msg.Object.Attributes.application_id,
                    new
                    {
                        Status = "Failed",
                        CorrelationId = msg.CorrelationId,
                        TimeStampUTC = DateTime.UtcNow
                    });

                throw;
            }
            
        }
    }
}
