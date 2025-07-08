using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MonkeModViewer.Tools
{
    public static class AssetLoader
    {
        public static string FormatPath(string path)
        {
            return path.Replace("/", ".").Replace("\\", ".");
        }
        
        public static AssetBundle LoadAssetBundle(string path)
        {
            path = FormatPath(path);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
    }
}
