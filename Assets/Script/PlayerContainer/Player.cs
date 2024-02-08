using Assets.Script.Entity.Enemy;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Script.PlayerContainer
{
    public abstract class Player : MonoBehaviour, ILiveObject
    {
        public static Player Instance { get; private set; }

        public PlayerForm Skill { get; protected set; }

        private EState state = EState.Free;
        public EState State
        {
            get => state;
            set
            {
                ResetTimeWaitAction(value);
                state = value;
            }
        }

        protected Sprite sprite;

        private bool isGrounded => GetComponent<Rigidbody2D>().velocity.y == 0;

        protected Animator animator => GetComponent<Animator>();

        // default stats //
        [SerializeField]
        protected float Health;

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

        private float statetime = 0f;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            body = GetComponent<Rigidbody2D>();
            DontDestroyOnLoad(gameObject);
        }

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
            if (gameObject == null) return;
            CheckAction();
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

        public void SetHealth(float health)
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

        // MOVEMENT //

        private void PlayerMove()
        {
            if (State != EState.Free) return;

            if (Input.GetKey(KeyCode.Space) && isGrounded)
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

        public void Jump()
        {
            if (!isGrounded) return;
            body.velocity = new Vector2(body.velocity.x, jumpforce);
        }

        protected abstract void GetCommandByKey();

        // GET ATTACKED //

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Enemy")) return;

            GetHit(collision);
        }

        private void GetHit(Collision2D collision)
        {
            var enemy = collision.collider.GetComponent<EnemyEntity>();
            float damage = enemy.GetDamage();

            Health -= damage;

            CheckAlive();
        }

        private void CheckAlive()
        {
            if (Health > 0) return;

            Destroy(gameObject);
            State = EState.Dead;
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
        public void ResetTimeWaitAction(EState State)
        {
            if (this.State != State) statetime = 0;
        }

        private void CheckFreezeAction()
        {
            if (State == EState.Free) return;

            State = EState.Free;
        }

        private void CheckAction()
        {
            switch (State)
            {
                case EState.IsKnockBack:
                    WaitAction(EState.IsKnockBack, GameSystem.KnockBackTime);
                    break;

                default:
                    break;
            }
        }

        protected void WaitAction(EState state, float time)
        {
            if (statetime >= time)
            {
                statetime = 0;
                State = EState.Free;
            }

            statetime += Time.deltaTime;
        }
    }
}