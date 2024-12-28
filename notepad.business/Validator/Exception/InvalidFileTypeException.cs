namespace notepad.business.Validator;

public class InvalidFileTypeException:Exception
{
    public InvalidFileTypeException(string message):base(message){}
    public InvalidFileTypeException(string message, Exception? exception):base(message,exception ){}
  
}