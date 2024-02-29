using Assets.Script.Enum;
using Assets.Script.Game.InGameObj;
using Assets.Script.PlayerContainer.Character.IllyaContainer.Projectile;
using UnityEngine;

namespace Assets.Script.Entity.Enemy.Ground
{
    public class RangeEnemy : EnemyEntity
    {
        [SerializeField]
        private GameObject Holder;

        private void Start()
        {
            DetectDistance = 15f;
            Damage = 400f;
            Health = 1500f;
            Speed = 4f;
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void DetectEnemy()
        {
            player.Effect.TryGetValue(EEffect.Invulnerable, out var effect);
            float time = effect.Item2;
            if (time > 0) return;

            var hit = GetDetectPlayerRaycast();

            if (hit.collider == null) return;

            detectenemy = hit.collider.gameObject;
            isDetectedEnemy = true;
        }

        protected override void ChaseEnemy()
        {
            base.ChaseEnemy();

            var hit = GetDetectPlayerRaycast();

            if (hit.collider == null) return;

            HanldeAttack();
        }

        private void HanldeAttack()
        {
            if (State != EState.Free) return;

            State = EState.IsAttack;

            var script = Holder.GetComponent<ProjectileHolder>();

            var ball = script.GetProjectile();

            var ballobj = ball.GetComponent<MagicBall>();

            Attack(ballobj);
        }

        private void Attack(MagicBall ballobj)
        {
            ballobj.Damage = Damage;
            ballobj.Owner = this;

            var position = Holder.transform;

            if (transform.localScale.x < 0) ballobj.SetDirection(-1f);
            else ballobj.SetDirection(1f);

            ballobj.SetPosition(position);
            ballobj.Active();
        }

        protected override void Dying()
        {
            Destroy(Holder);
            RemoveCorspe();
        }
    }
}
