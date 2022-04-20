// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public interface ISaveableJob
    {
        void SaveJobDataAsSO();
        void ClearJobData();
        bool TryLoadJob(int seedToLoad, Transform parent = null);
        void Run(int seed, bool tryLoadJob = true);
    }
}