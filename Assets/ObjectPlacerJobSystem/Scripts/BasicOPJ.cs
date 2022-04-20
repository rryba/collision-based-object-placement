// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class BasicOPJ : ObjectPlacerJob , ISaveableJob
    {
        public GameObject[] prefabsToSpawn;
        public int spawnCount;
        public int tries;
        public bool allowOverdraw;

        protected virtual bool InitializeSpawnedObject(GameObject spawnedObject)
        {
            return true;
        }

        protected virtual bool RandomizePosition(PositionCheck positionCheck)
        {
            return true;
        }

        public override void Run()
        {
            _seed = 0;
            StartCoroutine(SpawnObjects<CheckForCollisions>(prefabsToSpawn, spawnCount, tries, allowOverdraw, RandomizePosition,
                InitializeSpawnedObject));
        }

        public virtual void Run(int seed, bool tryLoadJob = true)
        {
            if (tryLoadJob)
            {
                if (TryLoadJob(seed))
                    return;
            }

            base.Run(seed);
        }

        public void SaveJobDataAsSO()
        {
            if (IsWorking)
            {
                Debug.LogWarning("Job is currently working");
                return;
            }

            if (placedObjectsData == null || placedObjectsData.Count <= 0)
            {
                Debug.LogWarning("Nothing to save. COnsider firsty run a job.");
                return;
            }

            // Data can be null whenever given path is invalid.
            if (!ScriptableObjectUtility.TryCreateJobAsset<JobData>(resourcesPath, out JobData data ))
                return;

            data.jobName = GetType().ToString();
            data.seed = _seed;
            data.prefabs = prefabsToSpawn;
            data.GenerateHexID();
            data.placedObjectsData = new List<PlacedObjectData>();
            foreach (var gameObject in placedObjectsData)
            {
                data.placedObjectsData.Add(gameObject.Clone());
            }

            ScriptableObjectUtility.SaveAsset(data);
        }

        public bool TryLoadJob(int seedToLoad, Transform parent = null)
        {
            JobData[] jobs = Resources.LoadAll<JobData>("");

            JobData foundJob = null;
            string jobName = GetType().ToString();
            for (int i = 0; i < jobs.Length; i++)
            {
                if (jobs[i].jobName == jobName && jobs[i].seed == seedToLoad)
                {
                    foundJob = jobs[i];
                    break;
                }
            }

            if (foundJob == null)
            {
                Debug.LogWarning("Cant find job type of " + jobName  + "with seed :" + seedToLoad);
                return false;
            }
            ClearJobData();

            LoadJob(foundJob);
            return true;
        }
    }
}