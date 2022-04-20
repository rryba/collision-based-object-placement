// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using ObjectPlacerJobSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class TerrainSceneExample : MonoBehaviour
    {
        public ObjectPlacerJob jobToRun;

        void Start()
        {
            if (jobToRun != null)
                jobToRun.Run();

        }
    }
}