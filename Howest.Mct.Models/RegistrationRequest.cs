namespace Howest.Mct.Models;

public class RegistrationRequest
{
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int Zipcode { get; set; }
    public int Age { get; set; }
    public bool IsFirstTimer { get; set; }
}