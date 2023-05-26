using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitServerLibrary
{
    public class SmtpMethodSettings
    {
        public string Password { get; set; }
        public string Login { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
    }
}
