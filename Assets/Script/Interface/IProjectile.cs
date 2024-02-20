using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Interface
{
    public interface IProjectile
    {
        public float GetDamage();

        public void SetPosition(Transform position);

        public void SetDirection(float direction);
    }
}
