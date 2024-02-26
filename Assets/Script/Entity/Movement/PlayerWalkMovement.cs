using Assets.Script.Enum;
using Assets.Script.Interface;
using Assets.Script.PlayerContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Script.Entity.Movement
{
    public class PlayerWalkMovement : IMovement
    {
        private Player player => Player.Instance;
        public PlayerWalkMovement() { }

        public void Move()
        {
            if (player.State != EState.Free) return;

            var x = Input.GetAxis("Horizontal");
            Vector2 pos = new Vector2(x, 0f);

            if (Input.GetKey(KeyCode.Space) && player.IsOnGround)
            {
                Jump();
            }

            player.body.transform.Translate(player.GetSpeed() * Time.deltaTime * pos);;

            // when key not pressed, Input.GetAxis("Horizontal") will return 0;
            // when jump will not active animation
            if (player.IsOnGround) player.animator.SetBool("isMoving", x != 0);
            else player.animator.SetBool("isMoving", false);
        }

        private void Jump()
        {
            if (!player.IsOnGround) return;
            player.body.velocity = new Vector2(player.body.velocity.x, player.GetJumpForce());
        }
    }
}
