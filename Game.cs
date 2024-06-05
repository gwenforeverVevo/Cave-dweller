using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Game
    {
        private Player _player;
        private List<Bitmap> _playerRunFrames;
        private Bitmap _playerRestBitmap;
        private int _currentFrame;
        private double _frameDuration;
        private double _lastFrameTime;
        private bool _isMoving;

        public Game()
        {
            _player = new Player(100, 10, new Vector2D() { X = 100, Y = 100 });
            _playerRunFrames = LoadFrames("player_run", "asset\\player_run.png", 20); // 4x5 grid
            _playerRestBitmap = SplashKit.LoadBitmap("player_rest", "asset\\player_rest.png");
            _currentFrame = 0;
            _frameDuration = 0.1; // Duration of each frame in seconds
            _lastFrameTime = SplashKit.CurrentTicks();
            _isMoving = false;
        }

        private List<Bitmap> LoadFrames(string baseName, string filePath, int frameCount)
        {
            Bitmap spriteSheet = SplashKit.LoadBitmap(baseName, filePath);
            List<Bitmap> frames = new List<Bitmap>();

            if (spriteSheet == null)
            {
                Console.WriteLine($"Error: Could not load {filePath}!");
                Environment.Exit(1); // Exit the program if the sprite sheet cannot be loaded
            }

            int frameWidth = spriteSheet.Width / 4; // 4 columns
            int frameHeight = spriteSheet.Height / 5; // 5 rows

            for (int i = 0; i < frameCount; i++)
            {
                int srcX = (i % 4) * frameWidth;
                int srcY = (i / 4) * frameHeight;
                Bitmap frame = SplashKit.CreateBitmap($"{baseName}_{i}", frameWidth, frameHeight);
                SplashKit.ClearBitmap(frame, Color.Transparent);
                SplashKit.DrawBitmapOnBitmap(frame, spriteSheet, -srcX, -srcY);
                frames.Add(frame);
            }

            return frames;
        }

        public void Update()
        {
            HandleInput();
            if (_isMoving)
            {
                UpdateAnimation();
            }
        }

        private void UpdateAnimation()
        {
            double currentTime = SplashKit.CurrentTicks();
            if (currentTime - _lastFrameTime > _frameDuration * 1000)
            {
                _currentFrame = (_currentFrame + 1) % _playerRunFrames.Count;
                _lastFrameTime = currentTime;
            }
        }

        public void Draw()
        {
            SplashKit.ClearScreen(Color.White);
            if (_isMoving)
            {
                SplashKit.DrawBitmap(_playerRunFrames[_currentFrame], (float)_player.GetLocation().X, (float)_player.GetLocation().Y);
            }
            else
            {
                SplashKit.DrawBitmap(_playerRestBitmap, (float)_player.GetLocation().X, (float)_player.GetLocation().Y);
            }
            SplashKit.RefreshScreen();
        }

        private void HandleInput()
        {
            Vector2D direction = new Vector2D() { X = 0, Y = 0 };
            _isMoving = false;

            if (SplashKit.KeyDown(KeyCode.WKey)) { direction.Y = -1; _isMoving = true; }
            if (SplashKit.KeyDown(KeyCode.SKey)) { direction.Y = 1; _isMoving = true; }
            if (SplashKit.KeyDown(KeyCode.AKey)) { direction.X = -1; _isMoving = true; }
            if (SplashKit.KeyDown(KeyCode.DKey)) { direction.X = 1; _isMoving = true; }

            if (_isMoving)
            {
                _player.Move(direction);
            }
            else
            {
                _currentFrame = 0; // Reset to the first frame if not moving
            }
        }
    }
}
