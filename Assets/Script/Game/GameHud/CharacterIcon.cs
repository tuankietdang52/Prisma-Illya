using Assets.Script.Enum;
using Assets.Script.PlayerContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Game.GameHud
{
    public class CharacterIcon : MonoBehaviour
    {
        private Player player => Player.Instance;

        private void Start()
        {
            HUDManage.SetCharacterIconObj(this);
        }

        public void Update()
        {
            CheckPlayerDead();
        }

        public void SetIcon(Sprite icon)
        {
            var spriterender = GetComponent<SpriteRenderer>();
            spriterender.sprite = icon;
        }

        private void CheckPlayerDead()
        {
            if (player.State != EState.Dead) return;

            var spriterender = GetComponent<SpriteRenderer>();

            ColorUtility.TryParseHtmlString("#969696", out Color color);
            spriterender.color = color;
        }
    }
}
