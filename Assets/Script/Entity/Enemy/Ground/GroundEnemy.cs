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

        protected override void ChasePlayer()
        {
            var playerpos = player.transform.position;
            float x = transform.localScale.x;

            var current = transform.position.x;

            if (current < playerpos.x)
            {
                _direction = 1f;
                x = x < 0 ? x *= -1 : x;
            }
            else
            {
                _direction = -1f;
                x = x > 0 ? x *= -1 : x;
            }

            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        protected override void DetectPlayer()
        {
            if (player.Effect == EEffect.Invulnerable) return;

            var mask = LayerMask.NameToLayer("Player");

            // convert layer to layermask
            var layer = 1 << mask;

            var pos = detectObject.transform.position;

            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            
            //hit player layer only
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, DetectDistance, layer);

            Debug.DrawRay(detectObject.transform.position, hit.distance * direction, Color.red);

            if (hit.collider == null && !IsDetectedPlayer) return;
            
            IsDetectedPlayer = true;

            if (IsDetectedPlayer) CheckingUnchase(hit);
        }
    }
}
