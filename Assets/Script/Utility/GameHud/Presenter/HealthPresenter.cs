﻿using Assets.Script.Error;
using Assets.Script.Utility.GameHud.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Utility.GameHud.Presenter
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField]
        private HealthModel Health;

        [SerializeField]
        private Image healthbar;

        private void Awake()
        {
            Health = new HealthModel();
            Health.HealthChanged += OnHealthChanged;
        }

        public void Init(float MaxHealth)
        {
            Health.Init(MaxHealth);
        }

        public void SetMaxHealth(float MaxHealth)
        {
            Health.SetMaxHealth(MaxHealth);
        }

        public void SetHealth(float Health)
        {
            this.Health.SetHealth(Health);
        }

        public float GetMaxHealth()
        {
            return Health.GetMaxHealth();
        }

        public float GetHealth()
        {
            return Health.GetHealth();
        }

        public void Restore()
        {
            Health.Restore();
        }

        private void OnHealthChanged()
        {
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            if (Health.GetMaxHealth() == 0) throw new GameLogicException("Max Health cannot equal 0");

            float percent = Health.GetHealth() / Health.GetMaxHealth();

            if (Health.GetHealth() <= 0) percent = 0;

            healthbar.fillAmount = percent;
        }


        private void OnDestroy()
        {
            if (Health != null)
            {
                Health.HealthChanged -= OnHealthChanged;
            }
        }
    }
}
