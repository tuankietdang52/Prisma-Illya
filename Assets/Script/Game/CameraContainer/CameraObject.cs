using Assets.Script;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Enum;
using UnityEngine;

namespace Assets.Script.Game.CameraContainer
{
    public class CameraObject : MonoBehaviour
    {
        public static CameraObject Instance { get; private set; }
        private Transform player => Player.Instance.transform;
        private Camera _camera => GetComponent<Camera>();
        // Start is called before the first frame update
        private void Awake()
        {
            Instance = this;
           _camera.backgroundColor = Color.clear;
            DontDestroyOnLoad(this);
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            if (Player.Instance.State != EState.Dead) FollowPlayer();
        }

        private void FollowPlayer()
        {
            var pos = new Vector3(player.position.x, player.position.y, 1);

            transform.position = pos;
        }
    }
}
