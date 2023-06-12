namespace Ediiie.Screens
{
    public class GameScreen : GameUI
    {
        public static GameScreen currentScreen;
    }


    public class GameScreenBackForth : GameScreen
    {
        public Screen backScreen;
        public Screen nextScreen;
        protected void ShowNextScreen()
        {
            ScreenManager.ShowScreen(nextScreen);
        }
    }
}