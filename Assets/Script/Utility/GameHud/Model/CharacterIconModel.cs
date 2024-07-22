using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Utility.GameHud.Model
{
    // MVP PATTERN
    public class CharacterIconModel
    {
        private Image icon;

        public event Action IconChanged;

        public CharacterIconModel(Image icon)
        {
            this.icon = icon;
        }

        public void SetIcon(string path)
        {
            var sprite = Resources.Load<Sprite>(path);

            if (sprite == null) throw new NullReferenceException();

            icon.sprite = sprite;
            UpdateIcon();
        }

        public Image GetIcon()
        {
            return icon;
        }

        public void UpdateIcon()
        {
            IconChanged?.Invoke();
        }
    }
}
