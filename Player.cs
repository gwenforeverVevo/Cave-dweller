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
        private static SoundEffect _hitSound;
        private Inventory _inventory;
        private bool _isFacingRight;
        public bool IsFacingRight => _isFacingRight;
        public int Damage { get; private set; }
        public double Speed { get; private set; }
        public int Score { get; private set; }

        static Player()
        {
            _hitSound = SplashKit.LoadSoundEffect("hit_sound", "asset/hit.wav");
        }

        public Player(int initialHealth, int initialAmmo, Vector2D startLocation, double speed = 1)
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
            Speed = speed;
            _inventory = new Inventory();
            Damage = 10;
            _isFacingRight = true;
            Score = 0;
        }

        public void IncreaseScore(int amount)
        {
            Score += amount;
            Console.WriteLine($"Player score increased by {amount}. New score: {Score}");
        }

        public override void Move(Vector2D direction)
        {
            Vector2D newLocation = GetLocation();
            newLocation.X += direction.X * Speed;
            newLocation.Y += direction.Y * Speed;
            SetLocation(newLocation);
            Facing = direction;

            if (direction.X > 0)
            {
                _isFacingRight = true;
            }
            else if (direction.X < 0)
            {
                _isFacingRight = false;
            }
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            if (_health <= 0)
            {
                Console.WriteLine("Player has died.");
                IsDead = true;
            }
            SplashKit.PlaySoundEffect(_hitSound);
        }

        public bool IsDead { get; private set; } = false;

        public void ShootProjectile(Vector2D target, int spriteWidth, int spriteHeight)
        {
            if (_isReloading) return;
            if (Ammunition > 0)
            {
                Vector2D startPosition = GetLocation();
                startPosition.X += 25;
                startPosition.Y += 25;
                Vector2D direction = VectorUtils.SubtractVectors(target, startPosition);
                direction = SplashKit.UnitVector(direction);
                Projectile newProjectile = new Projectile(startPosition, direction, Damage);
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
                SplashKit.DrawText(_reloadMessage, Color.Black, GetLocation().X, GetLocation().Y - 100);
            }
            else if (_showReloadPrompt)
            {
                SplashKit.DrawText("Press 'R' to reload your weapon", Color.Black, GetLocation().X - 100, GetLocation().Y - 100);
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
            int spriteWidth = 50;
            int spriteHeight = 50;
            base.DrawHitbox(color, spriteWidth, spriteHeight);
        }

        public void SetSpeed(double speed)
        {
            Speed = speed;
        }

        public void IncreaseSpeed(double amount)
        {
            Speed += amount;
            Console.WriteLine($"Player speed increased by {amount}. New speed: {Speed}");
        }

        public void IncreaseDamage(int amount)
        {
            Damage += amount;
            Console.WriteLine($"Player damage increased to {Damage}.");
        }

        public void IncreaseHealth(int amount)
        {
            _health += amount;
            Console.WriteLine($"Player health increased by {amount}. New health: {_health}");
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
            item.ApplyEffect(this);
        }
    }
}