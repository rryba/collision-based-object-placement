// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class LoadingJobsExample : MonoBehaviour
    {

        [SerializeField] private TerrainOPJ job;
        [SerializeField] private List<int> seeds;

        private int currentIndex = 0;

        void Start()
        {
            StartCoroutine(LoadJobsCoroutine());
        }

        IEnumerator LoadJobsCoroutine()
        {
            while (true)
            {
                currentIndex = (currentIndex + 1) % (seeds.Count - 1);
                job.Run(seeds[currentIndex]);
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
