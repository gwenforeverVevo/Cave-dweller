﻿using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class ProjectileManager
    {
        public static void UpdateProjectiles(Player player, List<Goblin> goblins, List<Wolf> wolfs, List<Spider> spiders)
        {
            List<Projectile> projectilesToRemove = new List<Projectile>();

            foreach (Projectile projectile in player.Projectiles)
            {
                projectile.Update();
                // Check for collisions with goblins
                List<Goblin> goblinsToRemove = new List<Goblin>();
                foreach (Goblin goblin in goblins)
                {
                    if (SplashKit.RectanglesIntersect(projectile.Hitbox, goblin.Hitbox))
                    {
                        goblin.TakeDamage(projectile.Damage); // Apply projectile damage to the goblin
                        projectilesToRemove.Add(projectile);
                        if (goblin.Health <= 0)
                        {
                            player.IncreaseScore(10); // Add points for killing a goblin
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

                // Check for collisions with wolfs
                List<Wolf> wolfsToRemove = new List<Wolf>();
                foreach (Wolf wolf in wolfs)
                {
                    if (SplashKit.RectanglesIntersect(projectile.Hitbox, wolf.Hitbox))
                    {
                        wolf.TakeDamage(projectile.Damage); // Apply projectile damage to the wolf
                        projectilesToRemove.Add(projectile);
                        if (wolf.Health <= 0)
                        {
                            player.IncreaseScore(20); // Add points for killing a wolf
                            wolfsToRemove.Add(wolf);
                        }
                        break; // Stop checking other wolfs once hit
                    }
                }
                // Remove dead wolfs
                foreach (Wolf wolf in wolfsToRemove)
                {
                    wolfs.Remove(wolf);
                }

                // Check for collisions with spiders
                List<Spider> spidersToRemove = new List<Spider>();
                foreach (Spider spider in spiders)
                {
                    if (SplashKit.RectanglesIntersect(projectile.Hitbox, spider.Hitbox))
                    {
                        spider.TakeDamage(projectile.Damage); // Apply projectile damage to the spider
                        projectilesToRemove.Add(projectile);
                        if (spider.Health <= 0)
                        {
                            player.IncreaseScore(30); // Add points for killing a spider
                            spidersToRemove.Add(spider);
                        }
                        break; // Stop checking other spiders once hit
                    }
                }
                // Remove dead spiders
                foreach (Spider spider in spidersToRemove)
                {
                    spiders.Remove(spider);
                }
            }
            // Remove projectiles that hit a goblin or wolf or spider
            foreach (Projectile projectile in projectilesToRemove)
            {
                player.Projectiles.Remove(projectile);
            }
        }
    }
}