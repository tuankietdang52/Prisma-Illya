using Assets.Script.Entity.Enemy;
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

        protected override void Detect()
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            RaycastHit2D hit = Physics2D.Raycast(detectObject.transform.position, direction, detectdistance);

            Debug.DrawRay(detectObject.transform.position, hit.distance * direction, Color.red);

            if (hit.collider == null && !IsDetected) return;

            if (IsDetected)
            {
                Unchase(hit);
                return;
            }

            IsDetected = Check(hit);
        }

        private bool Check(RaycastHit2D hit)
        {
            if (hit.collider == null) return false;

            string tag = hit.collider.tag;

            if (tag == "Player") return true;

            else if (tag == "Enemy")
            {
                var enemy = hit.collider.gameObject.GetComponent<EnemyEntity>();
                if (enemy == null || !enemy.IsDetected) return false;

                return true;
            }

            return false;
        }
    }
}
