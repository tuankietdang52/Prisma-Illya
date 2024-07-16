using Assets.Script.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.PlayerContainer.Illya
{
    public class Illya : Player
    {
        protected override void Setup()
        {
            SetMaxHealth(2027);
            SetHealth(2027);
            SetDamage(1677);
            SetMovementSpeed(10f);

            movement = new WalkMovement(this);
        }
    }
}
