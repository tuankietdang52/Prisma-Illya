using Assets.Script.Entity.Enemy;
using Assets.Script.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer.Projectile
{
    public class MagicBall : MonoBehaviour, IProjectile
    {
        [SerializeField]
        private float speed = 30f;

        private float _direction = 1f;

        private float existtime = 0;

        private CircleCollider2D circollider => GetComponent<CircleCollider2D>();

        public float Damage { get; set; }

        public MonoBehaviour Owner { get; set; }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            existtime += Time.deltaTime;
            CheckExist();
        }

        private void FixedUpdate()
        {
            Moving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!CheckCollision(collision))
            {
                Disactive();
                return;
            }

            var enemy = collision.collider;
            var script = enemy.GetComponent<ILiveObject>();

            script.DecreaseHealth(Damage);
            script.KnockBack(gameObject);

            Disactive();
        }

        private bool CheckCollision(Collision2D collision)
        {
            var tag = collision.collider.tag;
            var ownertag = Owner.tag;

            // check for owner
            if (ownertag != "Player" && ownertag != "Enemy") return false;

            var rivaltag = ownertag == "Player" ? "Enemy" : "Player";

            if (tag != rivaltag) return false;

            return true;
        }

        public float GetDamage()
        {
            return Damage;
        }

        private void Moving()
        {
            float movement = speed * Time.deltaTime * _direction;
            Vector2 pos = new Vector2(movement, 0f);

            transform.Translate(pos);
        }

        private void CheckExist()
        {
            if (existtime > 5)
            {
                Disactive();
            }
        }

        public void Active()
        {
            gameObject.SetActive(true);
            circollider.enabled = true;
        }

        public void Disactive()
        {
            circollider.enabled = false;
            gameObject.SetActive(false);

            existtime = 0;
        }

        public void SetPosition(Transform position)
        {
            transform.position = position.position;
        }

        public void SetDirection(float direction)
        {
            _direction = direction;
        }
    }
}
