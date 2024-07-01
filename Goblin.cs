using SplashKitSDK;

namespace Cave_dweller
{
    public class Goblin : Monster
    {
        private const double ChaseThreshold = 100.0; // Threshold distance to start chasing
        private Bitmap _smokeBitmap;

        public Goblin(Vector2D startLocation)
            : base(10, startLocation, MonsterType.Goblin, MovementPattern.Chasing) // Initial health set to 10
        {
            _smokeBitmap = SplashKit.LoadBitmap("smoke", "asset\\smoke.gif");
            if (_smokeBitmap == null)
            {
                Console.WriteLine("Error: Could not load smoke.gif!");
                Environment.Exit(1);
            }
        }

        public override void UpdateMovement(Vector2D playerLocation)
        {
            double distanceToPlayer = DistanceTo(playerLocation);
            if (distanceToPlayer < ChaseThreshold)
            {
                Vector2D direction = SubtractVectors(playerLocation, Location);
                direction = SplashKit.UnitVector(direction);
                Move(direction);
            }
        }

        public override void Move(Vector2D direction)
        {
            Vector2D newLocation = Location;
            newLocation.X += direction.X;
            newLocation.Y += direction.Y;
            SetLocation(newLocation);
        }

        public void DrawHitbox(int spriteWidth, int spriteHeight)
        {
            // Hitbox matching the sprite's dimensions and position
            Rectangle hitbox = SplashKit.RectangleFrom(Location.X, Location.Y, spriteWidth, spriteHeight);
            SplashKit.FillRectangle(Color.Purple, hitbox);
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
            Console.WriteLine("Goblin is hit!");
            if (_health <= 0)
            {
                Console.WriteLine("Goblin has died.");
                SplashKit.DrawBitmap(_smokeBitmap, (float)Location.X, (float)Location.Y);
            }
        }

        public Rectangle Hitbox => SplashKit.RectangleFrom(Location.X - 5, Location.Y - 5, 60, 60); // Slightly larger hitbox
    }
}
