// File path: Cave_dweller/Camera.cs
using SplashKitSDK;

namespace Cave_dweller
{
    public class Camera
    {
        public Vector2D Position { get; private set; }

        public Camera(Vector2D startPosition)
        {
            Position = startPosition;
        }

        public void UpdatePosition(Vector2D playerPosition, double screenWidth, double screenHeight)
        {
            double newX = playerPosition.X - screenWidth / 2;
            double newY = playerPosition.Y - screenHeight / 2;

            // Clamp camera position to the game world boundaries
            newX = Math.Max(0, newX);
            newY = Math.Max(0, newY);
            newX = Math.Min(GameConstants.GameWidth - screenWidth, newX);
            newY = Math.Min(GameConstants.GameHeight - screenHeight, newX);

            Position = new Vector2D() { X = newX, Y = newY };
        }
    }
}
