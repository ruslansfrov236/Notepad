namespace notepad.business.Dto_s.Auth;

public class LoginDto
{
    public string password { get; set; }
    public string usernameOrEmail { get; set;  }
    public int accessTokenLifeTime { get; set; }
}