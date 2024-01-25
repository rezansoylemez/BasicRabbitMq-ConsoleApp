using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqConsumer;

public  static class TopicExchangeConsumer
{
    public static void Consume(IModel channel)
    {
        channel.ExchangeDeclare("test-topic-exchange", ExchangeType.Topic);
        channel.QueueDeclare("test-topic-queue",
        durable: true,
        exclusive: false, 
        autoDelete: false,
        arguments: null);

        channel.QueueBind("test-topic-queue", "test-topic-exchange", "account.*");

        channel.BasicQos(0, 10, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        };

        channel.BasicConsume("test-topic-queue", true, consumer);

        Console.WriteLine("Consumer Started");
        Console.ReadLine();
    }
}
