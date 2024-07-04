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
                if (goblin.Health > 0) 
                {
                    goblin.UpdateMovement(playerLocation);
                }
            }
        }

        public static void UpdateGameState(Player player, List<Wolf> wolfs)
        {
            Vector2D playerLocation = player.GetLocation();
            foreach (Wolf wolf in wolfs)
            {
                if (wolf.Health > 0) // Only update movement if the goblin is alive
                {
                    wolf.UpdateMovement(playerLocation);
                }
            }
        }

        public static void UpdateGameState(Player player, List<Spider> spiders)
        {
            Vector2D playerLocation = player.GetLocation();
            foreach (Spider spider in spiders)
            {
                if (spider.Health > 0) // Only update movement if the goblin is alive
                {
                    spider.UpdateMovement(playerLocation);
                }
            }
        }
    }
}
