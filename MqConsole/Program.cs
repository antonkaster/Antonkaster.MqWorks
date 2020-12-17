using Antonkaster.MqWorks;
using Antonkaster.MqWorks.MqClient;
using Antonkaster.MqWorks.MqServer.BasicListener;
using Antonkaster.MqWorks.MqServer.BasicRpcServer;
using RabbitMQ.Client;
using System;
using System.Text;

namespace MqConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = MqWorksFactory.CreateConnectionFactory(new MqConnectionConfig());

            new MqBasicListener(connectionFactory)
                .StartListening("test/testclass", new Action<TestClass>(m => Console.WriteLine($"TestClass: {m}")), autoDelete: true)
                .StartListening("test/string", new Action<string>(m => Console.WriteLine($"string: {m}")), autoDelete: true)
                .StartListening("test/int", new Action<int>(m => Console.WriteLine($"int: {m}")), autoDelete: true);

            new MqBasicRpcServer(connectionFactory)
                .StartListening<int, int>("test/getInt", m => m + 1)
                .StartListening<string, string>("test/getString", m => m + " " + m);

            Console.ReadLine();
        }
    }

    public class TestClass
    {
        public int A { get; set; }
        public string B { get; set; }

        public override string ToString()
        {
            return $"TestClasss: a = {A}; b = '{B}'";
        }
    }
}
