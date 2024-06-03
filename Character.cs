using SplashKitSDK;

namespace Cave_dweller
{
    public abstract class Character
    {
        protected int Health { get; set; }
        protected Vector2D Location { get; set; }

        public Character(int initialHealth, Vector2D startLocation)
        {
            Health = initialHealth;
            Location = startLocation;
        }

        public abstract void Move(Vector2D direction);

        public virtual void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }

        // Public getter for Location
        public Vector2D GetLocation()
        {
            return Location;
        }
    }
}
