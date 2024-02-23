using Assets.Script.Enum;
using Assets.Script.PlayerContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Game.GameHud
{
    public class CharacterIcon : MonoBehaviour
    {
        private Player player => Player.Instance;

        public static CharacterIcon Instance { get; private set; }

        [SerializeField]
        private Image icon;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void Update()
        {
            CheckPlayerDead();
        }

       //public void UpdateIcon<T>(T type, Sprite icon)
       //{
       //    if (type is not CharacterIcon) return;
       //
       //    this.icon.sprite = icon;
       //}

        public void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        private void CheckPlayerDead()
        {
            if (player.State != EState.Dead) return;

            ColorUtility.TryParseHtmlString("#969696", out Color color);
            icon.color = color;
        }
    }
}
