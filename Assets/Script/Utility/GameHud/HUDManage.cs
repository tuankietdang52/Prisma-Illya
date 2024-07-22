using Assets.Script.Utility.GameHud.Presenter;
using UnityEngine;

namespace Assets.Script.Utility.GameHud
{
    public sealed class HUDManage : MonoBehaviour
    {
        public static HUDManage Instance { get; private set; }

        public HealthPresenter HealthHUD;

        public CharacterIconPresenter CharacterIconHUD;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
