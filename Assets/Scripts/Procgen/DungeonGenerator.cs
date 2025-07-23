using System.Collections.Generic;
using UnityEngine;

namespace Procgen
{
    public class DungeonGenerator : MonoBehaviour
    {
        [Header("Tile Prefabs")] public GameObject startTile;
        public List<GameObject> tilePrefabs;
        public GameObject fullTilePrefab;

        [Header("Elements Prefabs")] public List<GameObject> floorElementPrefabs;
        public float floorElementChance = 0.1f;
        public List<GameObject> airElementPrefabs;
        public float airElementChance = 0.1f;
        [Header("Elements Prefabs/Objects")] public List<GameObject> objectElementPrefabs;
        public float objectElementChance = 0.1f;

        [Header("Generation Settings")] public int maxTiles = 50;
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

            occupied.Clear();
            tileCount = 0;

            GameObject start = Instantiate(startTile, Vector3.zero, Quaternion.identity);
            TileConnector startConn = start.GetComponent<TileConnector>();
            Vector2Int startGridPos = Vector2Int.zero;
            occupied.Add(startGridPos);
            tileCount++;

            Queue<(Vector2Int gridPos, Direction enterFrom)> spawnQueue = new();

            foreach (var exitDir in startConn.exitDirections)
            {
                Vector2Int nextPos = startGridPos + DirectionToVector2Int(exitDir);
                spawnQueue.Enqueue((nextPos, DirectionUtils.GetOpposite(exitDir)));
            }

            while (spawnQueue.Count > 0 && tileCount < maxTiles)
            {
                var (gridPos, enterFrom) = spawnQueue.Dequeue();

                if (occupied.Contains(gridPos))
                {
                    continue;
                }

                if (Random.value < deadEndChance)
                {
                    continue;
                }

                GameObject prefab = GetRandomWeightedTile();
                TileConnector prefabConn = prefab.GetComponent<TileConnector>();

                Quaternion rotation = DirectionUtils.RotationTo(prefabConn.baseEntryDirection, enterFrom);

                int rotationSteps = ((int)enterFrom - (int)prefabConn.baseEntryDirection + 4) % 4;

                Vector3 worldPos = new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, 0);


                GameObject newObj = Instantiate(prefab, worldPos, rotation);

                if (Random.value <= floorElementChance)
                {
                    GameObject randomFloorPrefab = floorElementPrefabs[Random.Range(0, floorElementPrefabs.Count)];
                    GameObject floor = Instantiate(randomFloorPrefab, newObj.transform);
                    floor.transform.localPosition = Vector3.zero;
                }

                if (Random.value <= airElementChance)
                {
                    GameObject randomEnvPrefab = airElementPrefabs[Random.Range(0, airElementPrefabs.Count)];
                    GameObject floor = Instantiate(randomEnvPrefab, newObj.transform);
                    floor.transform.localPosition = Vector3.zero;
                }

                var tileConnector = newObj.GetComponent<TileConnector>();

                //HERE
                if (tileConnector.tileType == TileType.Open)
                {
                    foreach (var location in tileConnector.objectLocationPoints)
                    {
                        if (Random.value <= objectElementChance)
                        {
                            GameObject randomObjPrefab =
                                objectElementPrefabs[Random.Range(0, objectElementPrefabs.Count)];
                            GameObject obj = Instantiate(randomObjPrefab, location.position, Quaternion.identity,
                                newObj.transform);
                            //obj.transform.localPosition = Vector3.zero;
                            obj.transform.rotation = Quaternion.identity;
                        }
                    }
                }

                tileCount++;
                occupied.Add(gridPos);

                for (int i = 0; i < prefabConn.exitDirections.Count; i++)
                {
                    Direction originalExit = prefabConn.exitDirections[i];
                    Direction rotatedExit = RotateDirection(originalExit, rotationSteps);

                    if (rotatedExit == enterFrom) continue;

                    Vector2Int nextGridPos = gridPos + DirectionToVector2Int(rotatedExit);
                    if (occupied.Contains(nextGridPos)) continue;

                    spawnQueue.Enqueue((nextGridPos, DirectionUtils.GetOpposite(rotatedExit)));
                }
            }
            Debug.Log($"Dungeon generation complete. Tiles placed: {tileCount}");

            FillEmptySpacesWithFullTiles();
            Debug.Log($"Added closing tiles.");
            
            AstarPath.active.Scan();
            Debug.Log($"Updated A* pathfinding grid.");
        }

        Direction RotateDirection(Direction dir, int steps)
        {
            return (Direction)(((int)dir + steps) % 4);
        }

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

        void FillEmptySpacesWithFullTiles()
        {
            HashSet<Vector2Int> positionsToFill = new();

            foreach (var pos in occupied)
            {
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

        GameObject GetRandomWeightedTile()
        {
            float totalWeight = 0f;

            foreach (GameObject tile in tilePrefabs)
            {
                TileConnector conn = tile.GetComponent<TileConnector>();
                totalWeight += conn.weight;
            }

            float randomValue = Random.Range(0, totalWeight);
            float cumulative = 0f;

            foreach (GameObject tile in tilePrefabs)
            {
                TileConnector conn = tile.GetComponent<TileConnector>();
                cumulative += conn.weight;

                if (randomValue <= cumulative)
                {
                    return tile;
                }
            }

            return tilePrefabs[0];
        }
    }
}