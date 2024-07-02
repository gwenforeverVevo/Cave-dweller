// File path: Cave_dweller/Goblin.cs
using System;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Goblin : Monster
    {
        private const double ChaseThreshold = 250.0;
        private const double GoblinSpeed = 0.5;
        private const double WanderSpeed = 0.15;
        private const int WanderMoveDuration = 2000;
        private const int WanderStopDuration = 3000;
        private const int ChaseCooldownDuration = 3000;

        private Bitmap _smokeBitmap;
        private Vector2D _wanderDirection;
        private SplashKitSDK.Timer _wanderTimer;
        private SplashKitSDK.Timer _chaseCooldownTimer;
        private bool _isWandering;
        private bool _isChasing;
        private string _goblinId;
        private static int goblinCounter = 0;

        public Goblin(Vector2D startLocation)
            : base(10, startLocation, MonsterType.Goblin, MovementPattern.Wandering)
        {
            _smokeBitmap = SplashKit.LoadBitmap("smoke", "asset\\smoke.gif");
            if (_smokeBitmap == null)
            {
                Console.WriteLine("Error: Could not load smoke.gif!");
                Environment.Exit(1);
            }

            _wanderDirection = GetRandomDirection();
            _wanderTimer = SplashKit.CreateTimer("wander_timer" + goblinCounter);
            _chaseCooldownTimer = SplashKit.CreateTimer("chase_cooldown_timer" + goblinCounter);
            SplashKit.StartTimer(_wanderTimer);
            SplashKit.StartTimer(_chaseCooldownTimer);
            _isWandering = true;
            _isChasing = false;
            _goblinId = "Goblin" + (++goblinCounter);
            Console.WriteLine($"{_goblinId} initialized at position: {startLocation.X}, {startLocation.Y}");
        }

        public override void UpdateMovement(Vector2D playerLocation)
        {
            double distanceToPlayer = DistanceTo(playerLocation);

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
            Vector2D direction = SubtractVectors(playerLocation, Location);
            direction = SplashKit.UnitVector(direction);
            Move(direction, GoblinSpeed);
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
            Move(direction, GoblinSpeed);
        }

        public override void Move(Vector2D direction, double speed)
        {
            Vector2D newLocation = Location;
            newLocation.X += direction.X * speed;
            newLocation.Y += direction.Y * speed;
            SetLocation(newLocation);
        }

        private double DistanceTo(Vector2D other)
        {
            double dx = Location.X - other.X;
            double dy = Location.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private Vector2D SubtractVectors(Vector2D v1, Vector2D v2)
        {
            return new Vector2D() { X = v1.X - v2.X, Y = v1.Y - v2.Y };
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            Console.WriteLine($"{_goblinId} is hit!");
            if (_health <= 0)
            {
                Console.WriteLine($"{_goblinId} has died.");
                SplashKit.DrawBitmap(_smokeBitmap, (float)Location.X, (float)Location.Y);
            }
        }

        public Rectangle Hitbox => SplashKit.RectangleFrom(Location.X - 5, Location.Y - 5, 60, 60);

        private void PrintState(string state)
        {
            Console.WriteLine($"{_goblinId} is {state} at position: {Location.X}, {Location.Y}");
        }
    }
}
