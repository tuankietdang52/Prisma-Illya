﻿using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectOwner = System.Tuple<Assets.Script.Entity.LiveObject, float>;

namespace Assets.Script.Entity
{
    /// <summary>
    /// An abstract class for all living object
    /// </summary>
    public abstract class LiveObject : MonoBehaviour
    {
        public SpriteRenderer spriterender => GetComponent<SpriteRenderer>();
        public Animator animator => GetComponent<Animator>();
        public Rigidbody2D body => GetComponent<Rigidbody2D>();

        protected IMovement movement;

        [Header ("State")]
        [SerializeField]
        private EState state = EState.Free;
        public EState State
        {
            get => state;
            set
            {
                if (state != EState.Dead) state = value;
                HandleWaitAction();
            }
        }
        public Dictionary<EEffect, EffectOwner> Effect { get; protected set; }

        [Header ("Stats")]
        [SerializeField]
        protected float Health;

        [SerializeField]
        protected float MaxHealth;

        [SerializeField]
        protected float AttackSpeed = 1f;

        [SerializeField]
        protected float Damage;

        [SerializeField]
        protected float Speed;

        // GET SET //
        public virtual void SetMaxHealth(float maxhealth)
        {
            float percent = Health / this.MaxHealth;
            MaxHealth = maxhealth;

            if (Health <= 0) Health = 0;
            else Health = MaxHealth * percent;
        }

        public float GetMaxHealth()
        {
            return MaxHealth;
        }

        public virtual void SetHealth(float health)
        {
            Health = health;
        }

        public float GetHealth()
        {
            return Health;
        }
        public float GetAttackSpeed()
        {
            return AttackSpeed;
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        public float GetDamage()
        {
            return Damage;
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public float GetSpeed()
        {
            return Speed;
        }

        /// <summary>
        /// Decrease health of living object in game
        /// </summary>
        /// <param name="damage">Attacker Damage</param>
        public abstract void DecreaseHealth(float damage);

        /// <summary>
        /// Action of object when getting hit
        /// </summary>
        /// <param name="attacker"></param>
        public abstract void GetHitAction(GameObject attacker);

        /// <summary>
        /// Check if object is alive
        /// </summary>
        public virtual bool CheckAlive()
        {
            if (Health > 0) return true;

            return false;
        }

        public bool IsInvulnerable()
        {
            Effect.TryGetValue(EEffect.Invulnerable, out var effect);
            float time = effect.Item2;
            if (time > 0) return true;

            return false;
        }

        /// <summary>
        /// Knockback this object
        /// <para>
        /// Place above DecreaseHealth(float damage) function if both in the same function
        /// </para>
        /// </summary>
        /// <param name="attacker"></param>
        public virtual void KnockBack(GameObject attacker)
        {
            if (IsInvulnerable()) return;

            if (State == EState.Dead) return;
            if (State == EState.IsKnockBack) return;

            State = EState.IsKnockBack;
            float atkpos = attacker.transform.position.x;
            float direction;

            if (atkpos > transform.position.x) direction = -1f;
            else direction = 1f;

            float x = GameSystem.AttackKnockBackForce.x;
            float y = GameSystem.AttackKnockBackForce.y;

            Vector2 pos = new Vector2(x * direction, y);

            body.velocity = Vector2.zero;
            body.AddForce(pos, ForceMode2D.Impulse);

            TurnByKnockBack(direction);
        }

        protected void TurnByKnockBack(float direction)
        {
            float x = transform.localScale.x;

            if (x > 0 && direction > 0 || x < 0 && direction < 0) x *= -1;
            else return;

            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        private void HandleWaitAction()
        {
            float time;
            switch (State)
            {
                case EState.IsKnockBack:
                    time = GameSystem.KnockBackTime;
                    break;

                case EState.GetHit:
                    time = GameSystem.KnockBackTime;
                    break;

                default:
                    return;
            }

            StopCoroutine(nameof(WaitAction));

            if (State == EState.Dead) return;
            StartCoroutine(WaitAction(time));
        }

        protected IEnumerator WaitAction(float donetime)
        {
            float time = 0;
            while (time < donetime)
            {
                yield return new WaitForSeconds(0.2f);
                time += 0.2f;
            }

            if (State != EState.Dead) State = EState.Free;
        }

        public void SetCollider(bool isEnable, Collider2D _collider)
        {
            _collider.enabled = isEnable;
            body.velocity = Vector2.zero;
            body.isKinematic = true;
            body.gravityScale = 0;
        }
    }
}
