using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqConsumer;

public  static class FanoutExchangeConsumer
{
    public static void Consume(IModel channel)
    {
        channel.ExchangeDeclare("test-fanout-exchange", ExchangeType.Fanout);
        channel.QueueDeclare("test-fanout-queue",
        durable: true,
        exclusive: false, 
        autoDelete: false,
        arguments: null);


        channel.QueueBind("test-fanout-queue", "test-fanout-exchange", string.Empty);

        channel.BasicQos(0, 10, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        };

        channel.BasicConsume("test-fanout-queue", true, consumer);

        Console.WriteLine("Consumer Started");
        Console.ReadLine();
    }
}
