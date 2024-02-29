using Assets.Script.Game.GameHud;
using System;
using UnityEngine;

namespace Assets.Script.PlayerContainer
{
    /// <summary>
    /// Abstract class for Form of Player Character
    /// </summary>
    public abstract class PlayerForm
    {
        private HUDManage HUD => HUDManage.Instance;
        protected Player player => Player.Instance;

        private GameObject prefab;

        public PlayerForm() { }

        protected void SetupForm()
        {
            prefab = GetPrefab();

            if (prefab == null) throw new NullReferenceException();

            var spriteRender = player.GetComponent<SpriteRenderer>();
            var anim = player.GetComponent<Animator>();

            SetupSprites(spriteRender);
            SetupAnimator(anim);

            HUD.CharacterIconHUD.SetIcon(GetIconPath());
        }

        private void SetupSprites(SpriteRenderer spriteRenderer)
        {
            var sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.sprite = sprite;
        }

        private void SetupAnimator(Animator anim)
        {
            var animation = prefab.GetComponent<Animator>().runtimeAnimatorController;
            anim.runtimeAnimatorController = animation;
        }

        protected abstract GameObject GetPrefab();

        protected abstract string GetIconPath();

        public abstract void ExcuteAttack();

        public abstract void ExcuteFirstSkill();

        public abstract void ExcuteSecondSkill();

        public abstract void ExcuteThirdSkill();
    }
}
