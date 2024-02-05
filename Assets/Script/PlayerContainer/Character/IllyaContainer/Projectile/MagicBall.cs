using Assets.Script.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer
{
    public class MagicBall : MonoBehaviour, IProjectile
    {
        [SerializeField]
        private float speed = 30f;

        private bool isHit = false;

        private float _direction = 1f;

        private float existtime = 0;

        private CircleCollider2D circollider => GetComponent<CircleCollider2D>();

        public float Damage { get; set; }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            if (isHit) return;

            existtime += Time.deltaTime;
            CheckExist();
            Moving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tag = collision.collider.tag;
            if (tag == "Player") return;
            if (tag == "Projectile" || tag == "Skill") return;

            isHit = true;
            Disactive();
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
            existtime = 0;
            circollider.enabled = false;
            gameObject.SetActive(false);
            isHit = false;
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
