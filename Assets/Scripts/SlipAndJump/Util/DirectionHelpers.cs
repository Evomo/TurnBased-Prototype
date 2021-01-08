using UnityEngine;

namespace SlipAndJump.Util {
    public enum MapDirections {
        North,
        East,
        South,
        West,
    }

    public static class DirectionHelpers {

        public static MapDirections GetNeighbor(this MapDirections current, bool ccw) {
            int next = ((int) current + (ccw ? -1 : +1) + 4) % 4;
            return (MapDirections) (next);
        }

        public static Vector2Int DirectionsDelta(this MapDirections direction) {
            Vector2Int toReturn = Vector2Int.zero;
            switch (direction) {
                case MapDirections.North:
                    toReturn = Vector2Int.up;
                    break;
                case MapDirections.East:
                    toReturn = Vector2Int.right;
                    break;
                case MapDirections.South:
                    toReturn = Vector2Int.down;

                    break;
                case MapDirections.West:
                    toReturn = Vector2Int.left;
                    break;
            }

            return toReturn;
        }

        public static MapDirections Inverse(this MapDirections current) {
            return (MapDirections) (((int) current + 2 + 4) % 4);
        }
    }
}