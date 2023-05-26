namespace RabbitServerLibrary.SendingMethods
{
    public interface IMethod
    {
        Task Send (Mail mail, Settings settings);
    }
}