using System;
using SlipAndJump.BoardMovers;
using SlipAndJump.Collectables;
using SlipAndJump.Util;
using UnityEngine;

namespace SlipAndJump.Board.Platform {
    public class PlatformNode : BoardNode {
        private PlayerMover _markedBy;

        [SerializeField] private Material otherMaterial;
        private MaterialSwitcher _materialSwitcher;
        private PlatformEffects _effect;
        private ParticleSystem ps;

        #region Effects

        #endregion

        public override Vector2Int Coordinates {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        private Vector2Int _coordinates;

        public void Awake() {
        _materialSwitcher = new MaterialSwitcher(gameObject, otherMaterial);
        }


        public void SetEffect(PlatformEffects e) {
            _effect = e;

            if (ps != null) {
                ps.Stop();
                Destroy(ps);
            }

            if (e.particles != null) {
                ps = Instantiate(e.particles);
                ps.Play();
            }
        }
        
        public Collectable Spawn(Collectable prefab) {
            Collectable c = Instantiate(prefab);
            return c.Spawn(this);
        }

        public void MarkPlatform(PlayerMover playerMover) {
            _markedBy = playerMover;
            _materialSwitcher.ChangeOriginalMaterialColor(playerMover.color);
        }

        public void Highlight(bool isMarked) {
            _materialSwitcher.ApplyMaterial(isMarked);
        }
        public void ActivateSpecialEffect(PlayerMover mover) {
            if (_effect != null) {
                _effect.DoEffect(this, mover);
            }
        }
    }
}