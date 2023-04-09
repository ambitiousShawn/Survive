using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Shawn.EditorFramework
{

    public class UIResCollection
    {
        public List<string> UIResPathList = new List<string>();

        public void CollectionUIRes(string[] targetFolder)
        {
            UIResPathList = GetUIFiles(targetFolder);
        }

        private List<string> GetUIFiles(string[] targetFolder)
        {
            IEnumerable<string> files = targetFolder.SelectMany(
                c => Directory.GetFiles(c, "*.prefab", SearchOption.AllDirectories))
                .Distinct()
                .Where(item => Path.GetFileName(item).StartsWith("UGUI_"));
            return files.ToList();
        }
    }

}