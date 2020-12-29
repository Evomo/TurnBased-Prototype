using System.Collections;
using Cinemachine;
using SlipAndJump.BoardMovers;
using SlipAndJump.Util;
using UnityEngine;

namespace SlipAndJump.Board.Camera {
    [RequireComponent(typeof(PlayerMover))]
    public class DirectionFollower : MonoBehaviour {
        private PlayerMover _player;

        public CinemachineDollyCart path;

        void Start() {
            _player = GetComponent<PlayerMover>();
            _player.onRotate.AddListener(ccw => StartCoroutine(UpdatePathPosition(ccw)));
            path.m_Position = (float)DirectionHelpers.Inverse(_player.facing);
        }


        IEnumerator UpdatePathPosition(bool ccw) {
            float time = 0f;

            float turnDuration = .5f;
            float start = path.m_Position;
            float end = start + (ccw ? -1 : 1);

            while (time < turnDuration) {
                time += Time.deltaTime;


                float linearT = time / turnDuration;
                path.m_Position = Mathf.Lerp(start, end, linearT);


                yield return null;
            }
        }
    }
}