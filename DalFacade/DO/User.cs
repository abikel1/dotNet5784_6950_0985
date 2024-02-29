namespace DO;

public record User
(
     string userName,
     int Id,
     int Password,
     bool isMennager
)
{
    public User() : this("",0, 0,false) { }//empty ctor
}
