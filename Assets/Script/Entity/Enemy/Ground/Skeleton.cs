using Assets.Script.Enum;
using UnityEngine;

namespace Assets.Script.Entity.Enemy.Ground
{
    public class Skeleton : GroundEnemy
    {
        protected override void Update()
        {
            base.Update();
        }

        protected override void Moving()
        {
            base.Moving();

            //Debug.Log(isMoving);
            animator.SetBool("isMoving", IsMoving);
        }

        protected override void HandleAttack()
        {
            if (State != EState.Free) return;

            State = EState.IsAttack;
            StartAnimateAttack();
        }

        private void StartAnimateAttack()
        {
            animator.speed = AttackSpeed;
            animator.SetBool("isAttack", true);
        }

        private void Attack()
        {
            var hit = InAttackRange();

            if (hit.collider == null) return;

            HitEnemy();
        }

        private void EndAttack()
        {
            animator.speed = 1;
            animator.SetBool("isAttack", false);
            State = EState.Free;
        }

        public override void GetHitAction(GameObject attacker)
        {
            StopMovingByGetHit(attacker);
            animator.SetTrigger("getHit");
        }

        protected override void Dying()
        {
            State = EState.Dead;
            SetCollider(false, _collider);
            animator.SetTrigger("dead");
        }
    }
}
