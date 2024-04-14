namespace BlApi;

public interface IUser
{
    public void Create(BO.User item); //Creates new  user in DAL
    BO.User? Read(int id); //Reads user by its ID 
    BO.User? Read(string name); //Reads user by its ID 
    IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null); 
    public void Update(BO.User item); //Updates user
    void Delete(int id); //Deletes an user by its Id
    //BO.User? Read(Func<BO.User, bool> filter); 
    void clear();
    void checkInvalid(BO.User user);
}
