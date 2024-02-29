using Assets.Script.PlayerContainer.Character.IllyaContainer.Projectile;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer.Form
{
    /// <summary>
    /// For Illya with Casual Form
    /// </summary>
    public class IllyaCasual : PlayerForm
    {
        private Illya illya;

        public IllyaCasual()
        {
            illya = player as Illya;
            if (illya == null) throw new WrongTypeException();

            SetupForm();
        }

        // GET SET //

        protected override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("Prefab/Player/Illya/IllyaCasual");
        }

        protected override string GetIconPath()
        {
            return "UI/Player/Illya/illyacasualicon";
        }

        // NORMAL ATTACK //
        public override void ExcuteAttack()
        {
            var holder = illya.GetMGBallHolder();

            var mgballobj = holder.GetProjectile();

            Shoot(mgballobj, holder.transform);
        }

        private void Shoot(GameObject mgball, Transform position)
        {
            var script = mgball.GetComponent<MagicBall>();
            script.Damage = player.GetDamage();
            script.Owner = player;

            if (player.transform.localScale.x < 0) script.SetDirection(-1f);
            else script.SetDirection(1f);

            script.SetPosition(position);
            script.Active();
        }

        // FIRST SKILL //

        public override void ExcuteFirstSkill()
        {
           
        }

        public override void ExcuteSecondSkill()
        {
            
        }

        public override void ExcuteThirdSkill()
        {
            
        }
    }
}
