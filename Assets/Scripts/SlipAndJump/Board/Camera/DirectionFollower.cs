using System.Collections;
using Cinemachine;
using SlipAndJump.BoardMovers;
using UnityEngine;

namespace SlipAndJump.Board.Camera {
    [RequireComponent(typeof(PlayerMover))]
    public class DirectionFollower : MonoBehaviour {
        private PlayerMover player;

        public CinemachineDollyCart path;

        void Start() {
            player = GetComponent<PlayerMover>();
            player.onRotate.AddListener(ccw => StartCoroutine(UpdatePathPosition(ccw)));
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