using System.Collections.Generic;
using UnityEngine;

namespace Procgen
{
    public class DungeonGenerator : MonoBehaviour
    {
        public GameObject startTile;
        public List<GameObject> tilePrefabs;
        public GameObject fullTilePrefab;
        public int maxTiles = 50;
        public float deadEndChance = 0.1f;
        public float tileSize = 10f;

        private HashSet<Vector2Int> occupied = new();
        private Vector2Int currentPos = Vector2Int.zero;
        private Direction lastExit = Direction.North;
        private int tileCount = 0;

        void Start()
        {
            Generate();
        }

        void Generate()
        {
            Debug.Log("Starting dungeon generation...");

            // Keep track of occupied grid cells
            occupied.Clear();
            tileCount = 0;

            // Place start tile at origin
            GameObject start = Instantiate(startTile, Vector3.zero, Quaternion.identity);
            TileConnector startConn = start.GetComponent<TileConnector>();
            Vector2Int startGridPos = Vector2Int.zero;
            occupied.Add(startGridPos);
            tileCount++;

            // Queue of tiles to spawn: (grid position to spawn at, direction tile enters from)
            Queue<(Vector2Int gridPos, Direction enterFrom)> spawnQueue = new();

            // Enqueue all exits from the start tile to begin spawning
            foreach (var exitDir in startConn.exitDirections)
            {
                Vector2Int nextPos = startGridPos + DirectionToVector2Int(exitDir);
                spawnQueue.Enqueue((nextPos, DirectionUtils.GetOpposite(exitDir)));
            }

            // Process the spawn queue until we hit max tiles or no more to spawn
            while (spawnQueue.Count > 0 && tileCount < maxTiles)
            {
                var (gridPos, enterFrom) = spawnQueue.Dequeue();

                if (occupied.Contains(gridPos))
                {
                    // Already a tile here
                    continue;
                }

                if (Random.value < deadEndChance)
                {
                    // Random chance to create a dead end
                    continue;
                }

                // Pick a random tile prefab
                GameObject prefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
                TileConnector prefabConn = prefab.GetComponent<TileConnector>();

                // Calculate rotation needed to align prefab's baseEntryDirection with enterFrom
                Quaternion rotation = DirectionUtils.RotationTo(prefabConn.baseEntryDirection, enterFrom);

                // Calculate how many 90° clockwise steps the rotation is
                int rotationSteps = ((int)enterFrom - (int)prefabConn.baseEntryDirection + 4) % 4;

                // Calculate world position of the tile based on grid position and tile size
                Vector3 worldPos = new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, 0);

                Instantiate(prefab, worldPos, rotation);
                
                tileCount++;
                occupied.Add(gridPos);

                // For each exit in the prefab, calculate rotated exit direction and enqueue next tiles
                for (int i = 0; i < prefabConn.exitDirections.Count; i++)
                {
                    Direction originalExit = prefabConn.exitDirections[i];
                    Direction rotatedExit = RotateDirection(originalExit, rotationSteps);

                    // Don't go back through the entrance
                    if (rotatedExit == enterFrom) continue;

                    Vector2Int nextGridPos = gridPos + DirectionToVector2Int(rotatedExit);
                    if (occupied.Contains(nextGridPos)) continue;

                    spawnQueue.Enqueue((nextGridPos, DirectionUtils.GetOpposite(rotatedExit)));
                }
            }

            Debug.Log($"Dungeon generation complete. Tiles placed: {tileCount}");
            
            FillEmptySpacesWithFullTiles();
            
            Debug.Log($"Added closing tiles.");
        }

// Helper method to rotate directions by 90° steps clockwise
        Direction RotateDirection(Direction dir, int steps)
        {
            return (Direction)(((int)dir + steps) % 4);
        }

// Convert Direction enum to Vector2Int offsets
        Vector2Int DirectionToVector2Int(Direction dir)
        {
            return dir switch
            {
                Direction.North => new Vector2Int(0, 1),
                Direction.East => new Vector2Int(1, 0),
                Direction.South => new Vector2Int(0, -1),
                Direction.West => new Vector2Int(-1, 0),
                _ => Vector2Int.zero
            };
        }


        Vector2Int WorldToGrid(Vector3 worldPos)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldPos.x / tileSize),
                Mathf.RoundToInt(worldPos.y / tileSize)
            );
        }
        
        
        void FillEmptySpacesWithFullTiles()
        {
            HashSet<Vector2Int> positionsToFill = new();

            // For every occupied tile
            foreach (var pos in occupied)
            {
                // Check neighbors in all 4 directions
                foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
                {
                    Vector2Int neighbor = pos + DirectionToVector2Int(dir);

                    if (!occupied.Contains(neighbor))
                    {
                        positionsToFill.Add(neighbor);
                    }
                }
            }

            foreach (var pos in positionsToFill)
            {
                Vector3 worldPos = new Vector3(pos.x * tileSize, pos.y * tileSize, 0);
                Instantiate(fullTilePrefab, worldPos, Quaternion.identity);
            }
        }
    }
}