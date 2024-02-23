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
            if (gameObject == null) return null;

            foreach (var projectile in listprojectile)
            {
                if (projectile.activeInHierarchy) continue;

                return projectile;
            }

            return null;
        }

        private void OnDestroy()
        {
            if (gameObject == null) return;

            foreach (var projectile in listprojectile)
            {
                if (projectile == null) continue;
                if (projectile.activeInHierarchy) continue;
                
                Destroy(projectile);
            }
        }
    }
}
