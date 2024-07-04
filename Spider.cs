using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cave_dweller
{
    public class Spider : Monster, IInventory
    {
        private const double ChaseThreshold = 250.0;
        private const double SpiderSpeed = 0.7;
        private const double WanderSpeed = 0.15;
        private const int WanderMoveDuration = 2000;
        private const int WanderStopDuration = 3000;
        private const int ChaseCooldownDuration = 3000;
        private const int AttackCooldownDuration = 1000;
        private SplashKitSDK.Timer _attackCooldownTimer;
        private Bitmap _smokeBitmap;
        private Vector2D _wanderDirection;
        private SplashKitSDK.Timer _wanderTimer;
        private SplashKitSDK.Timer _chaseCooldownTimer;
        private bool _isWandering;
        private bool _isChasing;
        private string _spiderId;
        private static int spiderCounter = 0;
        private Inventory _inventory;

        public static List<Item> DroppedItems { get; } = new List<Item>();

        public Spider(Vector2D startLocation)
            : base(20, startLocation, MonsterType.Spider, MovementPattern.Wandering)
        {
            _smokeBitmap = SplashKit.LoadBitmap("smoke", "asset\\smoke.gif");
            if (_smokeBitmap == null)
            {
                Console.WriteLine("Error: Could not load smoke.gif!");
                Environment.Exit(1);
            }

            _wanderDirection = GetRandomDirection();
            _wanderTimer = SplashKit.CreateTimer("wander_timer" + spiderCounter);
            _chaseCooldownTimer = SplashKit.CreateTimer("chase_cooldown_timer" + spiderCounter);
            SplashKit.StartTimer(_wanderTimer);
            SplashKit.StartTimer(_chaseCooldownTimer);
            _isWandering = true;
            _isChasing = false;
            _spiderId = "Spider" + (++spiderCounter);
            Console.WriteLine($"{_spiderId} initialized at position: {startLocation.X}, {startLocation.Y}");
            _attackCooldownTimer = SplashKit.CreateTimer("attack_cooldown_timer" + spiderCounter);
            SplashKit.StartTimer(_attackCooldownTimer);
            _inventory = new Inventory();
            _inventory.AddItem(new Item("Spider Heart", "A heart of a spider. Increases your health.", "asset\\spiderHeart.png", player => player?.IncreaseHealth(1)));
        }

        public override void UpdateMovement(Vector2D playerLocation)
        {
            double distanceToPlayer = VectorUtils.DistanceTo(Location, playerLocation);
            if (distanceToPlayer < ChaseThreshold)
            {
                if (!_isChasing)
                {
                    PrintState("chasing");
                    StartChasing();
                }
                ResetChaseCooldownTimer();
            }
            else if (_isChasing && IsChaseCooldownElapsed())
            {
                PrintState("wandering");
                StopChasing();
            }

            if (_isChasing)
            {
                ChasePlayer(playerLocation);
            }
            else
            {
                HandleWandering();
            }
        }

        private void StartChasing()
        {
            _isChasing = true;
            _isWandering = false;
            movementPattern = MovementPattern.Chasing;
        }

        private void StopChasing()
        {
            _isChasing = false;
            movementPattern = MovementPattern.Wandering;
            ResetWanderTimer();
        }

        private void ChasePlayer(Vector2D playerLocation)
        {
            Vector2D direction = VectorUtils.SubtractVectors(playerLocation, Location);
            direction = SplashKit.UnitVector(direction);
            Move(direction, SpiderSpeed);
        }

        private void HandleWandering()
        {
            if (_isWandering && IsWanderMoveDurationElapsed())
            {
                PrintState("stopping");
                StopWandering();
            }
            else if (!_isWandering && IsWanderStopDurationElapsed())
            {
                PrintState("moving");
                StartWandering();
            }

            if (_isWandering)
            {
                Move(_wanderDirection, WanderSpeed);
            }
        }

        private void StartWandering()
        {
            _isWandering = true;
            _wanderDirection = GetRandomDirection();
            ResetWanderTimer();
        }

        private void StopWandering()
        {
            _isWandering = false;
            ResetWanderTimer();
        }

        private Vector2D GetRandomDirection()
        {
            Random random = new Random();
            double angle = random.NextDouble() * 2 * Math.PI;
            return new Vector2D() { X = Math.Cos(angle), Y = Math.Sin(angle) };
        }

        private void ResetWanderTimer()
        {
            SplashKit.ResetTimer(_wanderTimer);
        }

        private void ResetChaseCooldownTimer()
        {
            SplashKit.ResetTimer(_chaseCooldownTimer);
        }

        private bool IsWanderMoveDurationElapsed()
        {
            return SplashKit.TimerTicks(_wanderTimer) > WanderMoveDuration;
        }

        private bool IsWanderStopDurationElapsed()
        {
            return SplashKit.TimerTicks(_wanderTimer) > WanderStopDuration;
        }

        private bool IsChaseCooldownElapsed()
        {
            return SplashKit.TimerTicks(_chaseCooldownTimer) > ChaseCooldownDuration;
        }

        public override void Move(Vector2D direction)
        {
            Move(direction, SpiderSpeed);
        }

        public override void Move(Vector2D direction, double speed)
        {
            Vector2D newLocation = Location;
            newLocation.X += direction.X * speed;
            newLocation.Y += direction.Y * speed;
            SetLocation(newLocation);
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            Console.WriteLine($"{_spiderId} is hit for {amount} damage!");
            if (_health <= 0)
            {
                Console.WriteLine($"{_spiderId} has died.");
                SplashKit.DrawBitmap(_smokeBitmap, (float)Location.X, (float)Location.Y);
                List<Item> itemsToDrop = new List<Item>(_inventory.GetItems());
                foreach (Item item in itemsToDrop)
                {
                    DropItem(item, Location);
                }
            }
        }

        public override void AttackPlayer(Player player)
        {
            if (SplashKit.RectanglesIntersect(this.Hitbox, player.Hitbox) && IsAttackCooldownElapsed())
            {
                player.TakeDamage(1);
                ResetAttackCooldownTimer();
            }
        }

        private void ResetAttackCooldownTimer()
        {
            SplashKit.ResetTimer(_attackCooldownTimer);
        }

        private bool IsAttackCooldownElapsed()
        {
            return SplashKit.TimerTicks(_attackCooldownTimer) > AttackCooldownDuration;
        }

        public override Rectangle Hitbox => SplashKit.RectangleFrom(Location.X - 5, Location.Y - 5, 60, 60);

        private void PrintState(string state)
        {
            Console.WriteLine($"{_spiderId} is {state} at position: {Location.X}, {Location.Y}");
        }

        public void Update()
        {
            UpdateFlash();
        }

        // Inventory Methods
        public void AddItem(Item item) => _inventory.AddItem(item);
        public void RemoveItem(Item item) => _inventory.RemoveItem(item);
        public bool ContainsItem(Item item) => _inventory.ContainsItem(item);
        public void UseItem(Item item) => _inventory.UseItem(item, null); // Spider doesn't use items, passing null for player
        public void DropItem(Item item, Vector2D position)
        {
            _inventory.RemoveItem(item);
            item.Position = position;
            DroppedItems.Add(item);
            Console.WriteLine($"Dropped item '{item.Name}' at location ({position.X}, {position.Y}).");
        }
        public List<Item> GetItems() => _inventory.GetItems();
    }
}
