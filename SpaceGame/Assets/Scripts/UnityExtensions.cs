using UnityEngine;

namespace SpaceGame
{
	public static class UnityExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask & (1 << layer)) > 0;
        }
    }
}
