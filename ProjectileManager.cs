using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class ProjectileManager
    {

        public static void UpdateProjectiles(Player player, List<Monster> monsters)
        {
            List<Projectile> projectilesToRemove = new List<Projectile>();
            List<Monster> monstersToRemove = new List<Monster>();

            foreach (Projectile projectile in player.Projectiles)
            {
                projectile.Update();
                foreach (Monster monster in monsters)
                {
                    if (SplashKit.RectanglesIntersect(projectile.Hitbox, monster.Hitbox))
                    {
                        monster.TakeDamage(projectile.Damage); // Apply projectile damage to the monster
                        projectilesToRemove.Add(projectile);
                        if (monster.Health <= 0)
                        {
                            player.IncreaseScore(GetScoreForMonster(monster.Type)); // Add points for killing a monster
                            monstersToRemove.Add(monster);
                        }
                        break; // Stop checking other monsters once hit
                    }
                }
            }
            // Remove dead monsters
            foreach (Monster monster in monstersToRemove)
            {
                monsters.Remove(monster);
            }
            // Remove projectiles that hit a monster
            foreach (Projectile projectile in projectilesToRemove)
            {
                player.Projectiles.Remove(projectile);
            }
        }

        private static int GetScoreForMonster(MonsterType type)
        {
            switch (type)
            {
                case MonsterType.Goblin:
                    return 10;
                case MonsterType.Wolf:
                    return 20;
                case MonsterType.Spider:
                    return 30;
                default:
                    return 0;
            }
        }

    }


}