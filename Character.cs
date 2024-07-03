// File path: Cave_dweller/Character.cs
using SplashKitSDK;

namespace Cave_dweller
{
    public abstract class Character
    {
        protected int _health;
        protected Vector2D Location { get; private set; }
        private bool _isFlashing;
        private double _flashEndTime;
        private Color _originalColor;
        public Color CurrentColor { get; private set; }
        private static SoundEffect _hitSound;

        static Character()
        {
            _hitSound = SplashKit.LoadSoundEffect("hit_sound", "asset/hit.mp3"); // Load hit sound
        }

        protected Character(int initialHealth, Vector2D startLocation)
        {
            _health = initialHealth;
            Location = startLocation;
            _isFlashing = false;
            _originalColor = Color.White;
            CurrentColor = _originalColor;
        }

        public abstract void Move(Vector2D direction);

        public virtual void TakeDamage(int amount)
        {
            _health -= amount;
            StartFlash(Color.Red, 1000); // Flash red for 1 second
            SplashKit.PlaySoundEffect(_hitSound); // Play hit sound
        }

        public Vector2D GetLocation() => Location;

        public void SetLocation(Vector2D newLocation) => Location = newLocation;

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

        protected void StartFlash(Color flashColor, int duration)
        {
            _isFlashing = true;
            _flashEndTime = SplashKit.CurrentTicks() + duration;
            CurrentColor = flashColor;
        }

        protected void UpdateFlash()
        {
            if (_isFlashing && SplashKit.CurrentTicks() > _flashEndTime)
            {
                _isFlashing = false;
                CurrentColor = _originalColor;
            }
        }
    }
}
