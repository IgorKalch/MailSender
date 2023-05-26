using RabbitServerLibrary.SendingMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitServerLibrary.SendingModule
{
    public class MethodExecution
    {
        IMethod _method = null;

        public void Execute(Mail mail, Settings settings)
        {
            FabricSending fabric = new FabricSending();
            _method = fabric.SelectSendingMethod(mail.Method);
            _method.Send(mail, settings);
        }
    }
}
