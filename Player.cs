// File path: Cave_dweller/Player.cs
using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Player : Character, IInventory
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
        private int _damage;
        private static SoundEffect _hitSound;
        private Inventory _inventory;

        static Player()
        {
            _hitSound = SplashKit.LoadSoundEffect("hit_sound", "asset/hit.wav"); // Load hit sound
        }

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
            _inventory = new Inventory();
            _damage = 10; // Initial damage
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
                IsDead = true;
            }
            SplashKit.PlaySoundEffect(_hitSound); // Play hit sound
        }

        public bool IsDead { get; private set; } = false;

        public void ShootProjectile(Vector2D target, int spriteWidth, int spriteHeight)
        {
            if (_isReloading) return;
            if (Ammunition > 0)
            {
                Vector2D startPosition = GetLocation();
                startPosition.X += 25; // Adjusted for hitbox center
                startPosition.Y += 25; // Adjusted for hitbox center
                Vector2D direction = VectorUtils.SubtractVectors(target, startPosition);
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

        public void IncreaseDamage(int amount)
        {
            _damage += amount;
            Console.WriteLine($"Player damage increased to {_damage}.");
        }

        public Rectangle Hitbox => SplashKit.RectangleFrom(GetLocation().X - 5, GetLocation().Y - 5, 60, 60);

        public void Update()
        {
            UpdateFlash();
        }

        // Inventory Methods
        public void AddItem(Item item) => _inventory.AddItem(item);
        public void RemoveItem(Item item) => _inventory.RemoveItem(item);
        public bool ContainsItem(Item item) => _inventory.ContainsItem(item);
        public void UseItem(Item item) => _inventory.UseItem(item, this);
        public void DropItem(Item item, Vector2D position) => _inventory.DropItem(item, position);
        public List<Item> GetItems() => _inventory.GetItems();

        public void UseItemByName(string itemName)
        {
            Item? item = _inventory.Fetch(itemName);
            if (item != null)
            {
                UseItem(item);
            }
            else
            {
                Console.WriteLine($"Item '{itemName}' not found in inventory.");
            }
        }

        public void PickUpItem(Item item)
        {
            AddItem(item);
            Console.WriteLine($"Picked up item: {item.Name}");
        }
    }
}
