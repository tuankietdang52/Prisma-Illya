using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Game.GameHud
{
    public sealed class HUDManage
    {
        public static void UpdateHealth()
        {
            HealthBar.Instance.UpdateHealth();
        }

        public static void SetCharIcon(Sprite icon)
        {
            CharacterIcon.Instance.SetIcon(icon);
        }
    }
}
