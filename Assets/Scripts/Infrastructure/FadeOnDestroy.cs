using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DarkPower.Infrastructure
{
    public class FadeOnDestroy : MonoBehaviour
    {
        public Image FadeImage { get; set; }

        public Canvas _canvas;
        // public ColorAdjustments ColorAdjustments;

        [Inject]
        private void Constructor()
        {
            FadeImage = GetComponentInChildren<Image>();
            _canvas = GetComponentInChildren<Canvas>();
            _canvas.worldCamera = Camera.main;
            
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if(!_canvas.worldCamera)
                _canvas.worldCamera = Camera.main;
        }
    }
}