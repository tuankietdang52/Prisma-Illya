using Assets.Script.Enum;
using Assets.Script.Utility.Game;
using Assets.Script.Interface;
using Assets.Script.PlayerContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Script.Movement
{
    public class WalkMovement : IMovement
    {
        private Entity owner;
        public WalkMovement(Entity owner)
        {
            this.owner = owner;
        }

        public void Moving(float x = 0, float y = 0)
        {
            Vector2 pos = new Vector2(x, 0f);
            
            Rigidbody2D rb = owner.GetComponent<Rigidbody2D>();
            rb.transform.Translate(owner.GetMovementSpeed() * Time.deltaTime * pos); ;
        }

        public void AirMoving(float y = 0)
        {
            Jump(y);
        }

        private void Jump(float jumpForce)
        {
            if (!owner.IsOnGround()) return;

            Rigidbody2D rb = owner.GetComponent<Rigidbody2D>();
            rb.velocity = new(rb.velocity.x, jumpForce);
        }
    }
}
