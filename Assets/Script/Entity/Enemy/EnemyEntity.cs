using Assets.Script;
using Assets.Script.Enum;
using Assets.Script.Interface;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Entity.Enemy
{
    public abstract class EnemyEntity : MonoBehaviour
    {
        protected Transform player => Player.Instance.transform;
        protected Rigidbody2D body => GetComponent<Rigidbody2D>();
        public EState State { get; protected set; } = EState.Free;

        [SerializeField]
        protected GameObject detectObject;

        protected float timecount = 0;

        private float currentspeed;
        private float cdattack = 0f;

        [SerializeField]
        protected float health = 3000f;

        [SerializeField]
        protected float _direction = 1f;

        [SerializeField]
        protected float speed = 5f;

        [SerializeField]
        protected float chasespeed = 1.5f;

        [SerializeField]
        protected float AttackSpeed = 1f;

        [SerializeField]
        protected float detectdistance = 10f;

        [SerializeField]
        protected float timechase = 10f;

        [SerializeField]
        protected float timeturn = 5f;

        [SerializeField]
        protected float Damage = 200f;

        public bool IsDetected { get; set; } = false;

        // Start is called before the first frame update
        private void Start()
        {
            _direction = transform.localScale.x > 0 ? _direction : -_direction;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            CheckAction();
            Detect();

            timecount += Time.deltaTime;

            if (IsDetected) ChasePlayer();
            else SetDirection();

            Moving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tag = collision.collider.tag;
            
            switch (tag)
            {
                case "Player":
                    State = EState.IsAttack;
                    break;

                case "Projectile":
                    ProjectileHit(collision);
                    break;
            }

            CheckAlive();
        }

        private void CheckAction()
        {
            switch (State)
            {
                case EState.IsAttack:
                    CheckAttack();
                    break;
            }
        }

        // GET SET //

        public float GetDamage()
        {
            return Damage;
        }

        // MOVEMENT //

        private void SetDirection()
        {
            if (timecount < timeturn) return;

            _direction = 1f;

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _direction = transform.localScale.x > 0 ? _direction : -_direction;
            timecount = 0;
        }

        private void Moving()
        {
            if (State != EState.Free) return;

            if (IsDetected) currentspeed = speed * chasespeed;
            else currentspeed = speed;

            Vector2 pos = new Vector2(currentspeed * Time.deltaTime * _direction, 0f);
            transform.Translate(pos);
        }

        // BEHAVIOR //

        protected abstract void Detect();

        private void ChasePlayer()
        {
            float x = transform.localScale.x < 0 ? transform.localScale.x * -1 : transform.localScale.x;
            _direction = 1f;

            var current = transform.position.x;
            var playerpos = player.position.x;

            x = current < playerpos ? x : -x;
            _direction = x > 0 ? _direction : -_direction;

            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        protected void Unchase(RaycastHit2D hit)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                timecount = 0;
                return;
            }

            if (timecount < timechase) return;

            timecount = 0;
            IsDetected = false;
        }

        // GET HIT //

        private void ProjectileHit(Collision2D collision)
        {
            var obj = collision.gameObject;
            var projectile = obj.GetComponent<IProjectile>();

            health -= projectile.GetDamage();
        }

        private void CheckAlive()
        {
            if (health > 0) return;

            Destroy(gameObject);
        }

        // WAIT //
        protected void CheckAttack()
        {
            if (State != EState.IsAttack)
            {
                cdattack = 0;
            }

            if (cdattack >= AttackSpeed)
            {
                cdattack = 0;
                State = EState.Free;
            }

            cdattack += Time.deltaTime;
        }
    }
}
