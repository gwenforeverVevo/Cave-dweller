using System;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Game
    {
        private Player _player;
        private Bitmap _playerBitmap;

        public Game()
        {
            _player = new Player(100, 10, new Vector2D() { X = 100, Y = 100 });
            _playerBitmap = SplashKit.LoadBitmap("player", "asset\\player.png");

            if (_playerBitmap == null)
            {
                Console.WriteLine("Error: Could not load player bitmap!");
                Environment.Exit(1); // Exit the program if the bitmap cannot be loaded
            }
        }

        public void Update()
        {
            HandleInput();
        }

        public void Draw()
        {
            SplashKit.ClearScreen(Color.White);
            SplashKit.DrawBitmap(_playerBitmap, (float)_player.GetLocation().X, (float)_player.GetLocation().Y);
            SplashKit.RefreshScreen();
        }

        private void HandleInput()
        {
            if (SplashKit.KeyDown(KeyCode.WKey)) _player.Move(new Vector2D() { X = 0, Y = -1 });
            if (SplashKit.KeyDown(KeyCode.SKey)) _player.Move(new Vector2D() { X = 0, Y = 1 });
            if (SplashKit.KeyDown(KeyCode.AKey)) _player.Move(new Vector2D() { X = -1, Y = 0 });
            if (SplashKit.KeyDown(KeyCode.DKey)) _player.Move(new Vector2D() { X = 1, Y = 0 });
        }
    }
}