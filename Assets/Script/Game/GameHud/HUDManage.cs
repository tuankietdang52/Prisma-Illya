using Assets.Script.Game.GameHud.Presenter;
using UnityEngine;

namespace Assets.Script.Game.GameHud
{
    public sealed class HUDManage : MonoBehaviour
    {
        public static HUDManage Instance { get; private set; }

        public HealthPresenter HealthHUD;

        public CharacterIconPresenter CharacterIconHUD;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
