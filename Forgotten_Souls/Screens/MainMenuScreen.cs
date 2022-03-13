using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Forgotten_Souls.StateManagement;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Forgotten_Souls.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        private Song menuMusic;
        private ContentManager content;
        public Game game;
       

        public MainMenuScreen(Game g) : base("Forgotten Souls")
        {
            var playGameMenuEntry = new MenuEntry("Play Game");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            game = g;
            

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(game));
        }

        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            menuMusic = content.Load<Song>("Phantom");
            MediaPlayer.Play(menuMusic);
        }


        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to leave so soon?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Play(menuMusic);
            ScreenManager.Game.Exit();
        }
    }
}
