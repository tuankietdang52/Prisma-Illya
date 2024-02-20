using Assets.Script.Enum;
using Assets.Script.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EffectOwner = System.Tuple<Assets.Script.Interface.ILiveObject, float>;

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

        // EFFECT
        public static Dictionary<EEffect, EffectOwner> InitEffect()
        {
            var effect = new Dictionary<EEffect, EffectOwner>();

            var list = System.Enum.GetValues(typeof(EEffect)) as EEffect[];

            foreach (var item in list)
            {
                EffectOwner detail = new EffectOwner(null, 0);
                effect.Add(item, detail);
            }

            return effect;
        }
    }
}
