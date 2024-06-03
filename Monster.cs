using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cave_dweller
{
    public enum MonsterType { Goblin, Wolf, Spider }
    public enum MovementPattern { Stationary, Wandering, Chasing }

    public abstract class Monster : Character
    {
        protected MonsterType type;
        protected MovementPattern movementPattern;

        public Monster(int initialHealth, Vector2D startLocation, MonsterType mType, MovementPattern mPattern)
            : base(initialHealth, startLocation)
        {
            type = mType;
            movementPattern = mPattern;
        }

        public abstract void UpdateMovement(Vector2D playerLocation);

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            // Additional monster-specific damage handling
        }

        public MonsterType Type { get => type; }
        public MovementPattern Pattern { get => movementPattern; }
    }

}
