using System;
using System.Collections.Generic;
using System.Text;
using Forgotten_Souls.StateManagement;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace Forgotten_Souls.Screens
{
    public class PauseMenuScreen : MenuScreen
    {

        public Game game;
        public PauseMenuScreen(Game g) : base("Paused")
        {
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            game = g;

            MediaPlayer.Pause();

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to leave so soon?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(game));
        }
    }
}
