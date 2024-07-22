using Assets.Script.Enum;
using Assets.Script.Utility.Game;
using UnityEngine;
using Unity.Mathematics;

namespace Assets.Script.PlayerContainer
{

    /// <summary>
    /// Abstract class for Player
    /// </summary>
    public abstract class Player : Entity {
        public static Player Instance { get; private set; }

        public EState State { get; set; }

        protected override void Setup()
        {
            // Init instance for Player (im using Singleton for Player)
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            _collider = GetComponent<CapsuleCollider2D>();
        }

        protected abstract void SpaceAction();

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            SpaceAction();
            Moving();
        }

        private void Moving()
        {
            float x = Input.GetAxis("Horizontal");
            movement.Moving(x);

            MovingAnimation(x);
        }

        private void MovingAnimation(float x)
        {
            if (x != 0) animator.PlayWalkAnimation();
            else animator.PlayIdleAnimation();

            Vector3 scale = transform.localScale;

            // if x < 0 mean player input is A (left) so we need to turn
            if (x < 0)
            {
                // we need scale x < 0 so player sprites will turn to left
                if (scale.x > 0) transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
            }
            // if player input is D (right), simply make sure scale x always > 0 by using abs
            else if (x > 0) transform.localScale = new Vector3(math.abs(scale.x), scale.y, scale.z);
        }
    }
}