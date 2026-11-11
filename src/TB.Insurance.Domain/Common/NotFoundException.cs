namespace TB.Insurance.Domain.Common;

public sealed class NotFoundException(string message) : Exception(message);
