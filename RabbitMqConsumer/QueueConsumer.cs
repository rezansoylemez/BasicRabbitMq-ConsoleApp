using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMqConsumer;

public static class QueueConsumer
{
    public static void Consume(IModel channel)
    {
        channel.QueueDeclare("test-queue",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        };

        channel.BasicConsume("test-queue", true, consumer);

        Console.WriteLine("Consumer Started");
        Console.ReadLine();
    }
}
