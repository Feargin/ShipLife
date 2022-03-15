using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine;
using Zenject;

namespace DarkPower.Infrastructure
{
    public class LevelLoader
    {
        [Inject]
        private FadeOnDestroy _fadePanel;

        // [Inject] 
        // private Image _progressBar;

        public async void LoadScene(string levelNum)
        {
#pragma warning disable 618
            if(!Application.isPlaying || Application.isLoadingLevel)
                return;
#pragma warning restore 618
            _fadePanel.FadeImage.DOFade(1, 0.2f);

            await Task.Delay(TimeSpan.FromSeconds(1));
            
                
            // _fadePanel.SetFade(1, 0, 0.001f);
            SceneManager.LoadSceneAsync (levelNum) 
                .AsAsyncOperationObservable () 
                .Do (x =>
                {
                    //_progressBar.fillAmount = x.progress;
                }).Subscribe (x =>
                {
                    //x.allowSceneActivation = false;
                    _fadePanel.FadeImage.DOFade(0, 0.2f);
                });
        }
        
    }
}