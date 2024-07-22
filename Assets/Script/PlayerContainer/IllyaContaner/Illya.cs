using Assets.Script.Animate.Illya;
using Assets.Script.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Illya
{
    public class Illya : Player
    {
        [SerializeField]
        private float jumpForce = 5f;

        protected override void Setup()
        {
            base.Setup();

            SetMaxHealth(2027);
            SetHealth(2027);
            SetDamage(1677);
            SetMovementSpeed(10f);

            movement = new WalkMovement(this);
            animator = new IllyaAnimator(Sprites.GetComponent<Animator>());
        }

        protected override void SpaceAction()
        {
            if (!Input.GetKey(KeyCode.Space)) return;

            movement.AirMoving(jumpForce);
        }
    }
}
