namespace DO;

public record User
(
     int Id,
     int Password,
     bool IsMennager=false
)
{
    public User() : this(0, 0, false) { }//empty ctor
}
