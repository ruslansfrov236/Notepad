namespace notepad.business.Validator;

public class FileTooLargeException:Exception
{
    public FileTooLargeException(string message ):base(message){}
    public FileTooLargeException(string message, Exception?  exception ):base(message, exception){}
   
}