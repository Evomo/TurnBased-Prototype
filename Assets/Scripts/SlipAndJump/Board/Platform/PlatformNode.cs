using System;
using SlipAndJump.BoardMovers;
using SlipAndJump.Collectables;
using UnityEngine;

namespace SlipAndJump.Board.Platform {
    public class PlatformNode : BoardNode {
        private MeshRenderer _renderer;
        private PlayerMover _markedBy;
        public PlatformEffects effect;
        private ParticleSystem ps;

        #region Effects

        #endregion

        public override Vector2Int Coordinates {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        private Vector2Int _coordinates;
        private static readonly int ShaderColor = Shader.PropertyToID("_BaseColor");

        public void Awake() {
            _renderer = GetComponentInChildren<MeshRenderer>();
        }


        public void SetEffect(PlatformEffects e) {
            effect = e;

            if (ps != null) {
                ps.Stop();
                Destroy(ps);
            }

            if (e.particles != null) {
                ps = Instantiate(e.particles);
                ps.Play();
            }
        }


        public void SetColor(Color color) {
            _renderer.material.SetColor(ShaderColor, color);
        }

        public Collectable Spawn(Collectable prefab) {
            Collectable c = Instantiate(prefab);
            return c.Spawn(this);
        }

        public void MarkPlatform(PlayerMover playerMover) {
            _markedBy = playerMover;
            SetColor(playerMover.color);
        }

        public void ActivateSpecialEffect(PlayerMover mover) {
            if (effect != null) {
                effect.DoEffect(this, mover);
            }
        }
    }
}