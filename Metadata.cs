using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeModViewer
{
    public class Metadata
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string GUID { get; set; }

        public Metadata(string name, string version, string author, string description, string gUID)
        {
            Name = name;
            Version = version;
            Author = author;
            Description = description;
            GUID = gUID;
        }

        public Metadata(string name, string version, string author, string gUID)
        {
            Name = name;
            Version = version;
            Author = author;
            GUID = gUID;
        }
    }
}
