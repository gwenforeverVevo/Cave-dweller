// File path: Cave_dweller/Character.cs
using SplashKitSDK;

namespace Cave_dweller
{
    public abstract class Character
    {
        protected int _health;
        protected Vector2D Location { get; private set; }

        protected Character(int initialHealth, Vector2D startLocation)
        {
            _health = initialHealth;
            Location = startLocation;
        }

        public abstract void Move(Vector2D direction);

        public virtual void TakeDamage(int amount)
        {
            _health -= amount;
        }

        public Vector2D GetLocation()
        {
            return Location;
        }

        public void SetLocation(Vector2D newLocation)
        {
            Location = newLocation;
        }

        public int Health => _health;

        public void DrawHitbox(Color color, int spriteWidth, int spriteHeight)
        {
            double x = Location.X + spriteWidth / 2 - 25; // Adjusted for hitbox size
            double y = Location.Y + spriteHeight / 2 - 25; // Adjusted for hitbox size
            SplashKit.FillRectangle(color, x, y, 50, 50); // 50x50 hitbox
        }
    }
}
