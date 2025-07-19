using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procgen
{
    public class DungeonGenerator : MonoBehaviour
    {
        public GameObject startTile;
        public List<GameObject> tilePrefabs;
        public int maxTiles = 50;
        public float deadEndChance = 0.1f;

        private HashSet<Vector2Int> _occupiedPositions = new();
        private Queue<(Vector3 position, Direction enterFrom)> _spawnQueue = new();
        private int _tileCount = 0;

        void Start()
        {
            Generate();
        }

        void Generate()
        {
            Debug.Log("Dungeon Generation Started.");

            GameObject start = Instantiate(startTile, Vector3.zero, Quaternion.identity);
            TileConnector conn = start.GetComponent<TileConnector>();

            if (conn == null)
            {
                Debug.LogError("Start tile is missing a TileConnector!");
                return;
            }

            Vector2Int startGridPos = WorldToGrid(Vector3.zero);
            _occupiedPositions.Add(startGridPos);

            for (int i = 0; i < conn.exitDirections.Count; i++)
            {
                _spawnQueue.Enqueue((conn.exitPoints[i].position, DirectionUtils.GetOpposite(conn.exitDirections[i])));
            }

            while (_spawnQueue.Count > 0 && _tileCount < maxTiles)
            {
                var (spawnPos, enterFrom) = _spawnQueue.Dequeue();
                Vector2Int gridPos = WorldToGrid(spawnPos);

                if (_occupiedPositions.Contains(gridPos))
                {
                    Debug.Log($"Skipped spawn at {gridPos} — already occupied.");
                    continue;
                }

                if (Random.value < deadEndChance)
                {
                    Debug.Log($"Dead end triggered at {gridPos}.");
                    continue;
                }

                GameObject nextPrefab = GetMatchingTile(enterFrom);
                if (nextPrefab == null)
                {
                    Debug.LogWarning($"No matching tile found for entry from {enterFrom} at {gridPos}.");
                    continue;
                }

                TileConnector prefabConnector = nextPrefab.GetComponent<TileConnector>();
                Direction prefabBaseEntry = prefabConnector.baseEntryDirection;

                Quaternion rotation = DirectionUtils.RotationTo(enterFrom, prefabBaseEntry);
                GameObject nextTile = Instantiate(nextPrefab, spawnPos, rotation);

                TileConnector newConnector = nextTile.GetComponent<TileConnector>();

                _tileCount++;
                _occupiedPositions.Add(gridPos);
                Debug.Log($"Spawned tile #{_tileCount} at {gridPos} entering from {enterFrom}");

                for (int i = 0; i < newConnector.exitDirections.Count; i++)
                {
                    Direction outDir = newConnector.exitDirections[i];

                    // Do NOT skip the exit that leads back, so remove any continue here

                    Transform exitPoint = newConnector.exitPoints[i];
                    Vector3 nextExitPos = exitPoint.position;
                    Direction opposite = DirectionUtils.GetOpposite(outDir);

                    Debug.Log($"Queueing spawn at {nextExitPos} entering from {opposite}");

                    _spawnQueue.Enqueue((nextExitPos, opposite));
                }

            }

            Debug.Log($"Dungeon generation complete. Tiles placed: {_tileCount}");
        }

        GameObject GetMatchingTile(Direction entryFrom)
        {
            var candidates = tilePrefabs.Where(prefab =>
            {
                var conn = prefab.GetComponent<TileConnector>();
                return conn != null && conn.exitDirections.Contains(DirectionUtils.GetOpposite(entryFrom));
            }).ToList();

            if (candidates.Count == 0)
            {
                Debug.LogWarning($"No prefab can be entered from {entryFrom}");
                return null;
            }

            return candidates[Random.Range(0, candidates.Count)];
        }

        Vector2Int WorldToGrid(Vector3 pos)
        {
            return new Vector2Int(Mathf.RoundToInt(pos.x / 10f), Mathf.RoundToInt(pos.y / 10f));
        }
    }
}