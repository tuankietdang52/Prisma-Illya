using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Animate
{
    public abstract class AnimationManager
    {
        protected Animator ownerAnimator;

        public AnimationManager(Animator ownerAnimator)
        {
            this.ownerAnimator = ownerAnimator;
        }

        /// <summary>
        /// Set all bool int animator parameter to false
        /// </summary>
        protected void ResetBoolParameter()
        {
            AnimatorControllerParameter[] parameter = ownerAnimator.parameters;

            for (int i = 0; i < parameter.Length; i++)
            {
                try
                {
                    ownerAnimator.SetBool(parameter[i].name, false);
                }
                catch { }
            }
        }

        public abstract void PlayIdleAnimation();
        public abstract void PlayWalkAnimation();
        public abstract void PlayRunAnimation();
        public abstract void PlayAttackAnimation();
        public abstract void PlayGetHitAnimation();
        public abstract void PlayDeathAnimation();
    }
}
