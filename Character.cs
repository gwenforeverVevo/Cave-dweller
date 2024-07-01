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
            double x = Location.X - (spriteWidth / 2);
            double y = Location.Y - (spriteHeight / 2);
            double hitboxWidth = 50;
            double hitboxHeight = 50;
            double hitboxX = Location.X - (hitboxWidth / 2);
            double hitboxY = Location.Y - (hitboxHeight / 2);
            SplashKit.FillRectangle(color, hitboxX, hitboxY, hitboxWidth, hitboxHeight);
        }
    }
}
