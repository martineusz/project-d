using UnityEngine;

namespace Procgen
{
    public enum Direction { North = 0, East = 1, South = 2, West = 3 }

    public static class DirectionUtils
    {
        public static Direction GetOpposite(Direction dir)
        {
            return (Direction)(((int)dir + 2) % 4);
        }

        public static Quaternion RotationTo(Direction from, Direction to)
        {
            int delta = ((int)from - (int)to + 4) % 4;
            return Quaternion.Euler(0, 0, delta * 90);
        }
    }
}