namespace ConsoleApp1;

public abstract class User
{
    public string username { get; set; }
    public string password { get; set; }
    public abstract string role { get; }

    protected User(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}



