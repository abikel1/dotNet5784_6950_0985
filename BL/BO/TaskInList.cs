
namespace BO;
/// <summary>
/// task in list
/// </summary>
/// <param name="Id">the id of the task</param>
/// <param name="Description">the description of the task</param>
/// <param name="Alias">the alias of the task</param>
/// <param name="StatusTask">the status of the task</param>
public class TaskInList
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public Status StatusTask { get; set; }

}
