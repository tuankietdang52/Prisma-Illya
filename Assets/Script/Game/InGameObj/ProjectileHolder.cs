using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Game.InGameObj
{
    public class ProjectileHolder : MonoBehaviour
    {
        [SerializeField]
        protected List<GameObject> listprojectile;

        public GameObject GetProjectile()
        {
            foreach (var projectile in listprojectile)
            {
                if (projectile.activeInHierarchy) continue;

                return projectile;
            }

            return null;
        }

        private void OnDestroy()
        {
            foreach (var projectile in listprojectile)
            {
                Destroy(projectile);
            }
        }
    }
}
