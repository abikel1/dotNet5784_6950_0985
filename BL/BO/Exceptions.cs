namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
}
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}
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

[Serializable]
public class BlInValidInputException : Exception
{
    public BlInValidInputException(string? message) : base(message) { }
}

[Serializable]
public class BlCantRemoveObject : Exception
{
    public BlCantRemoveObject(string? message) : base(message) { }
}
[Serializable]
public class BLcantUpdateTask:Exception
{
    public BLcantUpdateTask(string? message) : base(message) { }
}
[Serializable]
public class BlCantAssignWorker:Exception
{
    public BlCantAssignWorker(string? message) : base(message) { }
}
[Serializable]
public class BlcanotUpdateStartdate:Exception
{
    public BlcanotUpdateStartdate(string? message) : base(message) { }
}
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
