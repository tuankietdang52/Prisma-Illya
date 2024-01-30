using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer
{
    public class MagicBall : MonoBehaviour
    {

        [SerializeField]
        private float speed = 15;

        private bool isHit = false;

        private float _direction = 1f;

        private float existtime = 0;

        private CircleCollider2D circollider => GetComponent<CircleCollider2D>();

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player") return;

            isHit = true;
            Disactive();
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

        public void ResetPosition(Transform origin)
        {
            transform.position = origin.position;
        }

        public void SetDirection(float direction)
        {
            _direction = direction;
        }
    }
}
