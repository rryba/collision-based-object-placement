// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace ObjectPlacerJobSystem
{
    public class JobData : ScriptableObject
    {
        public string ID;
        public string jobName;
        public int seed;
        public GameObject[] prefabs;
        public List<PlacedObjectData> placedObjectsData;

        public void GenerateHexID()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(jobName);
            sb.Append(seed);
            foreach (var gameObject in prefabs)
                sb.Append(gameObject.name);

            ID = sb.ToString().ToHexString();
        }
    }

}
 