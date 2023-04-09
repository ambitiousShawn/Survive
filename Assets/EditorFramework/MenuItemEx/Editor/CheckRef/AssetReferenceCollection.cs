using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.EditorFramework
{

    public interface IReferenceCollection
    {
        void CollectionFiles(string[] checkFolderPath);
        void Init(List<CollectionData> refs);
    }

    [Serializable]
    public class CollectionData
    {
        public string fileGuid;
        public string fileName;
        public List<string> referenceGids;
        public List<string> referenceNames;
    }

    [Serializable]
    public class DeleteAssetData
    {
        public string filePath;
    }
}