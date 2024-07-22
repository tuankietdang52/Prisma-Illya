using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Utility.Game
{
    public class ProjectileHolder : MonoBehaviour
    {
        [SerializeField]
        protected List<GameObject> listProjectile;

        public GameObject GetProjectile()
        {
            if (gameObject == null) return null;

            foreach (var projectile in listProjectile)
            {
                if (projectile.activeInHierarchy) continue;

                return projectile;
            }

            throw new NullReferenceException();
        }
    }
}
