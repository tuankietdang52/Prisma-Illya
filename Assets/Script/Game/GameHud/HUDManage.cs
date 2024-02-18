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
        private static HealthBar healthbar;
        private static CharacterIcon charactericon;

        public static void SetHealthBarObj(HealthBar HealthBar)
        {
            healthbar = HealthBar;
        }

        public static void SetCharacterIconObj(CharacterIcon characterIcon)
        {
            charactericon = characterIcon;
        }

        public static void HandleSetHealthBarSize()
        {
            healthbar.SetSize();
        }

        public static void HandleSetCharIcon(Sprite icon)
        {
            charactericon.SetIcon(icon);
        }
    }
}
