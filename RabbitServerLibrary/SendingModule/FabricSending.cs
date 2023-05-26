using RabbitServerLibrary.SendingMethods;

namespace RabbitServerLibrary.SendingModule
{
    public class FabricSending
    {
        public virtual IMethod SelectSendingMethod (EMethods type)
        {
            IMethod method = null;
            switch (type)
            {
                case EMethods.SmtpMethod:
                    method = new SmtpMethod();
                    break;
                default:
                    throw new Exception("No shipping send method.");
            }
            return method;
        }
    }
}
