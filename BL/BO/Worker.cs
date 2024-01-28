

namespace BO;
/// <summary>
/// Worker Entity represents a worker with all its props    
/// </summary>
/// <param name="Id">Personal unique Id of the worker (as in national id card)</param>
/// <param name="Name">Private name of the worker</param>
/// <param name="Email">Private email of the worker</param>
/// <param name="RankWorker">The level of the worker</param>
/// <param name="HourPrice">The worker's price for one hour</param>
/// <param name="IdTask">The id of task that the worker does</param>
/// <param name="AliasTask">the name of the task that the worker does</param>
public class Worker
{
   public int Id {  get; set; }
   public Rank RankWorker {  get; set; }
   public double HourPrice { get; set; }
   public string? Name {  get; set; }
   public string? Email {  get; set; }
   public WorkerOnTask? CurrentTask { get; set; }
}
