using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;


namespace RabbitMqProducer;

public static class DirectExchangePublisher
{
    public static void Publish(IModel channel)
    {
        var count = 0;
        channel.ExchangeDeclare("test-direct-queue",ExchangeType.Direct);



        while (count < 10)
        {
            var message = new
            {
                Name = "Producer",
                Message = $"Hello! Count: {count} ",
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));


            channel.BasicPublish("test-direct-queue", "account.init", null, body);

            count++;
            Thread.Sleep(1000);
        }
    }
}
