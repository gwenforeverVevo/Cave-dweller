using SplashKitSDK;

namespace Cave_dweller
{
    public class Player : Character
    {
        private int Ammunition { get; set; }
        private Vector2D Facing { get; set; }

        public Player(int initialHealth, int initialAmmo, Vector2D startLocation)
            : base(initialHealth, startLocation)
        {
            Ammunition = initialAmmo;
            Facing = new Vector2D() { X = 1, Y = 0 };
        }

        public override void Move(Vector2D direction)
        {
            Vector2D newLocation = GetLocation();
            newLocation.X += direction.X;
            newLocation.Y += direction.Y;
            Location = newLocation;
        }

        public void ShootProjectile()
        {
            // Implement shooting logic
        }
    }
}
