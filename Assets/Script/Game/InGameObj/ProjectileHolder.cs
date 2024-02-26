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

            throw new NullReferenceException();
        }
    }
}
