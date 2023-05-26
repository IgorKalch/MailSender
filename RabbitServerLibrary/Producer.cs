using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Net;
using System.Text;

namespace RabbitServerLibrary
{
    public class Producer
    {
        private readonly Settings _set;
        IConnection cnn;
        IModel channel;


        public Producer(Settings set)
        {
            _set = set;
        }

        public void OpenConnection()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(_set.ConnectionString);
            factory.ClientProvidedName = _set.ProducerName;

            cnn = factory.CreateConnection();
            channel = cnn.CreateModel();
        }

        public void AddToQueue(Mail mail)
        { 
            channel.ExchangeDeclare(_set.ExchangeName, ExchangeType.Fanout);
            channel.QueueDeclare(_set.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(_set.QueueName, _set.ExchangeName, _set.RoutingKey, null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mail));

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            channel.BasicPublish(_set.ExchangeName, _set.RoutingKey,  null, body);
        }

        public void CloseConnection ()  
        {
            channel.Close();
            cnn.Close();
        }
    }
}