using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqTest;

public static class QueueProducer
{
    public static void Publish(IModel channel)
    {

        channel.QueueDeclare("test-queue",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

        var count = 0;


        while (count < 10)
        {
            var message = new
            {
                Name = "Producer",
                Message = $"Hello! Count: {count} ",
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));


            channel.BasicPublish("", "test-queue", null, body);

            count++;
            Thread.Sleep(1000);
        }
    }
}
