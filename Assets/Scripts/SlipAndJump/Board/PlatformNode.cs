using SlipAndJump.BoardMovers;
using SlipAndJump.Collectables;
using UnityEngine;

namespace SlipAndJump.Board {

    public class PlatformNode : BoardNode {
        
        private MeshRenderer _renderer;
        private PlayerMover _markedBy;
        public override Vector2Int Coordinates {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        private Vector2Int _coordinates;
        private static readonly int ShaderColor = Shader.PropertyToID("_BaseColor");

        public void Awake() {
            _renderer = GetComponentInChildren<MeshRenderer>();
            
        }

        public void  SetColor(Color color) {
            _renderer.material.SetColor(ShaderColor, color);
        }
        public Collectable Spawn(Collectable prefab) {
            Collectable c = Instantiate(prefab);
            return c.Spawn(this);
        }
        
        public void PlayerInteraction(PlayerMover playerMover) {
            _markedBy = playerMover;
            SetColor(playerMover.color);
        }
    }
}