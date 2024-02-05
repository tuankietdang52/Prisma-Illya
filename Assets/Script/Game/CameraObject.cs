using Assets.Script;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Enum;
using UnityEngine;

namespace Assets.Script.Game
{
    public class CameraObject : MonoBehaviour
    {
        private Transform player => Player.Instance.transform;
        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void LateUpdate()
        {
            if (Player.Instance.State != EState.Dead) FollowPlayer();
        }

        private void FollowPlayer()
        {
            var pos = new Vector3(player.position.x, player.position.y, -10);

            transform.position = pos;
        }
    }
}
