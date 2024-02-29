using Assets.Script.Entity.Movement;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.PlayerContainer;
using UnityEngine;

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

        protected GameObject detectenemy;

        // Stats

        [SerializeField]
        protected GameObject detectObject;

        protected float timecount = 0;

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
        protected bool isDetectedEnemy = false;

        public bool IsMoving { get; set; } = false;

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
            movement = new EnemyWalkMovement(this);
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
            if (player.State == EState.Dead) isDetectedEnemy = false;
            Moving();
            CheckMoving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tag = collision.collider.tag;
            
            switch (tag)
            {
                case "Player":
                    HitEnemy(false);
                    break;

                default:
                    return;
            }

            CheckAlive();
        }

        // GET SET //
        public void SetChaseSpeed(float ChaseSpeed)
        {
            this.ChaseSpeed = ChaseSpeed;
        }
        public float GetChaseSpeed()
        {
            return ChaseSpeed;
        }

        public bool IsDetectedEnemy()
        {
            return isDetectedEnemy;
        }

        // MOVEMENT //

        protected virtual void Moving()
        {
            movement.Move();
        }

        private void MovingAI()
        {
            if (!isDetectedEnemy) DetectEnemy();
            else CheckingUnchase();

            timecount += Time.deltaTime;

            if (isDetectedEnemy) ChaseEnemy();
            else if (timecount >= TimeTurn) Turn();
        }

        private void CheckMoving()
        {
            var pos = detectObject.transform.position;

            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            RaycastHit2D hit = Physics2D.Raycast(pos, direction, 0.1f);

            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                IsMoving = false;
                return;
            }

            IsMoving = true;
        }

        public void Turn()
        {
            _direction = 1f;

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _direction = transform.localScale.x > 0 ? _direction : -_direction;
            timecount = 0;
        }

        // RAYCAST //

        public virtual RaycastHit2D GetDetectPlayerRaycast()
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

        public virtual bool CanMove()
        {
            if (isDetectedEnemy) return true;

            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            RaycastHit2D hit = Physics2D.Raycast(detectObject.transform.position, direction, 0.5f);

            if (hit.collider == null) return true;

            var collider = hit.collider;

            if (!collider.CompareTag("Wall") && !collider.CompareTag("Ground") && !collider.CompareTag("Enemy"))
            {
                return true;
            }

            return false;
        }

        // BEHAVIOR //
        protected abstract void DetectEnemy();

        protected virtual void ChaseEnemy()
        {
            if (detectenemy == null)
            {
                isDetectedEnemy = false;
                return;
            }

            var enemypos = detectenemy.transform.position;
            float x = transform.localScale.x;

            var current = transform.position.x;

            if (current < enemypos.x)
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

            if (hit.collider != null)
            {
                timecount = 0;
                if (hit.collider.gameObject != detectenemy) detectenemy = hit.collider.gameObject;
                return;
            }

            if (!detectenemy.TryGetComponent<LiveObject>(out var enemy)) throw new WrongTypeException();

            if (!enemy.CheckAlive())
            {
                Unchase();
                return;
            }

            if (timecount < TimeChase) return;

            Unchase();
        }

        private void Unchase()
        {
            timecount = 0;
            detectenemy = null;
            isDetectedEnemy = false;
        }

        protected void HitEnemy(bool canIntterupt = true)
        {
            if (isInterrupt && canIntterupt) return;

            if (!detectenemy.TryGetComponent<LiveObject>(out var enemy)) throw new WrongTypeException();

            enemy.GetHitAction(gameObject);
            enemy.DecreaseHealth(Damage);
        }

        // GET HIT //

        public override void DecreaseHealth(float damage)
        {
            if (IsInvulnerable()) return;

            Health -= damage;
            if (Health < 0) Health = 0;

            if (!CheckAlive()) Dying();
        }

        public void StopMovingByGetHit(GameObject attacker)
        {
            float direction = attacker.transform.localScale.x > 0 ? 1f : -1f;
            State = EState.GetHit;
            TurnByKnockBack(direction);
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
