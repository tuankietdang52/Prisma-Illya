using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.Game.GameHud;
using Assets.Script.Game.GameHud.Presenter;
using Assets.Script.Game.InGameObj;
using Assets.Script.Interface;
using System.Collections;
using UnityEngine;

namespace Assets.Script.PlayerContainer
{

    /// <summary>
    /// Abstract class for Player
    /// </summary>
    public abstract class Player : Entity {
        public static Player Instance { get; private set; }

        protected IMovement movement;

        public EState State { get; set; }

        protected void Awake()
        {
            // Init instance for Player (im using Singleton for Player)
            if (Instance == null) Instance = this;
            Setup();
        }

        protected abstract void Setup();

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            Moving();
        }

        private void Moving()
        {
            float x = Input.GetAxis("Horizontal");
            movement.Moving(x);
        }
    }
}