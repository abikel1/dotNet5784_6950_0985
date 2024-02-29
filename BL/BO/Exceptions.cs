namespace BO;

// Custom exception class for cases where an entity does not exist
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
}
// Custom exception class for cases where an entity already exists
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}
// Custom exception class for cases where deletion of an entity is not possible
[Serializable]
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
}
//[Serializable]
//public class BlXMLFileLoadCreateException : Exception
//{
//    public BlXMLFileLoadCreateException(string? message) : base(message) { }
//}
// Custom exception class for cases where input validation fails
[Serializable]
public class BlInValidInputException : Exception
{
    public BlInValidInputException(string? message) : base(message) { }
}
// Custom exception class for cases where an object cannot be removed
[Serializable]
public class BlCantRemoveObject : Exception
{
    public BlCantRemoveObject(string? message) : base(message) { }
}
// Custom exception class for cases where a task cannot be updated
[Serializable]
public class BLcantUpdateTask:Exception
{
    public BLcantUpdateTask(string? message) : base(message) { }
}
// Custom exception class for cases where a worker cannot be assigned to a task
[Serializable]
public class BlCantAssignWorker:Exception
{
    public BlCantAssignWorker(string? message) : base(message) { }
}
// Custom exception class for cases where the start date of a task cannot be updated
[Serializable]
public class BlcanotUpdateStartdate:Exception
{
    public BlcanotUpdateStartdate(string? message) : base(message) { }
}
// Custom exception class for cases where an operation is attempted during the planning project stage
[Serializable]
public class BlplanningStatus : Exception
{
    public BlplanningStatus(string? message) : base(message) { }
}
[Serializable]
public class BlexecutionStatus : Exception
{
    public BlexecutionStatus(string? message) : base(message) { }
}
[Serializable]
public class BlCantAddMenagger : Exception
{
    public BlCantAddMenagger(string? message) : base(message) { }
}
