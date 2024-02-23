using Assets.Script.Entity.Enemy;
using Assets.Script.Enum;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Entity.Enemy.Ground
{
    public abstract class GroundEnemy : EnemyEntity
    {
        [SerializeField]
        private float AttackRange = 1f;

        [SerializeField]
        private float RangeDistance = 0.2f;

        protected override void Update()
        {
            base.Update();
        }

        protected override void Moving()
        {
            base.Moving();
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

        protected override void ChasePlayer()
        {
            base.ChasePlayer();

            if (!InAttackRange()) return;

            HandleAttack();
        }

        protected RaycastHit2D InAttackRange()
        {
            Vector3 pos = _collider.bounds.center;
            var direction = transform.right * transform.localScale.x;

            var size = new Vector3(_collider.bounds.size.x * AttackRange * RangeDistance, _collider.bounds.size.y, _collider.bounds.size.z);

            var mask = LayerMask.NameToLayer("Player");
            var layer = 1 << mask;

            RaycastHit2D hit = Physics2D.BoxCast(pos + AttackRange * direction, size, 0, Vector2.right, 0, layer);

            return hit;
        }

        protected abstract void HandleAttack();

        private void OnDrawGizmos()
        {
            var _collider = GetComponent<CapsuleCollider2D>();

            Vector3 pos = _collider.bounds.center;
            var direction = transform.right * transform.localScale.x;

            var size = new Vector3(_collider.bounds.size.x * AttackRange * RangeDistance, _collider.bounds.size.y, _collider.bounds.size.z);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pos + AttackRange * direction, size);
        }
    }
}
