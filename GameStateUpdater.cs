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
                goblin.UpdateMovement(playerLocation);
            }
        }
    }
}
