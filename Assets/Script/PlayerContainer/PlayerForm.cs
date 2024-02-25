using Assets.Script.Game.GameHud;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.PlayerContainer
{
    /// <summary>
    /// Abstract class for Form of Player Character
    /// </summary>
    public abstract class PlayerForm
    {
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
            SetupIcon();
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

        private void SetupIcon()
        {
            var ico = GetIcon();

            if (ico == null) throw new NullReferenceException();

            HUDManage.SetCharIcon(ico);
        }

        protected abstract GameObject GetPrefab();

        protected abstract Sprite GetIcon();

        public abstract void ExcuteAttack();

        public abstract void ExcuteFirstSkill();

        public abstract void ExcuteSecondSkill();

        public abstract void ExcuteThirdSkill();
    }
}
