using Assets.Script.Entity.Enemy;
using Assets.Script.Enum;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Entity.Enemy.Ground
{
    public class GroundEnemy : EnemyEntity
    {
        protected override void Update()
        {
            base.Update();
        }

        protected override void DetectPlayer()
        {
            player.Effect.TryGetValue(EEffect.Invulnerable, out var effect);
            float time = effect.Item2;
            if (time > 0) return;

            var hit = GetDetectPlayerRaycast();

            if (hit.collider == null) return;
            
            IsDetectedPlayer = true;
        }
    }
}
