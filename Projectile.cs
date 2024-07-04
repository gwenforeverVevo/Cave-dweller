using SplashKitSDK;

namespace Cave_dweller
{
    public class Projectile
    {
        private Vector2D _position;
        private Vector2D _velocity;
        private Bitmap _bitmap;
        public int Damage { get; private set; }
        private double _angle;

        public Projectile(Vector2D startPosition, Vector2D direction, int playerDamage)
        {
            _position = startPosition;
            _velocity = SplashKit.UnitVector(direction);
            _bitmap = SplashKit.LoadBitmap("projectile", "asset\\projectile.png");
            if (_bitmap == null)
            {
                Console.WriteLine("Error: Could not load projectile.png!");
                Environment.Exit(1);
            }
            Damage = playerDamage; // Set the projectile's damage to the player's damage
            _angle = SplashKit.VectorAngle(_velocity);
        }


        public void Update()
        {
            _position.X += _velocity.X * 5; // Speed multiplier
            _position.Y += _velocity.Y * 5;
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_bitmap, (float)_position.X, (float)_position.Y, SplashKit.OptionRotateBmp((float)_angle));
            
        }

       

        public Rectangle Hitbox => SplashKit.RectangleFrom(_position.X - 5, _position.Y - 5, _bitmap.Width + 10, _bitmap.Height + 10);
        public Vector2D Position => _position;
        public void DrawHitbox()
        {
            Rectangle hitbox = Hitbox;
            SplashKit.FillRectangle(Color.Purple, hitbox);
        }
    }
}
