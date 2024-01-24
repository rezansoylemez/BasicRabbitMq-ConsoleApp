// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Serialization;

Console.WriteLine("Hello, World!");


var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("test-queue",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var message = new
{
    Name = "Producer",
    Message = "Hello!"
};

var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));


channel.BasicPublish("","test-queue",null, body);