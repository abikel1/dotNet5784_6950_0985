
using BlApi;
using BO;
namespace BlImplementation;

internal class TaskInLIstImplementation : ITaskInList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Add(int idTask, int Previous)
    {
        _dal.Dependency.Create(new DO.Dependency(0, idTask, Previous));
    }

    public void Remove(int idDepend,int idPrevius)
    {
        try
        {
            DO.Dependency dependency = _dal.Dependency.Read(x => x.IdDependentTask == idDepend && x.IdPreviousTask == idPrevius)!;
            _dal.Dependency.Delete(dependency.Id);
        }
        catch(DO.DalDoesNotExistException ex)
        { 
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }
}
