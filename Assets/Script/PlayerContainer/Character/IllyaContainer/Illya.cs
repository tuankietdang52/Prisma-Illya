using Assets.Script.Enum;
using Assets.Script.Game.InGameObj;
using Assets.Script.PlayerContainer.Character.IllyaContainer.Form;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer
{
    /// <summary>
    /// Character Illya
    /// <para>This class inherit Player</para>
    /// </summary>
    public class Illya : Player
    {
        [SerializeField]
        private GameObject MGBallHolder;

        public Illya() { }

        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            Damage = 1677;
            MaxHealth = 2027;
            Health = MaxHealth;
            Speed = 10f;
            Form = new IllyaCasual();
        }

        protected override void Update()
        {
            base.Update();
        }

        // GET SET //

        public ProjectileHolder GetMGBallHolder()
        {
            return MGBallHolder.GetComponent<ProjectileHolder>();
        }

        protected override void PressKey()
        {
            if (Input.GetKey(KeyCode.J))
            {
                HandleAttack();
            }
        }

        private void Attack()
        {
            //Call in animation
            Form.ExcuteAttack();
            //Debug.Log("Attacking");
        }

        // ANIMATION //
        private void HandleAttack()
        {
            if (State != EState.Free) return;

            animator.speed = AttackSpeed;
            animator.Play("IllyaAttack");
            State = EState.IsAttack;
            //Debug.Log($"{State}");
        }

        private void StopAttackAnimate()
        {
            //Call in animation
            animator.speed = 1;
            animator.Play("IllyaStand");
            State = EState.Free;
            //Debug.Log($"{State}");
        }
    }
}
