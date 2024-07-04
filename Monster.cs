// File path: Cave_dweller/Monster.cs
using SplashKitSDK;

namespace Cave_dweller
{
    public enum MonsterType { Goblin, Wolf, Spider }
    public enum MovementPattern { Wandering, Chasing }

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
        public abstract void Move(Vector2D direction, double speed);

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            // Additional monster-specific damage handling
        }

        public MonsterType Type { get => type; }
        public MovementPattern Pattern { get => movementPattern; }
    }
}
