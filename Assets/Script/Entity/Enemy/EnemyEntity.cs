using Assets.Script;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Interface;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Entity.Enemy
{
    public abstract class EnemyEntity : MonoBehaviour, ILiveObject
    {
        protected Player player => Player.Instance;
        protected Rigidbody2D body => GetComponent<Rigidbody2D>();

        public EState State { get; protected set; } = EState.Free;

        [SerializeField]
        protected GameObject detectObject;

        protected float timecount = 0;

        private float currentspeed;
        private float cdattack = 0f;

        [SerializeField]
        protected float Health = 3000f;

        [SerializeField]
        protected float _direction = 1f;

        [SerializeField]
        protected float Speed = 5f;

        [SerializeField]
        protected float ChaseSpeed = 1.5f;

        [SerializeField]
        protected float AttackSpeed = 1f;

        [SerializeField]
        protected float DetectDistance = 10f;

        [SerializeField]
        protected float TimeChase = 10f;

        [SerializeField]
        protected float TimeTurn = 3f;

        [SerializeField]
        protected float Damage = 200f;
        public bool IsDetectedPlayer { get; set; } = false;

        // Start is called before the first frame update
        private void Start()
        {
            if (gameObject == null) return;
            Setup();
        }

        private void Setup()
        {
            _direction = transform.localScale.x > 0 ? _direction : -_direction;

            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
           if (player.State == EState.Dead) return;

           CheckAction();
           DetectPlayer();
           
           timecount += Time.deltaTime;
           
           if (IsDetectedPlayer) ChasePlayer();
           else if (timecount >= TimeTurn) Turn();
        }

        protected virtual void FixedUpdate()
        {
            if (player.State == EState.Dead) IsDetectedPlayer = false;
            Moving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tag = collision.collider.tag;
            
            switch (tag)
            {
                case "Player":
                    HitPlayer();
                    break;

                case "Projectile":
                    ProjectileHit(collision);
                    break;

                default:
                    return;
            }

            CheckAlive();
        }

        private void CheckAction()
        {
            switch (State)
            {
                case EState.IsAttack:
                    WaitAction(State, AttackSpeed);
                    break;

                case EState.IsKnockBack:
                    WaitAction(State, GameSystem.KnockBackTime);
                    break;

                default:
                    return;
            }
        }

        // GET SET //
        public void SetHealth(float health)
        {
            Health = health;
        }

        public float GetHealth()
        {
            return Health;
        }

        public float GetDamage()
        {
            return Damage;
        }

        // MOVEMENT //

        private void Turn()
        {
            _direction = 1f;

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _direction = transform.localScale.x > 0 ? _direction : -_direction;
            timecount = 0;
        }

        private void Moving()
        {
            if (player.State == EState.IsKnockBack) return;
            if (State != EState.Free) return;

            DetectWall();

            if (IsDetectedPlayer) currentspeed = Speed * ChaseSpeed;
            else currentspeed = Speed;

            Vector2 pos = new Vector2(currentspeed * Time.deltaTime * _direction, 0f);
            transform.Translate(pos);
        }

        private void DetectWall()
        {
            if (IsDetectedPlayer) return;

            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            RaycastHit2D hit = Physics2D.Raycast(detectObject.transform.position, direction, 0.5f);

            if (hit.collider == null) return;

            var collider = hit.collider;

            if (!collider.CompareTag("Wall") && !collider.CompareTag("Ground") && !collider.CompareTag("Enemy"))
            {
                return;
            }

            Turn();
        }

        // BEHAVIOR //

        protected abstract void DetectPlayer();

        protected abstract void ChasePlayer();

        protected void Unchase(RaycastHit2D hit)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                timecount = 0;
                return;
            }

            if (timecount < TimeChase) return;

            timecount = 0;
            IsDetectedPlayer = false;
        }

        private void HitPlayer()
        {
            if (player.State == EState.IsKnockBack) return;

            State = EState.IsAttack;
            player.KnockBack(gameObject);

            float health = player.GetHealth() - Damage;
            player.SetHealth(health);
        }

        // GET HIT //

        private void ProjectileHit(Collision2D collision)
        {
            var obj = collision.gameObject;
            var projectile = obj.GetComponent<IProjectile>();

            Health -= projectile.GetDamage();
        }

        private void CheckAlive()
        {
            if (Health > 0) return;

            Destroy(gameObject);
        }

        public bool CanKnockBack() => true;

        public void KnockBack(GameObject attacker)
        {
            if (!CanKnockBack()) return;

            State = EState.IsKnockBack;
            float atkpos = attacker.transform.position.x;
            float direction;

            if (atkpos > transform.position.x) direction = 1f;
            else direction = -1f;

            float x = GameSystem.AttackKnockBackForce.x;
            float y = GameSystem.AttackKnockBackForce.y;

            Vector2 pos = new Vector2(x * direction * -1f, y);

            body.AddForce(pos, ForceMode2D.Impulse);
            TurnByKnockBack(direction);
        }

        private void TurnByKnockBack(float direction)
        {
            float x = transform.localScale.x;

            if (x > 0 && direction > 0 || x < 0 && direction < 0) x *= -1;
            else return;

            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        // WAIT //
        protected void WaitAction(EState state, float time)
        {
            if (State != state)
            {
                cdattack = 0;
            }

            if (cdattack >= time)
            {
                cdattack = 0;
                State = EState.Free;
            }

            cdattack += Time.deltaTime;
        }
    }
}
