using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Interface
{
    public interface ILiveObject
    {
        float GetHealth();
        void SetHealth(float health);
        void DecreaseHealth(float damage);
        bool CanKnockBack();
        void KnockBack(GameObject attacker);
    }
}
