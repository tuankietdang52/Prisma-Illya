using Assets.Script.Entity.Enemy;
using Assets.Script.Enum;
using Assets.Script.Interface;
using UnityEngine;

namespace Assets.Script.Entity.Movement
{
    public class EnemyWalkMovement : IMovement
    {
        private readonly EnemyEntity enemy;
        public EnemyWalkMovement(EnemyEntity enemy)
        {
            this.enemy = enemy;
        }

        public void Move()
        {
            if (enemy.State != EState.Free)
            {
                enemy.IsMoving = true;
                return;
            }

            Turn();

            var currentspeed = enemy.GetSpeed();
            var chasespeed = enemy.GetChaseSpeed();

            var direction = enemy.transform.localScale.x < 0 ? -1f : 1f;

            if (enemy.IsDetectedEnemy()) currentspeed *= chasespeed;

            Vector2 pos = new Vector2(currentspeed * Time.deltaTime * direction, 0f);
            enemy.transform.Translate(pos);
        }

        private void Turn()
        {
            if (!enemy.CanMove()) enemy.Turn();
        }
    }
}
