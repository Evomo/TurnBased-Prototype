using System;
using System.Collections.Generic;
using SlipAndJump.Board.Platform;
using SlipAndJump.BoardMovers;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Collectables;
using SlipAndJump.Util;
using UnityEngine;
using UnityEngine.Events;

namespace SlipAndJump.Board {
    [DisallowMultipleComponent]
    public class MapBoard : MonoBehaviour {
        public PlatformNode[][] Platforms;
        public PlayerMover player;
        [SerializeField, Range(3, 10)] public int mapSize = 5;

        public List<SpawnerNode> spawnerNodes;
        [SerializeField] private PlatformNode boardPrefab;
        [SerializeField] private SpawnerNode spawnerPrefab;
        [SerializeField, Range(1, 5)] private float spacing = 2;
        public UnityEvent onTurn;

        public PlatformNode StartNode {
            get {
                int center = mapSize / 2;
                return Platforms[center][center];
            }
        }

        void Awake() {
            Platforms = new PlatformNode[mapSize][];
            SpawnPlatforms();
            SetSpawnNodes();
        }

        #region Setup

        private void SetSpawnNodes() {
            GameObject pgo = new GameObject();
            pgo.transform.position = transform.position;
            Transform parent = pgo.transform;
            parent.parent = transform;

            parent.name = "Spawner Nodes";
            //Horizontal
            for (int i = 0; i < mapSize; i++) {
                PlatformNode forwardNode = Platforms[i][0];
                SpawnerNode sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.back * spacing;
                sn.transform.name = $"SN: {i}-0";
                sn.forwardNode = forwardNode;
                sn.forwardDirection = MapDirections.North;
                spawnerNodes.Add(sn);

                forwardNode = Platforms[i][mapSize - 1];
                sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.forward * spacing;
                sn.transform.name = $"SN: {i}-{mapSize - 1}";
                sn.forwardNode = forwardNode;
                sn.forwardDirection = MapDirections.South;
                spawnerNodes.Add(sn);
            }


            //Vertical
            for (int i = 0; i < mapSize; i++) {
                PlatformNode forwardNode = Platforms[0][i];
                SpawnerNode sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.left * spacing;
                sn.transform.name = $"SN: {0}-{i}";
                sn.forwardNode = forwardNode;
                sn.forwardDirection = MapDirections.East;
                spawnerNodes.Add(sn);


                forwardNode = Platforms[mapSize - 1][i];
                sn = Instantiate(spawnerPrefab, parent, true);
                sn.transform.position = forwardNode.landingPosition.transform.position + Vector3.right * spacing;
                sn.transform.name = $"SN: {mapSize - 1}-{i}";
                sn.forwardDirection = MapDirections.West;
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
                Platforms[x] = new PlatformNode[mapSize];
                for (int y = 0; y < mapSize; y++) {
                    PlatformNode board = Instantiate(boardPrefab, parent, true);
                    board.transform.position = new Vector3(x * spacing, transform.position.y, y * spacing);
                    board.transform.name = $"Node: {x}-{y}";
                    board.Coordinates = new Vector2Int(x, y);
                    Platforms[x][y] = board;
                    
                }
            }
        }

        #endregion


        public PlatformNode GetPlatform(Vector2Int coord) {
            try {
                PlatformNode pn = Platforms[coord.x][coord.y];
                return pn;
            }
            catch (IndexOutOfRangeException) {
                return null;
            }
        }
    }
}