namespace DalApi;
using DO;
public interface IWorker:ICrud<Worker>
{
    public bool Check(DO.User user);
    public void AddUser(DO.User user);
    public void RemoveUser(DO.User user);
}
