using Zenject;

namespace DarkPower.Infrastructure
{
    public class TransitionView
    {
        public bool LoadingIcon { get; private set; }
        public bool LoadingDoneIcon { get; private set; }
        public string LoadingText { get; private set; }
        public float Progress { get; set; }
        public bool FadeOverlay { get; set; }
        
        private LevelLoader _levelLoader;

        public TransitionView(LevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }

        public void ShowLoadingVisuals()
        {
            LoadingIcon = true;
            LoadingDoneIcon = false;

            Progress = 0f;
            LoadingText = "Loading...";
        }

        public void ShowCompletionVisuals()
        {
            LoadingIcon = false;
            LoadingDoneIcon = true;

            Progress = 1f;
            LoadingText = "Loading complete";
        }
    }
}