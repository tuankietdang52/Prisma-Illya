﻿using Assets.Script.Entity;
using Assets.Script.Entity.Movement;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Game.GameHud;
using Assets.Script.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectOwner = System.Tuple<Assets.Script.Entity.LiveObject, float>;

namespace Assets.Script.PlayerContainer
{

    /// <summary>
    /// Abstract class for Player
    /// <para>This class inherit Live Object class</para>
    /// </summary>
    public abstract class Player : LiveObject
    {
        public static Player Instance { get; protected set; }

        protected IMovement Movement;

        public PlayerForm Form { get; protected set; }
        public Collider2D _collider => GetComponent<CapsuleCollider2D>();

        public bool IsOnGround => GetComponent<Rigidbody2D>().velocity.y == 0;


        [SerializeField]
        private float jumpforce = 10;

        [SerializeField]
        private float direction = 1f;
        public float Direction
        {
            get => direction;
            set => direction = value;
        }


        private void Awake()
        {
            if (gameObject == null)
            {
                Debug.Log($"{gameObject.name} is null");
                return;
            }

            if (Instance == null)
            {
                Instance = this;
            }

            Setup();
            DontDestroyOnLoad(gameObject);
        }

        private void Setup()
        {
            Direction = transform.localScale.x > 0 ? Direction : -Direction;

            body.constraints = RigidbodyConstraints2D.FreezeRotation;

            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            Effect = GameSystem.InitEffect();
            Movement = new PlayerWalkMovement();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (gameObject == null || State == EState.Dead) return;
        }

        protected virtual void FixedUpdate()
        {
            if (gameObject == null || State == EState.Dead) return;
            PressKey();
            UpdatePlayer();
        }

        /// <summary>
        /// Handle action while player pressed key
        /// </summary>
        protected abstract void PressKey();

        private void UpdatePlayer()
        {
            Movement.Move();
            UpdateDirection();
        }

        // GET SET //

        public override void SetHealth(float health)
        {
            base.SetHealth(health);
            HUDManage.UpdateHealth();
        }

        public void SetJumpForce(float jumpforce)
        {
            this.jumpforce = jumpforce;
        }
        public float GetJumpForce()
        {
            return jumpforce;
        }

        // Player Interface //
        private void UpdateDirection()
        {
            var x = Input.GetAxis("Horizontal");
            var playerscale = transform.localScale;

            if (x > 0.01f)
            {
                Direction = 1f;
                playerscale.x = playerscale.x < 0 ? playerscale.x *= -1 : playerscale.x;
            }

            else if (x < -0.01f)
            {
                Direction = -1f;
                playerscale.x = playerscale.x > 0 ? playerscale.x *= -1 : playerscale.x;
            }


            transform.localScale = new Vector3(playerscale.x, playerscale.y, playerscale.z);
        }

        // GET ATTACKED //

        public override void DecreaseHealth(float damage)
        {
            if (IsInvulnerable()) return;

            Health -= damage;
            if (Health < 0) Health = 0;
            HUDManage.UpdateHealth();

            if (CheckAlive())
            {
                Dying();
                return;
            }

            animator.SetTrigger("getHit");
            StartCoroutine(BeingInvulnerable());
        }

        private IEnumerator BeingInvulnerable()
        {
            var player = LayerMask.NameToLayer("Player");
            var enemy = LayerMask.NameToLayer("Enemy");
            var projectile = LayerMask.NameToLayer("ProjectileEnemy");

            float time = 0;

            Physics2D.IgnoreLayerCollision(player, enemy, true);
            Physics2D.IgnoreLayerCollision(player, projectile, true);

            Effect.TryGetValue(EEffect.Invulnerable, out var effect);
            Effect[EEffect.Invulnerable] = new EffectOwner(effect.Item1, 1.5f);

            while (time < 1.5f)
            {
                spriterender.color = new Color(1, 1, 1, 0.4f);

                // hold this process in 0.5 seconds
                // when done this func will get called in next frame by start coroutine
                yield return new WaitForSeconds(0.5f);
                time += 0.5f;
            }

            spriterender.color = Color.white;
            Effect[EEffect.Invulnerable] = new EffectOwner(effect.Item1, 0f);
            Physics2D.IgnoreLayerCollision(player, enemy, false);
            Physics2D.IgnoreLayerCollision(player, projectile, false);
        }

        public override void GetHitAction(GameObject attacker)
        {
            KnockBack(attacker);
        }

        private bool CheckAlive()
        {
            if (Health > 0) return false;

            return true;
        }

        protected virtual void Dying()
        {
            State = EState.Dead;
            animator.SetTrigger("isDefeat");

            SetCollider(false, _collider);
            Effect.Clear();
        }

        private void CheckFreezeAction()
        {
            //Call in animation
            if (State == EState.Free) return;

            State = EState.Free;
        }
    }
}