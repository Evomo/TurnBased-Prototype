using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace SlipAndJump.Board.Camera {
    [RequireComponent(typeof(MapBoard))]
    public class CameraSetup : MonoBehaviour {
        private MapBoard _board;
        public Transform CameraFollow;
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

            CameraFollow.position = _board.Platforms[mid][mid].transform.position;
            cmSmoothPath.m_Waypoints = new CinemachineSmoothPath.Waypoint[4] {
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[mid][0], MapDirections.SOUTH)},
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[0][mid], MapDirections.WEST)},
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[mid][size-1], MapDirections.NORTH)},
                new CinemachineSmoothPath.Waypoint {position = FromNode(_board.Platforms[size-1][mid], MapDirections.EAST)},
            };
        }

        private Vector3 FromNode(PlatformNode p0, MapDirections direction) {
            Vector3 off = Vector3.zero;
            
            switch (direction) {
                case MapDirections.NORTH:
                    off = new Vector3(0, height, offset);
                    break;
                case MapDirections.SOUTH:
                    off = new Vector3(0, height, -offset);
                    break;
                case MapDirections.EAST:
                    off = new Vector3(offset, height, 0);
                    break;
                case MapDirections.WEST:
                    off = new Vector3(-offset, height, 0);
                    break;
            }

            return p0.transform.localPosition + off;
        }
    }
}