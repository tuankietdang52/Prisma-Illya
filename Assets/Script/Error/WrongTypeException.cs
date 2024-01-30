using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongTypeException : Exception
{
    private static readonly string message = "Invalid type passing";
    
    public WrongTypeException() : base(message)
    {

    }
}
