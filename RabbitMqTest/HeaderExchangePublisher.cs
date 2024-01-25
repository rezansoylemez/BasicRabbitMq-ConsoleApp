using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqProducer;

public static class HeaderExchangePublisher
{
    public static void Publish(IModel channel)
    {
        var count = 0;

        var ttl = new Dictionary<string, object>
        {
            {"x-message-ttl", 30000 }
        };
        channel.ExchangeDeclare("test-header-exchange", ExchangeType.Headers, arguments: ttl);



        while (count < 10)
        {
            var message = new
            {
                Name = "Producer",
                Message = $"Hello! Count: {count} ",
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object> { { "account", "new" } };

            channel.BasicPublish("test-header-exchange",string.Empty,properties, body);

            count++;
            Thread.Sleep(1000);
        }
    }
}
