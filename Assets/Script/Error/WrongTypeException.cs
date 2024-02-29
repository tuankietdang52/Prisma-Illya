using System;

public class WrongTypeException : Exception
{
    private static readonly string message = "Invalid type passing";
    
    public WrongTypeException() : base(message)
    {

    }
}
