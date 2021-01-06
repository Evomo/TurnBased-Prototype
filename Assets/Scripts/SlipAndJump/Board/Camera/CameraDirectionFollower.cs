using System;
using System.Collections;
using Cinemachine;
using SlipAndJump.BoardMovers;
using SlipAndJump.Util;
using UnityEngine;

namespace SlipAndJump.Board.Camera {
    [RequireComponent(typeof(PlayerMover))]
    public class CameraDirectionFollower : MonoBehaviour {
        private PlayerMover _player;

        public CinemachineDollyCart path;

        [SerializeField]
        private bool _shouldAutoRotate;

        private Vector2 lastTouch = Vector2.zero;
        public void ToggleRotate(){
            _shouldAutoRotate = !_shouldAutoRotate;
                if (_shouldAutoRotate) {
                    float start = path.m_Position;
                    float end = (float)DirectionHelpers.Inverse(_player.facing);
                    StartCoroutine(UpdatePathPosition(start , end)); 
            }
        }

        void Start() {
            _player = GetComponent<PlayerMover>();
            _player.onPreRotate.AddListener(ccw => AutoRotate(ccw));
            path.m_Position = (float)DirectionHelpers.Inverse(_player.facing);
        }

        public void AutoRotate(bool ccw) {
            if (_shouldAutoRotate) {
                float start = (float)DirectionHelpers.Inverse(_player.facing);
                float end = start + (ccw ? -1 : 1);
                StartCoroutine(UpdatePathPosition(start , end));
            }
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && !_shouldAutoRotate) {
                Vector2 curr = Input.mousePosition.normalized;
                float distance = Vector3.Distance(curr, lastTouch);
                
                float start = path.m_Position;
                float end = start + Time.deltaTime * distance;
                StartCoroutine(UpdatePathPosition(start , end, Time.deltaTime)); 
                lastTouch = curr;
            }

        }

        IEnumerator UpdatePathPosition(float start, float end, float turnDuration = .5f) {
                
            float time = 0f;


            while (time < turnDuration) {
                time += Time.deltaTime;


                float linearT = time / turnDuration;
                path.m_Position = Mathf.Lerp(start, end, linearT);


                yield return null;
            }
        }
    }
}