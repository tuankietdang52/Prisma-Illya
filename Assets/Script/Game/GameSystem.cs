using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Game
{
    public sealed class GameSystem
    {
        // NORMAL ATTACK KNOCKBACK FORCE //
        public static float KnockBackTime = 1f;
        public struct AttackKnockBackForce
        {
            public static float x = 5f;
            public static float y = 3f;
        }
    }
}
