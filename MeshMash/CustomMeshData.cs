using UnityEngine;

// Lovingly ripped off from aedenthorn.CustomMeshes 0.2.3

namespace MeshMash
{
    internal class CustomMeshData
    {
        public string objName;
        public string meshName;
        public Mesh mesh;
        public SkinnedMeshRenderer renderer;

        public CustomMeshData(string dirName, string name, Mesh mesh, SkinnedMeshRenderer renderer = null)
        {
            this.objName = dirName;
            this.meshName = name;
            this.mesh = mesh;
            this.renderer = renderer;
        }
    }
}