using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Game.InGameObj
{
    public abstract class Entity : MonoBehaviour
    {
        private int health;
        private int maxHealth;
        private int damage;
        private float movementSpeed;

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
    }
}
