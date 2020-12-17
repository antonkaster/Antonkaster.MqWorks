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

            basicListener
                .StartListening("test/testclass", new Action<TestClass>(m => Console.WriteLine($"TestClass: {m}")))
                .StartListening("test/string", new Action<string>(m => Console.WriteLine($"string: {m}")))
                .StartListening("test/int", new Action<int>(m => Console.WriteLine($"int: {m}")));


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
