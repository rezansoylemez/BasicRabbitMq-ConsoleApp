using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqConsumer;

public  static class HeaderExchangeConsumer
{
    public static void Consume(IModel channel)
    {
        channel.ExchangeDeclare("test-header-exchange", ExchangeType.Headers);
        channel.QueueDeclare("test-header-queue",
        durable: true,
        exclusive: false, 
        autoDelete: false,
        arguments: null);

        var header = new Dictionary<string, object> { {"account", "new"}};

        channel.QueueBind("test-header-queue", "test-header-exchange",string.Empty,header);

        channel.BasicQos(0, 10, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        };

        channel.BasicConsume("test-header-queue", true, consumer);

        Console.WriteLine("Consumer Started");
        Console.ReadLine();
    }
}
