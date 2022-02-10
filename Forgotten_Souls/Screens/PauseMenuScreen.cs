using System;
using System.Collections.Generic;
using System.Text;
using Forgotten_Souls.StateManagement;

namespace Forgotten_Souls.Screens
{
    public class PauseMenuScreen : MenuScreen
    {
        public PauseMenuScreen() : base("Paused")
        {
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += quitGameMenuEntrySelected;
        }
    }

    private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        const string message = "Are you sure you want to leave so soon?";
        var confirmQuitMessageBox = new MessageBoxScreen(message);

        confirmQuitMessageBox.Accepted += confirmQuitMessageBoxAccepted;

        ScreenManager.AddScreen(confirmQuitMessageBox, controllingPlayer);
    }

    private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
    {
        LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
    }
}
