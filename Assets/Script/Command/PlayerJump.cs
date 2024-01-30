using Assets.Script.Interface;
using Assets.Script.PlayerContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Command
{
    public class PlayerJump : ICommand
    {
        private Player player;

        public PlayerJump() { }

        public PlayerJump(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            player.Jump();
        }
    }
}
