using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Animate.Illya
{
    public class IllyaAnimator : AnimationManager
    {
        public IllyaAnimator(Animator ownerAnimator) : base(ownerAnimator) { }

        public override void PlayIdleAnimation()
        {
            ResetBoolParameter();
        }

        public override void PlayWalkAnimation()
        {
            ownerAnimator.SetBool("isWalk", true);
        }

        public override void PlayRunAnimation()
        {
            throw new NotImplementedException();
        }

        public override void PlayAttackAnimation()
        {
            throw new NotImplementedException();
        }

        public override void PlayGetHitAnimation()
        {
            throw new NotImplementedException();
        }

        public override void PlayDeathAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
