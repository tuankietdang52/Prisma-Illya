using Assets.Script.Enum;
using Assets.Script.PlayerContainer;
using UnityEngine;

namespace Assets.Script.Utility.CameraContainer
{
    public class CameraObject : MonoBehaviour
    {
        public static CameraObject Instance { get; private set; }
        private Player player => Player.Instance;
        private Camera _camera => GetComponent<Camera>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

           _camera.backgroundColor = Color.clear;
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            FollowPlayer();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {

        }

        private void FollowPlayer()
        {
            Vector3 pos = new(player.transform.position.x, player.transform.position.y, 1);

            transform.position = pos;
        }
    }
}
