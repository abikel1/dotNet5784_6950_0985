namespace DO;
/// <summary>
/// Worker Entity represents a worker with all its props    
/// </summary>
/// <param name="Id">Personal unique Id of the worker (as in national id card)</param>
/// <param name="Name">Private name of the worker</param>
/// <param name="Email">Private email of the worker</param>
/// <param name="RankWorker">The level of the worker</param>
/// <param name="HourPrice">The worker's price for one hour</param>
public record Worker
(

    int Id,

    Rank RankWorker,

    float HourPrice,

    string? Name = null,

    string? Email = null

)
{
    public Worker() : this(0, 0, 0) { }//empty ctor
}
