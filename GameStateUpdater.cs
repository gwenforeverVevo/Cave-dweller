// File path: Cave_dweller/GameStateUpdater.cs
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class GameStateUpdater
    {
        public static void UpdateGameState(Player player, List<Goblin> goblins)
        {
            Vector2D playerLocation = player.GetLocation();
            foreach (Goblin goblin in goblins)
            {
                if (goblin.Health > 0) // Only update movement if the goblin is alive
                {
                    goblin.UpdateMovement(playerLocation);
                }
            }
        }
    }
}
