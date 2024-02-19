using Assets.Script.Game.CameraContainer;
using Assets.Script.PlayerContainer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Game.GameHud
{
    public class HealthBar : MonoBehaviour
    {
        private Player player => Player.Instance;

        [SerializeField]
        private Image healthbar;

        private void Start()
        {
            HUDManage.SetHealthBarObj(this);
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {

        }

        public void SetSize()
        {
            float maxhealth = player.GetMaxHealth();
            float health = player.GetHealth();


            float percent = health / maxhealth;

            if (health <= 0) percent = 0;

            healthbar.fillAmount = percent;
        }
    }
}
