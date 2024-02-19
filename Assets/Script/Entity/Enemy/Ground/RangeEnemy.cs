using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.AI;

namespace Assets.Script.Entity.Enemy.Ground
{
    public class RangeEnemy : EnemyEntity
    {
        private void Start()
        {
            DetectDistance = 15f;
            Damage = 400f;
            Health = 1500f;
            Speed = 2f;
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void DetectPlayer()
        {
            var mask = LayerMask.NameToLayer("Player");
            var pos = detectObject.transform.position;
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            // hit player layer only
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, DetectDistance, mask);

            if (hit.collider == null && !IsDetectedPlayer) return;

            IsDetectedPlayer = true;

            if (IsDetectedPlayer) CheckingUnchase(hit);
        }

        protected override void ChasePlayer()
        {
            throw new NotImplementedException();
        }

    }
}
