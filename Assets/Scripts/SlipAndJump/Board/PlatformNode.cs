using SlipAndJump.Collectables;
using UnityEngine;

namespace SlipAndJump.Board {
    public class PlatformNode : BoardNode {
        public override Vector2Int Coordinates {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        private Vector2Int _coordinates;


        public Collectable Spawn(Collectable prefab) {
            Collectable c = Instantiate(prefab);
            return c.Spawn(this);
        }
    }
}