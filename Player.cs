﻿// File path: Cave_dweller/Player.cs
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Player : Character
    {
        private const int MaxAmmo = 10;
        private const double ReloadDuration = 2000; // 2 seconds reload time
        public int Ammunition { get; private set; }
        private Vector2D Facing { get; set; }
        public List<Projectile> Projectiles { get; private set; }
        private string _reloadMessage;
        private int _reloadAnimationStep;
        private double _lastReloadTime;
        private bool _isReloading;
        private bool _showReloadPrompt;
        private double _speed;

        public Player(int initialHealth, int initialAmmo, Vector2D startLocation, double speed = 1.0)
            : base(initialHealth, startLocation)
        {
            Ammunition = initialAmmo;
            Facing = new Vector2D() { X = 1, Y = 0 };
            Projectiles = new List<Projectile>();
            _reloadMessage = string.Empty;
            _reloadAnimationStep = 0;
            _lastReloadTime = 0;
            _isReloading = false;
            _showReloadPrompt = false;
            _speed = speed; // Initialize speed
        }

        public override void Move(Vector2D direction)
        {
            Vector2D newLocation = GetLocation();
            newLocation.X += direction.X * _speed; // Apply speed multiplier
            newLocation.Y += direction.Y * _speed; // Apply speed multiplier
            SetLocation(newLocation);
            Facing = direction;
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            if (_health <= 0)
            {
                Console.WriteLine("Player has died.");
            }
        }

        public void ShootProjectile(Vector2D target, int spriteWidth, int spriteHeight)
        {
            if (_isReloading) return;

            if (Ammunition > 0)
            {
                Vector2D startPosition = GetLocation();
                startPosition.X += 25; // Adjusted for hitbox center
                startPosition.Y += 25; // Adjusted for hitbox center
                Vector2D direction = SubtractVectors(target, startPosition);
                direction = SplashKit.UnitVector(direction); // Normalize direction vector
                Projectile newProjectile = new Projectile(startPosition, direction);
                Projectiles.Add(newProjectile);
                Ammunition--;
                if (Ammunition == 0)
                {
                    _showReloadPrompt = true;
                }
            }
            else
            {
                _showReloadPrompt = true;
            }
        }

        private void StartReload()
        {
            if (!_isReloading)
            {
                _isReloading = true;
                _reloadMessage = "Reloading";
                _reloadAnimationStep = 0;
                _lastReloadTime = SplashKit.TimerTicks(SplashKit.CreateTimer("game_timer"));
                _showReloadPrompt = false;
            }
        }

        public void Reload()
        {
            Ammunition = MaxAmmo;
            _isReloading = false;
            _reloadMessage = string.Empty;
        }

        public void UpdateReloadAnimation()
        {
            if (_isReloading)
            {
                double currentTime = SplashKit.TimerTicks(SplashKit.CreateTimer("game_timer"));
                if (currentTime - _lastReloadTime >= ReloadDuration / 6)
                {
                    _reloadAnimationStep = (_reloadAnimationStep + 1) % 6;
                    _reloadMessage = "Reloading" + new string('.', _reloadAnimationStep);
                    if (currentTime - _lastReloadTime >= ReloadDuration)
                    {
                        Reload();
                    }
                }
            }
        }

        public void DrawReloadMessage()
        {
            if (!string.IsNullOrEmpty(_reloadMessage))
            {
                SplashKit.DrawText(_reloadMessage, Color.Black, GetLocation().X, GetLocation().Y - 100); // Adjusted Y-coordinate
            }
            else if (_showReloadPrompt)
            {
                SplashKit.DrawText("Press 'R' to reload your weapon", Color.Black, GetLocation().X - 100, GetLocation().Y - 100); // Adjusted Y-coordinate
            }
        }

        private Vector2D SubtractVectors(Vector2D v1, Vector2D v2)
        {
            return new Vector2D() { X = v1.X - v2.X, Y = v1.Y - v2.Y }; // Corrected direction calculation
        }

        public void HandleInput()
        {
            if (SplashKit.KeyTyped(KeyCode.RKey))
            {
                StartReload();
            }
        }

        public void DrawHitbox(Color color)
        {
            int spriteWidth = 50; // Assuming player sprite width
            int spriteHeight = 50; // Assuming player sprite height
            base.DrawHitbox(color, spriteWidth, spriteHeight);
        }

        public void SetSpeed(double speed)
        {
            _speed = speed; // Method to set player speed
        }
    }
}
