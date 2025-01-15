namespace WarehouseManagementSystem.Common;

public sealed class BusinessLogicException : Exception
{
    public BusinessLogicException(string message) : base(message)
    {
    }
}
