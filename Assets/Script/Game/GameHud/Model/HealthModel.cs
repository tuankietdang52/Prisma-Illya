using Assets.Script.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Game.GameHud.Model
{
    public class HealthModel
    {
        [SerializeField]
        private float Health = 0;

        [SerializeField]
        private float MaxHealth = 0;

        public event Action HealthChanged;

        public HealthModel() { }

        public void Init(float MaxHealth)
        {
            this.MaxHealth = MaxHealth;
            Health = MaxHealth;
        }

        public void SetMaxHealth(float MaxHealth)
        {
            this.MaxHealth = MaxHealth;

            UpdateHealth();
        }

        public void SetHealth(float Health)
        {
            if (MaxHealth == 0) throw new ConflictLogicException("Max Health cannot be lower than Health");

            this.Health = Health;

            if (this.Health > MaxHealth) this.Health = MaxHealth;
            if (this.Health < 0) this.Health = 0;
            UpdateHealth();
        }

        public float GetMaxHealth()
        {
            return MaxHealth;
        }

        public float GetHealth()
        {
            return Health;
        }

        public void Restore()
        {
            Health = MaxHealth;
            UpdateHealth();
        }

        public void UpdateHealth()
        {
            HealthChanged?.Invoke();
        }
    }
}
