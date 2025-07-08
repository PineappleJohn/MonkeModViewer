using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MonkeModViewer.Tools
{
    public static class AssemblyManager
    {
        public static readonly string[] names =
            {
            "GUID",
            "Name",
            "Version"
        };
        public static List<Metadata> FindDllsWithSpecificClasses(string directoryPath)
        {
            // Use a list to store metadata for all mods
            List<Metadata> allModsMetadata = new List<Metadata>();

            if (!Directory.Exists(directoryPath))
            {
                Logging.Fatal($"Directory not found: {directoryPath}");
                return allModsMetadata;
            }

            var dllFiles = Directory.GetFiles(directoryPath, "*.dll", SearchOption.AllDirectories);

            foreach (var dllFile in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dllFile);
                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        if (type.Name == "PluginInfo" || type.Name == "Constants")
                        {
                            Logging.Info($"Found matching class in: {dllFile}");

                            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                            var fieldValues = new Dictionary<string, string>();

                            foreach (var fieldInfo in fields)
                            {
                                if (AssemblyManager.names.Contains(fieldInfo.Name))
                                {
                                    var value = fieldInfo.GetValue(null)?.ToString();
                                    fieldValues[fieldInfo.Name] = value;
                                }
                            }

                            if (fieldValues.ContainsKey("GUID") && fieldValues.ContainsKey("Name") && fieldValues.ContainsKey("Version"))
                            {
                                var author = fieldValues["GUID"].Split(".")[0];
                                var metadata = new Metadata(
                                    fieldValues["Name"],
                                    fieldValues["Version"],
                                    author,
                                    fieldValues["GUID"]
                                );
                                Logging.Info("mod found", metadata.GUID, metadata.Name, metadata.Description, metadata.Version);
                                allModsMetadata.Add(metadata);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
            }
            return allModsMetadata;
            // Now allModsMetadata contains a Metadata object for each mod found
        }

    }
}
