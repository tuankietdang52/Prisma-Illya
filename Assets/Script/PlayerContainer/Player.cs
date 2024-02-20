using Assets.Script.Entity.Enemy;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Game.GameHud;
using Assets.Script.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using EffectOwner = System.Tuple<Assets.Script.Interface.ILiveObject, float>;

namespace Assets.Script.PlayerContainer
{
    public abstract class Player : MonoBehaviour, ILiveObject
    {
        public static Player Instance { get; protected set; }

        public PlayerForm Form { get; protected set; }

        private EState state = EState.Free;
        public EState State
        {
            get => state;
            set
            {
                state = value;
                CheckAction();
            }
        }

        public Dictionary<EEffect, EffectOwner> Effect { get; private set; }

        protected SpriteRenderer spriterender => GetComponent<SpriteRenderer>();

        public bool IsOnGround => GetComponent<Rigidbody2D>().velocity.y == 0;

        protected Animator animator => GetComponent<Animator>();

        // default stats //
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

        [SerializeField]
        private float jumpforce = 10;

        private float _direction = 0.8f;

        private Rigidbody2D body;


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
            _direction = transform.localScale.x > 0 ? _direction : -_direction;

            body = GetComponent<Rigidbody2D>();

            body.constraints = RigidbodyConstraints2D.FreezeRotation;

            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            Effect = GameSystem.InitEffect();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (gameObject == null) return;
            CheckAlive();
        }

        protected virtual void FixedUpdate()
        {
            if (gameObject == null) return;
            GetCommandByKey();
            UpdatePlayer();
        }

        private void UpdatePlayer()
        {
            PlayerMove();
        }

        // GET SET //

        public void SetMaxHealth(float maxhealth)
        {
            MaxHealth = maxhealth;
        }

        public float GetMaxHealth()
        {
            return MaxHealth;
        }

        public void SetHealth(float health)
        {
            Health = health;
            HUDManage.HandleSetHealthBarSize();
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

        // MOVEMENT //

        private void PlayerMove()
        {
            if (State != EState.Free) return;

            if (Input.GetKey(KeyCode.Space) && IsOnGround)
            {
                Jump();
            }

            var x = Input.GetAxis("Horizontal");
            Vector2 pos = new Vector2(x, 0f);

            body.transform.Translate(Speed * Time.deltaTime * pos);
            Turn();
        }

        private void Turn()
        {
            var x = Input.GetAxis("Horizontal");
            var playerscale = transform.localScale;

            if (x > 0.01f)
            {
                _direction = 1f;
                playerscale.x = playerscale.x < 0 ? playerscale.x *= -1 : playerscale.x;
            }

            else if (x < -0.01f)
            {
                _direction = -1f;
                playerscale.x = playerscale.x > 0 ? playerscale.x *= -1 : playerscale.x;
            }


            transform.localScale = new Vector3(playerscale.x, playerscale.y, playerscale.z); 
        }

        // COMMAND //

        private void Jump()
        {
            if (!IsOnGround) return;
            body.velocity = new Vector2(body.velocity.x, jumpforce);
        }

        protected abstract void GetCommandByKey();

        // GET ATTACKED //

        public void DecreaseHealth(float damage)
        {
            Effect.TryGetValue(EEffect.Invulnerable, out var effect);
            float time = effect.Item2;
            if (time > 0) return;

            Health -= damage;
            HUDManage.HandleSetHealthBarSize();

            if (CheckAlive()) return;

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

        private bool CheckAlive()
        {
            if (Health > 0) return false;

            Destroy(gameObject);
            State = EState.Dead;
            return true;
        }

        public bool CanKnockBack() => true;

        public void KnockBack(GameObject attacker)
        {
            if (!CanKnockBack()) return;

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

        private void TurnByKnockBack(float direction)
        {
            float x = transform.localScale.x;

            if (x > 0 && direction > 0 || x < 0 && direction < 0) x *= -1;
            else return;

            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        // WAIT //

        private void CheckFreezeAction()
        {
            //Call in animation
            if (State == EState.Free) return;

            State = EState.Free;
        }

        private void CheckAction()
        {
            float time;
            switch (State)
            {
                case EState.IsKnockBack:
                    time = GameSystem.KnockBackTime;
                    break;

                default:
                    return;
            }

            StopCoroutine(nameof(WaitAction));
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
    }
}