using System;

namespace Assets.Script.Error
{
    public class GameLogicException : Exception
    {
        private static readonly string message = "Something wrong\n Detail: ";

        public GameLogicException() : base(message) { }

        public GameLogicException(string Detail) : base(message + Detail) { }
    }
}
