namespace DO;

public record User
(
     string? UserName,
     int Password,
     bool IsMennager=false
)
{
    public User() : this(" ", 0, false) { }//empty ctor
}
