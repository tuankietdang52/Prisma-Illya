using Assets.Script.Game.CameraContainer;
using Assets.Script.PlayerContainer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Game.GameHud
{
    public class HealthBar : MonoBehaviour
    {
        private readonly float size = 2;
        private Player player => Player.Instance;

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

            float newsize = size * percent;

            if (health <= 0) newsize = 0;

            transform.localScale = new Vector3(newsize, transform.localScale.y);
        }
    }
}
