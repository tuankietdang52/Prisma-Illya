using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.PlayerContainer.Character.IllyaContainer
{
    public class MagicBallHolder : MonoBehaviour
    {
        [SerializeField]
        protected List<GameObject> listball;

        public GameObject GetMagicBall()
        {
            foreach (var mgball in listball)
            {
                if (mgball.activeInHierarchy) continue;

                return mgball;
            }

            return null;
        }
    }
}
