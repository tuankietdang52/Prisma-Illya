using Assets.Script.Entity;
using Assets.Script.Entity.Movement;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Game.GameHud;
using Assets.Script.Game.GameHud.Presenter;
using System.Collections;
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

        protected HUDManage HUD => HUDManage.Instance;

        public PlayerForm Form { get; protected set; }
        public Collider2D _collider => GetComponent<CapsuleCollider2D>();

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

            body.isKinematic = false;

            Effect = GameSystem.InitEffect();
            movement = new PlayerWalkMovement();
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
            movement.Move();
            UpdateDirection();
        }

        public bool IsOnGround()
        {
            var mask = LayerMask.GetMask("Structure");
            var layer = 1 << mask;

            var position = transform.position;
            var size = new Vector3(1f, _collider.bounds.size.y, _collider.bounds.size.z);

            RaycastHit2D hit = Physics2D.BoxCast(position, size, 0, Vector2.down, 0f, layer);

            if (hit.collider == null) return false;

            return true;
        }

        // GET SET //
        public override void SetHealth(float health)
        {
            base.SetHealth(health);
            HUD.HealthHUD.SetHealth(health);
        }

        public override void SetMaxHealth(float maxhealth)
        {
            base.SetMaxHealth(maxhealth);
            HUD.HealthHUD.SetMaxHealth(maxhealth);
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
            HUD.HealthHUD.SetHealth(Health);

            if (!CheckAlive())
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