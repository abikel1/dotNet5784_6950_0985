
namespace BO;
/// <summary>
/// task worker
/// </summary>
/// <param name="Id">id of task</param>
/// <param name="Difficulty">the difficulty of task</param>
/// <param name="StatusTask">the status of the task</param>
/// <param name="DependencyTasks">the tasks that dependency on the current task</param>
/// <param name="WorkerId">the id of the worker performing the task </param>
/// <param name="WorkerName">the name of the worker performing the task </param>
/// <param name="TaskDescription">the description of task</param>
/// <param name="Alias">the alaias of the task</param>
/// <param name="CreateTask">the task creation date</param>
/// <param name="BeginWork">the planned date for work to begin</param>
/// <param name="BeginTask">the  date of working start</param>
/// <param name="TimeTask">the duration of time to perform a task</param>
/// <param name="DeadLine">possible final end date</param>
/// <param name="Remarks">the remarks about the task</param>
/// <param name="Product">the product of the task</param>
public class Task
{
    public int Id {  get; init; }
    public Rank Difficulty {  get; set; }
    public Status StatusTask { get; set; }
    public IEnumerable<TaskInList>? DependencyTasks { get; set; }
    public WorkerOnTask? Worker { get; set; }
    public string? TaskDescription { get; set; }
    public string? Alias { get; set; }
    public DateTime? CreateTask { get; set; }
    public DateTime? BeginWork { get; set; }
    public DateTime? BeginTask { get; set; }
    public int? TimeTask {  get; set; }
    public DateTime? DeadLine {  get; set; }
    public DateTime? EndWorkTime { get; set; }
    public string? Remarks {  get; set; }
    public string? Product { get; set; }
    public override string ToString() => this.ToStringProperty();
}
