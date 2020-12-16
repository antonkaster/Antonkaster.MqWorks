using Antonkaster.MqWorks;
using Antonkaster.MqWorks.MqClient;
using Antonkaster.MqWorks.MqServer;
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

            var basicListener = new MqBasicListener(connectionFactory);

            basicListener.StartListening(
                "test/test",
                new Action<string>(m => Console.WriteLine($"1: {m}")),
                null);
            basicListener.StartListening(
                "test/test2",
                new Action<string>(m => Console.WriteLine($"2: {m}")),
                null);


            Console.ReadLine();
        }
    }

    public class TestClass
    {
        public int A { get; set; }
        public string B { get; set; }

        public override string ToString()
        {
            return $"a = {A}; b = '{B}'";
        }
    }
}
