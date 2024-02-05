using Assets.Script.Interface;
using Assets.Script.PlayerContainer;
using Assets.Script.PlayerContainer.Character.IllyaContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Command
{
    public class IllyaAttack : ICommand
    {
        private GameObject mgball;
        private Transform position;

        public IllyaAttack(GameObject mgball, Transform position)
        {
            var check = mgball.GetComponent<MagicBall>();
            if (check == null)
            {
                throw new WrongTypeException();
            }

            this.mgball = mgball;
            this.position = position;
        }

        public void Execute()
        {
            Shoot();
        }

        private void Shoot()
        {
            var script = mgball.GetComponent<MagicBall>();

            if (Player.Instance.transform.localScale.x < 0) script.SetDirection(-1f);
            else script.SetDirection(1f);

            script.SetPosition(position);
            script.Active();
        }
    }
}
