using SpaceGame.Pooling;

namespace SpaceGame.Weapons.Targeting
{
    // A simple component that marks the object as poolable and connects it to the GameObject it is leading.
	public class TargetLead : PoolableObject
    {
        public Target Target { get; set; }
    }
}
