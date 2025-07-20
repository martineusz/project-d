using System.Collections.Generic;
using UnityEngine;

namespace Procgen
{

    public class TileConnector : MonoBehaviour
    {
        [Header("Tile Entry/Exit Setup")]
        public Direction baseEntryDirection = Direction.South; // Default assumed entry
        public List<Direction> exitDirections = new();
        public List<Transform> exitPoints = new();
        
        [Header("Tile Selection Weight")]
        public float weight = 1.0f;


        public Transform GetExitPoint(Direction dir)
        {
            int index = exitDirections.IndexOf(dir);
            if (index != -1) return exitPoints[index];
            return null;
        }
    }
}