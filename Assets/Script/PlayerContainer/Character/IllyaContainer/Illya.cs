using Assets.Script;
using Assets.Script.Command;
using Assets.Script.Enum;
using Assets.Script.Game;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer
{
    public class Illya : Player
    {
        [SerializeField]
        private GameObject handleattack;

        public Illya() { }

        private void Start()
        {
            Damage = 1677;
            Health = 2027;
            Speed = 10f;
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void GetCommandByKey()
        {
            if (Input.GetKey(KeyCode.J))
            {
                Attack();
            }
        }

        private void Attack()
        {
            if (State != EState.Free) return;

            var holder = handleattack.GetComponent<MagicBallHolder>();
            var mgballobj = holder.GetMagicBall();

            var mgball = mgballobj.GetComponent<MagicBall>();
            mgball.Damage = Damage;

            IllyaAttack attack = new IllyaAttack(mgballobj, holder.transform);

            DoCommand(attack);
            State = EState.IsAttack;
        }
    }
}
