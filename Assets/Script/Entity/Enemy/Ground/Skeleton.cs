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
            animator.SetBool("isMoving", isMoving);
        }

        protected override void ChasePlayer()
        {
            base.ChasePlayer();
        }

        protected override void HandleAttack()
        {
            if (State != EState.Free) return;

            Debug.Log("Attacking");

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

            player.KnockBack(gameObject);
            player.DecreaseHealth(Damage);
        }

        private void EndAttack()
        {
            animator.SetBool("isAttack", false);
            State = EState.Free;
        }

        protected override void GetHitAction()
        {
            animator.SetTrigger("getHit");
        }

        protected override void Dying()
        {
            State = EState.Dead;
            SetCollider(false);
            animator.SetTrigger("dead");
        }

        private void RemoveCorspe()
        {
            base.Dying();
        }
    }
}
