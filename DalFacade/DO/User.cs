namespace DO;

public record User
(
     string userName,
     int Id,
     int Password
)
{
    public User() : this("",0, 0) { }//empty ctor
}
