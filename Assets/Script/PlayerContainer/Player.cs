using Assets.Script.Command;
using Assets.Script.Entity.Enemy;
using Assets.Script.Enum;
using Assets.Script.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.PlayerContainer
{
    public abstract class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        public EState State { get; protected set; } = EState.Free;

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

        private ICommand command;

        private float cdattack = 0f;

        private bool isGrounded => GetComponent<Rigidbody2D>().velocity.y == 0;

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
            _direction = transform.localScale.x > 0 ? _direction : -_direction;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            GetCommandByKey();
            UpdatePlayer();
        }

        private void UpdatePlayer()
        {
            PlayerMove();
            DoCommand(command);
        }

        // GET SET //

        public float GetAttackSpeed()
        {
            return AttackSpeed;
        }


        // MOVEMENT //

        private void PlayerMove()
        {
            if (State != EState.Free) return;

            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                command = new PlayerJump(this);
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

        public void DoCommand(ICommand Command = null)
        {
            Command ??= this.command;

            Command?.Execute();
            this.command = null;
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
            KnockBack(damage);
        }

        private void CheckAlive()
        {
            if (Health > 0) return;

            Destroy(gameObject);
        }

        public void KnockBack(float damage)
        {
            float force = damage / 50;
            float y = jumpforce / 2;

            Vector2 pos = new Vector2(force * _direction * -1f, y);

            body.AddForce(pos, ForceMode2D.Impulse);
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
