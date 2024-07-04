// File path: Cave_dweller/ProjectileManager.cs
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class ProjectileManager
    {
        public static void UpdateProjectiles(Player player, List<Goblin> goblins)
        {
            List<Projectile> projectilesToRemove = new List<Projectile>();

            foreach (Projectile projectile in player.Projectiles)
            {
                projectile.Update();
                // Check for collisions with goblins
                List<Goblin> goblinsToRemove = new List<Goblin>();
                List<Wolf> wolfsToRemove = new List<Wolf>();
                foreach (Goblin goblin in goblins)
                {
                    if (SplashKit.RectanglesIntersect(projectile.Hitbox, goblin.Hitbox))
                    {
                        goblin.TakeDamage(projectile.Damage); // Apply projectile damage to the goblin
                        projectilesToRemove.Add(projectile);
                        if (goblin.Health <= 0)
                        {
                            goblinsToRemove.Add(goblin);
                        }
                        break; // Stop checking other goblins once hit
                    }
                }
                // Remove dead goblins
                foreach (Goblin goblin in goblinsToRemove)
                {
                    goblins.Remove(goblin);
                }
            }
            // Remove projectiles that hit a goblin
            foreach (Projectile projectile in projectilesToRemove)
            {
                player.Projectiles.Remove(projectile);
            }
        }
    }
}
