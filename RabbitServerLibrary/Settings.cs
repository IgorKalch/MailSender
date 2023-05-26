using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitServerLibrary
{
    public class Settings
    {
        public const string SettingsKey = "appSettings";

        public string ConnectionString { get; set; }
        public string ProducerName { get; set; }
        public string ConsumerName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }
        public SmtpMethodSettings SmtpMethodSettings { get; set; } 
    }    
}
