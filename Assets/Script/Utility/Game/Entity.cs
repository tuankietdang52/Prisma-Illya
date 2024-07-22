using Assets.Script.Animate;
using Assets.Script.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Utility.Game
{
    public abstract class Entity : MonoBehaviour
    {
        #region Basic Stat Field

        private int health;
        private int maxHealth;
        private int damage;
        private float movementSpeed;

        #endregion

        protected Collider2D _collider;
        protected AnimationManager animator;
        protected IMovement movement;

        [SerializeField]
        protected GameObject Sprites;

        #region Get Set

        public void SetHealth(int health)
        {
            this.health = health;
        }

        public void SetMaxHealth(int maxHealth)
        {
            this.maxHealth = maxHealth;
        }

        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        public void SetMovementSpeed(float speed)
        {
            this.movementSpeed = speed;
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetDamage()
        {
            return damage;
        }

        public float GetMovementSpeed()
        {
            return movementSpeed;
        }

        #endregion

        void Awake()
        {
            if (Sprites == null)
            {
                throw new NullReferenceException("Look like this object is missing sprites");
            }

            Setup();
        }

        protected abstract void Setup();

        public bool IsOnGround()
        {
            // get layer mask
            // layer mask can put one or multiple layer to one layer, so we can filt it
            int layerMask = LayerMask.GetMask("Structure");

            /*
             * AABB box, dont use transform.position, it give a topleft position or ...pluh, idk
             * but it hardly give center of game object
             * so we using collider bound which we can see in editor
             */
            Vector3 position = _collider.bounds.center;
            Vector3 size = new(1f, _collider.bounds.size.y, _collider.bounds.size.z);

            RaycastHit2D hit = Physics2D.BoxCast(position, size, 0f, Vector2.down, 0f, layerMask);

            if (hit.collider == null) return false;

            return true;
        }

        /// <summary>
        /// for debug
        /// </summary>
        //private void OnDrawGizmos()
        //{
        //    var _collider = GetComponent<CapsuleCollider2D>();

        //    Vector3 pos = _collider.bounds.center;

        //    Vector3 size = new Vector3(1f, _collider.bounds.size.y, _collider.bounds.size.z);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireCube(pos, size);
        //}
    }
}
