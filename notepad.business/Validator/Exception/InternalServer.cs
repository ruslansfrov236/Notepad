namespace notepad.business.Validator;

public class InternalServer:Exception
{
    public InternalServer(string message):base(message){}
    public InternalServer(string message, Exception? exception):base(message, exception){}
   
}