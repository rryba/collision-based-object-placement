// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using ObjectPlacerJobSystem;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class Scene2DExample : MonoBehaviour
    {
        public DoubleDimensionOBJ[] jobs;

        private void Start()
        {
            if (OPJManager.Instance == null)
            {
                Debug.LogWarning("There is not opj manager instance set.");
                return;
            }

            for (int i = 0; i < jobs.Length; i++)
            {
                OPJManager.Instance.CreateJob(jobs[i].jobName, jobs[i]);
            }

            OPJManager.Instance.TryRunJob("2dopj");
            OPJManager.Instance.TryRunJob("2doverdrawopj");
        }
    }
}