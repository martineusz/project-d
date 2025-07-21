using System.Collections.Generic;
using UnityEngine;

namespace Procgen
{

    public class TileConnector : MonoBehaviour
    {
        [Header("Tile Entry/Exit Setup")]
        public Direction baseEntryDirection = Direction.South;
        public List<Direction> exitDirections = new();
        public List<Transform> exitPoints = new();
        [Header("Tile Entry/Object Location Setup")]
        public List<Transform> objectLocationPoints = new();
        
        [Header("Tile Metadata")]
        public float weight = 1.0f;
        public TileType tileType = TileType.Closed;

        public Transform GetExitPoint(Direction dir)
        {
            int index = exitDirections.IndexOf(dir);
            if (index != -1) return exitPoints[index];
            return null;
        }
    }
    
    public enum TileType { Open, Corridor, Closed }
}