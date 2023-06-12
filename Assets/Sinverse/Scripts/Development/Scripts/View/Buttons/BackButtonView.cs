using Ediiie.Screens;

namespace EDIIIE
{
    public class BackButtonView : BaseButtonView
    {
        protected override void OnButtonClicked()
        {
            ScreenManager.ShowBackScreen();
        }
    }
}
