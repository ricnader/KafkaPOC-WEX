using Confluent.Kafka;
using System.Text.Json;
using System.Diagnostics;

namespace KafkaConsumerPOC
{
    public class KafkaConsumerService: IHostedService
    {
        private readonly string topic = "test";
        private readonly string groupId = "test_group";
        private readonly string bootstrapServers = "localhost:29092";



        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(topic);
                    var cancelToken = new CancellationTokenSource();

                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume
                               (cancelToken.Token);
                            var orderRequest = JsonSerializer.Deserialize
                                <OrderProcessingRequest>
                                    (consumer.Message.Value);
                            Debug.WriteLine($"Processing Order Id:{ orderRequest.OrderId}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
