using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.PlayerContainer
{
    public abstract class PlayerForm
    {
        protected Player player => Player.Instance;

        public PlayerForm() { }


        public abstract void SetupForm();
        public abstract void ExcuteAttack();

        public abstract void ExcuteFirstSkill();

        public abstract void ExcuteSecondSkill();

        public abstract void ExcuteThirdSkill();
    }
}
