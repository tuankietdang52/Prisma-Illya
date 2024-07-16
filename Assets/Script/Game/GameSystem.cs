using Assets.Script.Enum;
using System.Collections.Generic;

namespace Assets.Script.Game
{
    public sealed class GameSystem
    {
        // NORMAL ATTACK KNOCKBACK FORCE //
        public static float KnockBackTime = 0.8f;
        public struct AttackKnockBackForce
        {
            public static float x = 3f;
            public static float y = 2f;
        }
    }
}
