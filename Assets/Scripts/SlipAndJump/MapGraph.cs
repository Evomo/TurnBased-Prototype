using System;
using UnityEngine;

namespace SlipAndJump {
    public class MapGraph : MonoBehaviour{
        public PlatformNode[][] platforms;

        [SerializeField] private PlatformNode platformPrefab;
        [SerializeField, Range(3, 10)] private int mapSize = 5;
        [SerializeField, Range(1, 5)] private float spacing = 2;

        public PlatformNode StartNode {
            get {
                int center = mapSize / 2;
                return platforms[center][center];
            }
        }
        void Awake() {
            platforms = new PlatformNode[mapSize][];
            SpawnPlatforms();
            SetNeighbors();
        }

        private void SpawnPlatforms() {
            for (int x = 0; x < mapSize; x++) {
                platforms[x] = new PlatformNode[mapSize];
                for (int y = 0; y < mapSize; y++) {
                    PlatformNode platform = Instantiate(platformPrefab, transform, true);
                    platform.transform.position = new Vector3(x * spacing, transform.position.y, y * spacing);
                    platform.coordinates = new Vector2Int(x, y);
                    platform.transform.name = $"Node: {x}-{y}";
                    platforms[x][y] = platform;
                }
            }
        }

        private void SetNeighbors() {
            for (int x = 0; x < mapSize; x++) {
                for (int y = 0; y < mapSize; y++) {
                    PlatformNode platform = platforms[x][y];
                }
            }
        }

        public PlatformNode GetPlatform(Vector2Int coord) {
            try {
                PlatformNode pn = platforms[coord.x][coord.y];
                return pn;
            }
            catch (IndexOutOfRangeException ) {
                return null;
            }
        }
    }
}