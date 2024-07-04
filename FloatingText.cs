using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cave_dweller
{
    public class FloatingText
    {
        private string _text;
        private Color _color;
        private Vector2D _position;
        private int _lifetime;
        private double _startTime;
        private int _floatSpeed;
        private int _fadeOutTime;
        private double _lastUpdateTime;

        public FloatingText(string text, Color color, Vector2D position, int lifetime = 2000, int floatSpeed = 1, int fadeOutTime = 500)
        {
            _text = text;
            _color = color;
            _position = position;
            _lifetime = lifetime;
            _startTime = SplashKit.CurrentTicks();
            _lastUpdateTime = _startTime;
            _floatSpeed = floatSpeed;
            _fadeOutTime = fadeOutTime;
        }

        public bool IsExpired => SplashKit.CurrentTicks() > _startTime + _lifetime;

        public void Update()
        {
            double currentTime = SplashKit.CurrentTicks();
            double deltaTime = currentTime - _lastUpdateTime;
            _lastUpdateTime = currentTime;

            // Move the text up
            _position.Y -= _floatSpeed * (deltaTime / 1000.0);
        }

        public void Draw()
        {
            double elapsed = SplashKit.CurrentTicks() - _startTime;
            int alpha = 255;

            if (elapsed > _lifetime - _fadeOutTime)
            {
                alpha = (int)(255 * ((_lifetime - elapsed) / (double)_fadeOutTime));
            }

            Color textColor = Color.RGBAColor(_color.R, _color.G, _color.B, alpha);
            SplashKit.DrawText(_text, textColor, (float)_position.X, (float)_position.Y);
        }
    }
}
