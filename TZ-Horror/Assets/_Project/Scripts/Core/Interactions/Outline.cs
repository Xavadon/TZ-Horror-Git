using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xavadon
{
    public class Outline : MonoBehaviour
    {
        [SerializeField] private Material _outlineFill;
        [SerializeField] private Material _outlineMask;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            List<Material> materials = new List<Material>(_meshRenderer.sharedMaterials);
            materials.Add(_outlineFill);
            materials.Add(_outlineMask);
            _meshRenderer.materials = materials.ToArray();
            SetOutlineWidth(0);
        }

        public void SetOutlineWidth(float width)
        {
            _meshRenderer.materials[1].SetFloat("_OutlineWidth", width);
        }
    }
}
