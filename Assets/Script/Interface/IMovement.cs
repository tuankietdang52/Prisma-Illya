using Assets.Script.Utility.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Assets.Script.Interface
{
    public interface IMovement
    {
        void Moving(float x = 0, float y = 0);

        /// <summary>
        /// For air movement like jump or fly
        /// </summary>
        /// <param name="y">jump force or y</param>
        void AirMoving(float y = 0);
    }
}
