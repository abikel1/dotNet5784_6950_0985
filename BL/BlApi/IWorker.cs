using BO;

namespace BlApi;
public interface IWorker
{
    /// <summary>
    /// return list of workers that establish the particular requirment
    /// </summary>
    /// <param name="filter">function according to what will be filtered</param>
    /// <returns>list of the workers</returns>
    public IEnumerable<BO.Worker> ReadWorkers(Func<BO.Worker, bool>? filter = null);
    /// <summary>
    /// return the details of the worker with the id that the function got
    /// </summary>
    /// <param name="id">the id of the worker that I want his details</param>
    /// <returns>worker object</returns>
    public BO.Worker Read(int id);
    /// <summary>
    /// add new worker with the details that the function got
    /// </summary>
    /// <param name="worker">the worker that I want to add</param>
    public void AddWorker(BO.Worker worker);
    /// <summary>
    /// remove worker with the id that the function got
    /// </summary>
    /// <param name="id">the id of the worker that I want to remove</param>
    public void RemoveWorker(int id);
    /// <summary>
    /// update the worker with the details that the function got
    /// </summary>
    /// <param name="worker">the worker that I want to update</param>
    public void UpdateWorker(BO.Worker worker);
    public bool CheckUser(BO.User user);
    public void AddUser(BO.User user);
    public void RemoveUser(BO.User user);
    public void clear();
    //public IEnumerable<BO.Worker> RankGroups(int rank);

}
