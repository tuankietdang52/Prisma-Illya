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
            string path = "Sprites/illyastand";
            var sprites = player.GetComponent<SpriteRenderer>();
            sprites.sprite = Resources.Load<Sprite>($"{path}");

            SetupAnimation();
        }

        private void SetupAnimation()
        {
            string path = "Sprites/Animation/IllyaCasualAnim";
            var anim = player.GetComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load($"{path}") as RuntimeAnimatorController;
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
