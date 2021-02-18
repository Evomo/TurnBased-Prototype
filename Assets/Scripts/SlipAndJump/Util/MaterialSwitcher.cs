using UnityEngine;

namespace SlipAndJump.Util {
    public class MaterialSwitcher {
        private MeshRenderer _meshRenderer;
        private static readonly int ShaderColor = Shader.PropertyToID("_BaseColor");

        private Material originalMaterial, otherMaterial;

        public MaterialSwitcher(GameObject owner, Material newOtherMaterial) {
            _meshRenderer = owner.GetComponentInChildren<MeshRenderer>();
            originalMaterial = _meshRenderer.material;
            otherMaterial = newOtherMaterial;
        }

        public void ApplyMaterial(bool useOther = false) {
            if (useOther) {
                _meshRenderer.material = otherMaterial;
            }
            else {
                _meshRenderer.material = originalMaterial;
            }
        }

        public void ChangeOriginalMaterialColor(Color color ) {
            _meshRenderer.material.SetColor(ShaderColor, color);

        }
    }
}