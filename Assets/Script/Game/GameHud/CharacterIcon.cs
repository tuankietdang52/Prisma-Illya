﻿using Assets.Script.Enum;
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

        [SerializeField]
        private Image icon;

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