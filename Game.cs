﻿// File path: Cave_dweller/Game.cs
using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Game
    {
        private Player _player;
        private List<Goblin> _goblins;
        private List<Bitmap> _playerRunFrames;
        private Bitmap _playerRestBitmap;
        private Bitmap _cursorBitmap; // Add cursor bitmap
        private int _currentFrame;
        private double _frameDuration;
        private double _lastFrameTime;
        private bool _isMoving;
        private bool _showHitboxes;
        private SplashKitSDK.Timer _gameTimer;
        private bool _gameOver;

        public Game()
        {
            _player = new Player(3, 10, new Vector2D() { X = 100, Y = 100 }, 0.6);
            _goblins = new List<Goblin>
            {
                new Goblin(new Vector2D() { X = 500, Y = 200 }),
                new Goblin(new Vector2D() { X = 500, Y = 300 })
            };

            AssetLoader assetLoader = new AssetLoader();
            _playerRunFrames = assetLoader.LoadFrames("player_run", "asset\\player_run.png", 20);
            _playerRestBitmap = assetLoader.LoadBitmap("player_rest", "asset\\player_rest.png");

            // Load the cursor bitmap
            _cursorBitmap = assetLoader.LoadBitmap("cursor", "asset\\cursor.png");

            _currentFrame = 0;
            _frameDuration = 0.1; // Duration of each frame in seconds
            _lastFrameTime = SplashKit.CurrentTicks();
            _isMoving = false;
            _showHitboxes = false; // Hitboxes are hidden by default

            _gameTimer = SplashKit.CreateTimer("game_timer");
            SplashKit.StartTimer(_gameTimer);
            _gameOver = false;
        }

        public void Update()
        {
            if (_gameOver)
            {
                return;
            }

            if (SplashKit.KeyTyped(KeyCode.HKey))
            {
                _showHitboxes = !_showHitboxes; // Toggle hitbox visibility
            }

            GameStateUpdater.UpdateGameState(_player, _goblins);
            ProjectileManager.UpdateProjectiles(_player, _goblins);

            int playerSpriteWidth = _playerRestBitmap.Width;
            int playerSpriteHeight = _playerRestBitmap.Height;
            InputHandler.HandleInput(_player, ref _isMoving, playerSpriteWidth, playerSpriteHeight);

            _player.HandleInput();
            _player.UpdateReloadAnimation();

            foreach (Goblin goblin in _goblins)
            {
                goblin.AttackPlayer(_player);
                if (_player.IsDead)
                {
                    _gameOver = true;
                    return;
                }
            }

            if (_isMoving)
            {
                AnimationManager.UpdateAnimation(ref _currentFrame, ref _lastFrameTime, _frameDuration, _playerRunFrames.Count);
            }
        }

        public void Draw()
        {
            SplashKit.ClearScreen(Color.White);

            if (_gameOver)
            {
                SplashKit.DrawText("Game Over", Color.Red, 700, 400);
                SplashKit.RefreshScreen();
                return;
            }

            GameDrawer.DrawGame(_player, _goblins, _playerRunFrames, _playerRestBitmap, _currentFrame, _isMoving, _showHitboxes);
            _player.DrawReloadMessage();

            // Draw the cursor at the current mouse position
            SplashKit.DrawBitmap(_cursorBitmap, SplashKit.MouseX(), SplashKit.MouseY());

            SplashKit.RefreshScreen();
        }
    }
}
