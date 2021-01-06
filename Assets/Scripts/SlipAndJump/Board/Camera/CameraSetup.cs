using System.Collections.Generic;
using Cinemachine;
using SlipAndJump.Util;
using UnityEngine;

namespace SlipAndJump.Board.Camera {
    [RequireComponent(typeof(MapBoard))]
    public class CameraSetup : MonoBehaviour {
        private MapBoard _board;
        public Transform cameraFollow;
        public CinemachineSmoothPath cmSmoothPath;
        public float offset;
        public float height;
        void Start() {
            _board = GetComponent<MapBoard>();
            SetBoardPoints();
        }

        private void SetBoardPoints() {
            List<CinemachineSmoothPath.Waypoint> waypoints = new List<CinemachineSmoothPath.Waypoint>();
            int size = _board.mapSize;
            int mid = size / 2;

            cameraFollow.position = _board.Platforms[mid][mid].transform.position;
            cmSmoothPath.m_Waypoints = new CinemachineSmoothPath.Waypoint[4] {
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[mid][size-1], MapDirections.North)},
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[size-1][mid], MapDirections.East)},
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[mid][0], MapDirections.South)},
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[0][mid], MapDirections.West)},
            };
        }

        private Vector3 FromNode(PlatformNode p0, MapDirections direction) {
            Vector3 off = Vector3.zero;
            
            switch (direction) {
                case MapDirections.North:
                    off = new Vector3(0, height, offset);
                    break;
                case MapDirections.South:
                    off = new Vector3(0, height, -offset);
                    break;
                case MapDirections.East:
                    off = new Vector3(offset, height, 0);
                    break;
                case MapDirections.West:
                    off = new Vector3(-offset, height, 0);
                    break;
            }

            return p0.transform.localPosition + off;
        }
    }
}