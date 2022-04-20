// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class OPJLoader : ObjectPlacerJob
    {
        public string jobId;
        public GameObject parent;

        public void LoadJob()
        {
            TryLoadJob(jobId, parent);
        }
    }

}

