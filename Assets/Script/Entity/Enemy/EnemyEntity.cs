using Assets.Script;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Interface;
using Assets.Script.PlayerContainer;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using EffectOwner = System.Tuple<Assets.Script.Entity.LiveObject, float>;

namespace Assets.Script.Entity.Enemy
{
    /// <summary>
    /// Abstract class for Enemy
    /// <para>This class inherit Live Object class</para>
    /// </summary>
    public abstract class EnemyEntity : LiveObject
    {
        protected Player player => Player.Instance;
        protected CapsuleCollider2D _collider => GetComponent<CapsuleCollider2D>();

        // Stats

        [SerializeField]
        protected GameObject detectObject;

        protected float timecount = 0;

        private float currentspeed;

        [SerializeField]
        protected float _direction = 1f;

        [SerializeField]
        protected float ChaseSpeed = 1.5f;

        [SerializeField]
        protected float DetectDistance = 10f;

        [SerializeField]
        protected float TimeChase = 10f;

        [SerializeField]
        protected float TimeTurn = 3f;

        // Bool Element
        public bool IsDetectedPlayer { get; set; } = false;

        protected bool isMoving = true;

        [SerializeField]
        private bool isInterrupt = false;
        public int IsInterrupt
        {
            get
            {
                if (!isInterrupt) return 0;
                else return 1;
            }
            set
            {
                if (value == 0) isInterrupt = false;
                else isInterrupt = true;
            }
        }

        private void Awake()
        {
            if (gameObject == null)
            {
                Debug.Log($"{gameObject.name} is null");
                return;
            }

            Setup();
        }

        private void Setup()
        {
            _direction = transform.localScale.x > 0 ? _direction : -_direction;

            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            Effect = GameSystem.InitEffect();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (State == EState.Dead) return;
            if (player.State == EState.Dead) return;

            MovingAI();
        }

        protected virtual void FixedUpdate()
        {
            if (State == EState.Dead)
            {
                return;
            }

            UpdateObject();
        }

        protected void UpdateObject()
        {
            if (player.State == EState.Dead) IsDetectedPlayer = false;
            Moving();
            CheckMoving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tag = collision.collider.tag;
            
            switch (tag)
            {
                case "Player":
                    HitPlayer(false);
                    break;

                default:
                    return;
            }

            CheckAlive();
        }

        // GET SET //
        public float GetCurrentSpeed()
        {
            return currentspeed;
        }

        // MOVEMENT //

        private void MovingAI()
        {
            if (!IsDetectedPlayer) DetectPlayer();
            else CheckingUnchase();

            timecount += Time.deltaTime;

            if (IsDetectedPlayer) ChasePlayer();
            else if (timecount >= TimeTurn) Turn();
        }

        private void CheckMoving()
        {
            var pos = detectObject.transform.position;

            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            RaycastHit2D hit = Physics2D.Raycast(pos, direction, 0.1f);

            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                isMoving = false;
                return;
            }

            isMoving = true;
        }

        private void Turn()
        {
            _direction = 1f;

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _direction = transform.localScale.x > 0 ? _direction : -_direction;
            timecount = 0;
        }

        protected virtual void Moving()
        {
            if (State != EState.Free)
            {
                isMoving = false;
                return;
            }

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

        protected virtual RaycastHit2D GetDetectPlayerRaycast()
        {
            var mask = LayerMask.NameToLayer("Player");

            // convert layer to layermask
            var layer = 1 << mask;

            var pos = detectObject.transform.position;

            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            //hit player layer only
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, DetectDistance, layer);

            Debug.DrawRay(detectObject.transform.position, hit.distance * direction, Color.red);

            return hit;
        }

        protected abstract void DetectPlayer();

        protected virtual void ChasePlayer()
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

        protected void CheckingUnchase()
        {
            var hit = GetDetectPlayerRaycast();

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                timecount = 0;
                return;
            }

            if (timecount < TimeChase) return;

            timecount = 0;
            IsDetectedPlayer = false;
        }

        protected void HitPlayer(bool canIntterupt = true)
        {
            Debug.Log("Can Interrupt" + canIntterupt);
            if (isInterrupt && canIntterupt) return;

            player.GetHitAction(gameObject);
            player.DecreaseHealth(Damage);
        }

        // GET HIT //

        public override void DecreaseHealth(float damage)
        {
            if (IsInvulnerable()) return;

            Health -= damage;
            if (Health < 0) Health = 0;

            CheckAlive();
        }

        public void StopMovingByGetHit(GameObject attacker)
        {
            float direction = attacker.transform.localScale.x > 0 ? 1f : -1f;
            State = EState.GetHit;
            TurnByKnockBack(direction);
        }

        private void CheckAlive()
        {
            if (Health > 0) return;

            Dying();
        }

        public override void GetHitAction(GameObject attacker)
        {
            if (State == EState.GetHit || State == EState.Dead) return;
            StopMovingByGetHit(attacker);
        }

        protected virtual void Dying()
        {
            State = EState.Dead;
            RemoveCorspe();
        }

        protected void RemoveCorspe()
        {
            Destroy(gameObject);
        }
    }
}
