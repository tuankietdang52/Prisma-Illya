using Assets.Script;
using Assets.Script.Command;
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

        [SerializeField]
        private float CooldownAttack = 1f;

        private float cdattack = 0f;

        private bool isAttack = false;

        public Illya() { }

        protected override void Update()
        {
            base.Update();
            if (isAttack) CheckAttack();
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
            if (isAttack) return;

            var holder = handleattack.GetComponent<MagicBallHolder>();
            var mgball = holder.GetMagicBall();

            IllyaAttack attack = new IllyaAttack(mgball, holder.transform);

            DoCommand(attack);
            isAttack = true;
        }

        private void CheckAttack()
        {
            if (cdattack > CooldownAttack)
            {
                cdattack = 0;
                isAttack = false;
            }

            cdattack += Time.deltaTime;
        }
    }
}
