namespace DO;
/// <summary>
/// task worker
/// </summary>
/// <param name="Id">id of task</param>
/// <param name="WorkerId">the id of the worker performing the task </param>
/// <param name="Difficulty">the difficulty of task</param>
/// <param name="TaskDescription">the description of task</param>
/// <param name="MileStone">the milestone of the task</param>
/// <param name="Alias">the alaias of the task</param>
/// <param name="CreateTask">the task creation date</param>
/// <param name="BeginWork">the planned date for work to begin</param>
/// <param name="BeginTask">the  date of working start</param>
/// <param name="TimeTask">the duration of time to perform a task</param>
/// <param name="DeadLine">possible final end date</param>
/// <param name="EndWorkTime">actual end date</param>
/// <param name="Remarks">the remarks about the task</param>
/// <param name="Product">the product of the task</param>

public record Task
(
    int Id,
    Rank Difficulty,
    int? WorkerId=0,
    string? TaskDescription = null,
    bool MileStone = false,
    string? Alias = null,
    DateTime? CreateTask = null,
    DateTime? BeginWork = null,
    DateTime? BeginTask = null,
    TimeSpan? TimeTask = null,
    DateTime? DeadLine = null,
    DateTime? EndWorkTime = null,
    string? Remarks = null,
    string? Product = null
)

{
    public Task() : this(0, 0, 0) { }//empty ctor
}
