using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqProducer;

public static class TopicExchangePublisher
{
    public static void Publish(IModel channel)
    {
        var count = 0;

        var ttl = new Dictionary<string, object>
        {
            {"x-message-ttl", 30000 }
        };
        channel.ExchangeDeclare("test-topic-queue", ExchangeType.Topic, arguments: ttl);



        while (count < 10)
        {
            var message = new
            {
                Name = "Producer",
                Message = $"Hello! Count: {count} ",
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));


            channel.BasicPublish("test-topic-queue", "account.init", null, body);

            count++;
            Thread.Sleep(1000);
        }
    }
}
