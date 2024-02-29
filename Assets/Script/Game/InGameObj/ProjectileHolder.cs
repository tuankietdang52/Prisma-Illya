using System;
using System.Collections.Generic;
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
