using UnityEngine;

namespace SlipAndJump.Board {
    public class BoardNode : MonoBehaviour {
        public Transform landingPosition;

        public virtual Vector2Int Coordinates => new Vector2Int(-1, -1);
    }
}