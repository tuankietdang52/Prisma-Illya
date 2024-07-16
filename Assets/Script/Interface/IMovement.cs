using Assets.Script.Game.InGameObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Interface
{
    public interface IMovement
    {
        void Moving(float x = 0, float y = 0);
    }
}
