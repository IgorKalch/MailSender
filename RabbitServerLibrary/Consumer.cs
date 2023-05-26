using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitServerLibrary.SendingModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RabbitServerLibrary
{
    public class Consumer
    {
        private readonly Settings _set;
        private readonly MethodExecution _methodExecution;
        IConnection _cnn;
        IModel _channel;


        public Consumer(Settings set)
        {
            _set = set;
            _methodExecution = new MethodExecution();
        }

        public void OpenConnection()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(_set.ConnectionString);
            factory.ClientProvidedName = _set.ConsumerName;

            _cnn = factory.CreateConnection();
            _channel = _cnn.CreateModel();
        }

        public void StartConsuming()
        {
                _channel.ExchangeDeclare(_set.ExchangeName, ExchangeType.Fanout);
                _channel.QueueDeclare(_set.QueueName, true, false, false, null);
                _channel.QueueBind(_set.QueueName, _set.ExchangeName, _set.RoutingKey, null);
                _channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var mail = JsonConvert.DeserializeObject<Mail>(json);

                    _methodExecution.Execute(mail, _set);

                    _channel.BasicAck(args.DeliveryTag, false);
                };

                _channel.BasicConsume(_set.QueueName, autoAck: false, consumer);

        }

        public void Close()
        {
            _channel.Close();
            _cnn.Close();
        }
    }
}
