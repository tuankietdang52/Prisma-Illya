using Assets.Script.Enum;
using Assets.Script.Game.GameHud.Model;
using Assets.Script.PlayerContainer;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Game.GameHud.Presenter
{
    public class CharacterIconPresenter : MonoBehaviour
    {
        private CharacterIconModel iconModel;

        private Player player => Player.Instance;

        [SerializeField]
        private Image icon;

        public void Awake()
        {
            iconModel = new CharacterIconModel(icon);
            iconModel.IconChanged += OnIconChanged;
        }

        private void Update()
        {
            if (!IsPlayerDead()) return;

            ColorUtility.TryParseHtmlString("#969696", out Color color);
            ChangeIconColor(color);
        }

        public void SetIcon(string path)
        {
            try
            {
                iconModel.SetIcon(path);
            }
            catch (Exception ex)
            {
                Debug.Log("Cant set icon\n" + ex.Message);
            }
        }

        public void GetIcon()
        {
            iconModel.GetIcon();
        }

        private bool IsPlayerDead()
        {
            if (player.State != EState.Dead) return false;

            return true;
        }

        public void ChangeIconColor(Color color)
        {
            icon.color = color;
        }

        public void OnIconChanged()
        {
            UpdateHUD();
        }

        public void UpdateHUD()
        {
            icon = iconModel.GetIcon();
        }
    }
}
