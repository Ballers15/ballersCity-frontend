namespace Ediiie.Screens
{
    //used for popups
    public class GameUI : View
    {
        public bool hasBlockerUI;
        public bool hasAudio;
        public bool isOverlay;
        public Screen screen;

        protected virtual void OnDisable()
        {
            ScreenManager.ShowBlocker(false);
        }

        public void Show(bool on)
        {
            this.gameObject.SetActive(on);
        }
    }
}