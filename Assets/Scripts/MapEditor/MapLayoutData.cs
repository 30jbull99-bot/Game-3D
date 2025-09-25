using System;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseProtocol.MapEditor
{
    [Serializable]
    public class MapLayoutData
    {
        [Serializable]
        public class Entry
        {
            public string prefabGuid;
            public Vector3 position;
            public Quaternion rotation;
        }

        public List<Entry> entries = new();
    }
}
