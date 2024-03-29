﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMqConsumer;

public static class DirectExchangeConsumer
{
    public static void Consume(IModel channel)
    {
        channel.ExchangeDeclare("test-direct-exchange" , ExchangeType.Direct);
        channel.QueueDeclare("test-direct-queue",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

        channel.QueueBind("test-direct-queue", "test-direct-exchange", "account.init");

        channel.BasicQos(0, 10, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        };

        channel.BasicConsume("test-direct-queue",   true, consumer);

        Console.WriteLine("Consumer Started");
        Console.ReadLine();
    }
}
