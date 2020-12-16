using System;
using System.Collections.Generic;
using UnityEngine;

public enum MapDirections {
    NORTH,
    EAST,
    SOUTH,
    WEST,
}

public static class DirectionHelpers {
    public static IEnumerable<(MapDirections name, Vector3 vector)> DirectionsIterator() {
        yield return (MapDirections.NORTH, Vector3.forward);
        yield return (MapDirections.EAST, Vector3.right);
        yield return (MapDirections.SOUTH, -Vector3.forward);
        yield return (MapDirections.WEST, Vector3.left);
    }

    public static MapDirections GetNeighbor(MapDirections current, bool ccw) {
        int next = (((int) current + (ccw ? -1 : +1)) + 4) % 4;
        return (MapDirections) (next);
    }

    public static Vector2Int DirectionsDelta(MapDirections direction) {
        Vector2Int toReturn = Vector2Int.zero;
        switch (direction) {
            case MapDirections.NORTH:
                toReturn = Vector2Int.up;
                break;
            case MapDirections.EAST:
                toReturn = Vector2Int.right;
                break;
            case MapDirections.SOUTH:
                toReturn = Vector2Int.down;

                break;
            case MapDirections.WEST:
                toReturn = Vector2Int.left;
                break;
        }

        return toReturn;
    }

    public static MapDirections Inverse(MapDirections current) {
        return (MapDirections) (((int) current + 2 + 4) % 4);
    }
}