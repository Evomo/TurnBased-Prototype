using UnityEngine;

namespace SlipAndJump.Board.Platform {
    public class BoardNode : MonoBehaviour {
        public Transform landingPosition;

        public virtual Vector2Int Coordinates {
            get { return new Vector2Int(-1, -1); }
            set {}
        }



    }
}