using Assets.Script.Command;
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

        [SerializeField]
        protected int health = 2000;

        [SerializeField]
        private float speed = 2;

        [SerializeField]
        private float jumpforce = 10;

        private Rigidbody2D body;

        private ICommand command;

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

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            GetCommandByKey();
            UpdatePlayer();
        }

        private void LateUpdate()
        {
            CheckAlive();
        }

        private void UpdatePlayer()
        {
            PlayerMove();
            DoCommand(command);
        }

        private void PlayerMove()
        {
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                command = new PlayerJump(this);
            }

            var x = Input.GetAxis("Horizontal");
            Vector2 pos = new Vector2(x, 0f);

            body.transform.Translate(speed * Time.deltaTime * pos);
            Turn();
        }

        private void CheckAlive()
        {
            if (health > 0) return;

            Destroy(gameObject);
        }

        private void Turn()
        {
            var x = Input.GetAxis("Horizontal");
            var playerscale = transform.localScale;

            if (x > 0.01f)
            {
                playerscale.x = playerscale.x < 0 ? playerscale.x *= -1 : playerscale.x;
            }

            else if (x < -0.01f)
            {
                playerscale.x = playerscale.x > 0 ? playerscale.x *= -1 : playerscale.x;
            }


            transform.localScale = new Vector3(playerscale.x, playerscale.y, playerscale.z); 
        }

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
    }
}
