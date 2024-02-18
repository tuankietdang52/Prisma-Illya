using Assets.Script.Enum;
using Assets.Script.Game.GameHud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer.Form
{
    public class IllyaCasual : PlayerForm
    {
        private Illya illya;

        public IllyaCasual()
        {
            illya = player as Illya;
            if (illya == null) throw new WrongTypeException();

            SetupForm();
        }

        public override void SetupForm()
        {
            SetupSprites();
            SetupAnimation();
            SetupHUD();
        }

        private void SetupSprites()
        {
            string path = "Sprites/IllyaSprites/Sprites/illyastand";
            var sprites = player.GetComponent<SpriteRenderer>();
            sprites.sprite = Resources.Load<Sprite>($"{path}");
        }

        private void SetupAnimation()
        {
            string path = "Sprites/IllyaSprites/Animation/IllyaCasualAnim";
            var anim = player.GetComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load($"{path}") as RuntimeAnimatorController;
        }

        private void SetupHUD()
        {
            string path = "UI/Illya/illyacasualicon";
            var ico = Resources.Load<Sprite>($"{path}");
            HUDManage.HandleSetCharIcon(ico);
        }


        // NORMAL ATTACK //
        public override void ExcuteAttack()
        {
            var holder = illya.GetMGBallHolder();

            var mgballobj = holder.GetMagicBall();

            Shoot(mgballobj, holder.transform);
        }

        private void Shoot(GameObject mgball, Transform position)
        {
            var script = mgball.GetComponent<MagicBall>();
            script.Damage = player.GetDamage();

            if (player.transform.localScale.x < 0) script.SetDirection(-1f);
            else script.SetDirection(1f);

            script.SetPosition(position);
            script.Active();
        }

        // FIRST SKILL //

        public override void ExcuteFirstSkill()
        {
           
        }

        public override void ExcuteSecondSkill()
        {
            
        }

        public override void ExcuteThirdSkill()
        {
            
        }
    }
}
