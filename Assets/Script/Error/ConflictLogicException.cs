using System;

namespace Assets.Script.Error
{
    public class ConflictLogicException : Exception
    {
        private static readonly string message = "Something wrong\n Detail: ";

        public ConflictLogicException() : base(message) { }

        public ConflictLogicException(string Detail) : base(message + Detail) { }
    }
}
