using System;
using System.Collections.Generic;
using SlipAndJump.BoardMovers;
using UnityEngine;
using UnityEngine.Events;

namespace SlipAndJump.Board {
    [DisallowMultipleComponent]
    public class MapBoard : MonoBehaviour {
        public PlatformNode[][] platforms;
        public PlayerMover player;
        public List<BaseMover> entities;
        public List<SpawnerNode> spawnerNodes;
        [SerializeField] private PlatformNode boardPrefab;
        [SerializeField] private SpawnerNode spawnerPrefab;
        [SerializeField, Range(3, 10)] private int mapSize = 5;
        [SerializeField, Range(1, 5)] private float spacing = 2;
        public UnityEvent onTurn;

        public PlatformNode StartNode {
            get {
                int center = mapSize / 2;
                return platforms[center][center];
            }
        }

        void Awake() {
            onTurn = new UnityEvent();
            entities = new List<BaseMover>();
            platforms = new PlatformNode[mapSize][];
            SpawnPlatforms();
            SetNeighbors();
            SetSpawnNodes();
        }

        private void SetSpawnNodes() {
            GameObject pgo = new GameObject();
            pgo.transform.position = transform.position;
            Transform parent = pgo.transform;
            parent.parent = transform;

            parent.name = "Spawner Nodes";
            //Horizontal
            for (int i = 0; i < mapSize; i++) {
                PlatformNode forwardNode = platforms[i][0];
                SpawnerNode sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.back * spacing;
                sn.transform.name = $"SN: {i}-0";
                sn.forwardNode = forwardNode;
                sn.forwardDirection = MapDirections.NORTH;
                spawnerNodes.Add(sn);

                forwardNode = platforms[i][mapSize - 1];
                sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.forward * spacing;
                sn.transform.name = $"SN: {i}-{mapSize - 1}";
                sn.forwardNode = forwardNode;
                sn.forwardDirection = MapDirections.SOUTH;
                spawnerNodes.Add(sn);
            }


            //Vertical
            for (int i = 0; i < mapSize; i++) {
                PlatformNode forwardNode = platforms[0][i];
                SpawnerNode sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.left * spacing;
                sn.transform.name = $"SN: {0}-{i}";
                sn.forwardNode = forwardNode;
                sn.forwardDirection = MapDirections.EAST;
                spawnerNodes.Add(sn);


                forwardNode = platforms[mapSize - 1][i];
                sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.right * spacing;
                sn.transform.name = $"SN: {mapSize - 1}-{i}";
                sn.forwardDirection = MapDirections.WEST;
                sn.forwardNode = forwardNode;
                spawnerNodes.Add(sn);
            }
        }

        private void SpawnPlatforms() {
            GameObject pgo = new GameObject();
            pgo.transform.position = transform.position;

            Transform parent = pgo.transform;
            parent.parent = transform;
            parent.name = "Platform Nodes";
            for (int x = 0; x < mapSize; x++) {
                platforms[x] = new PlatformNode[mapSize];
                for (int y = 0; y < mapSize; y++) {
                    PlatformNode board = Instantiate(boardPrefab, parent, true);
                    board.transform.position = new Vector3(x * spacing, transform.position.y, y * spacing);
                    board.transform.name = $"Node: {x}-{y}";
                    board.coordinates = new Vector2Int(x, y);
                    platforms[x][y] = board;
                }
            }
        }

        private void SetNeighbors() {
            for (int x = 0; x < mapSize; x++) {
                for (int y = 0; y < mapSize; y++) {
                    PlatformNode board = platforms[x][y];
                }
            }
        }

        public PlatformNode GetPlatform(Vector2Int coord) {
            try {
                PlatformNode pn = platforms[coord.x][coord.y];
                return pn;
            }
            catch (IndexOutOfRangeException) {
                return null;
            }
        }
    }
}