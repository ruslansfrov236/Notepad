namespace notepad.business.Validator;

public class PasswordChangeFailedException:Exception
{
    public PasswordChangeFailedException():base("An error occurred while resetting the password."){}
    public PasswordChangeFailedException(string message):base(message){}

    public PasswordChangeFailedException(string message , Exception? exception):base(message, exception){}

   
    
}