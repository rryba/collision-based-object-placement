// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace ObjectPlacerJobSystem
{
    public class EmptyOPJ : ObjectPlacerJob , ISaveableJob
    {
        private GameObject[] prefabsToSpawn;
        private int spawnCount;
        private int maximumRetry;
        private bool allowOverdraw;

        private Func<PositionCheck, bool> RandomizeMethod;
        private Func<GameObject, bool> InitializeSpawnedObject;

        public void Initialize(GameObject[] prefabsToSpawn, int spawnCount, int maximumRetry, bool allowOverdraw,
            Func<PositionCheck, bool> RandomizeMethod, Func<GameObject, bool> InitializeSpawnedObject)
        {
            this.prefabsToSpawn = prefabsToSpawn;
            this.spawnCount = spawnCount;
            this.maximumRetry = maximumRetry;
            this.allowOverdraw = allowOverdraw;
            this.RandomizeMethod = RandomizeMethod;
            this.InitializeSpawnedObject = InitializeSpawnedObject;
        }

        public override void Run()
        {
            _seed = 0;
            StartCoroutine(SpawnObjects<CheckForCollisions>(prefabsToSpawn, spawnCount, maximumRetry, allowOverdraw,
                RandomizeMethod, InitializeSpawnedObject));
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

            if (!ScriptableObjectUtility.TryCreateJobAsset<JobData>(resourcesPath, out JobData data))
                return;

            data.jobName = this.GetType().ToString();
            data.seed = _seed;
            data.prefabs = prefabsToSpawn;
            data.GenerateHexID();
            data.placedObjectsData = new List<PlacedObjectData>();
            for (int i = 0; i < placedObjectsData.Count; i++)
            {
                data.placedObjectsData.Add(placedObjectsData[i].Clone());
            }

            ScriptableObjectUtility.SaveAsset(data);
        }

        public bool TryLoadJob(int seedToLoad, Transform parent = null)
        {
            JobData[] datas = Resources.LoadAll<JobData>("");

            JobData foundJob = null;
            string jobName = this.GetType().ToString();
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].jobName == jobName && datas[i].seed == seedToLoad)
                {
                    foundJob = datas[i];
                    break;
                }
            }

            if (foundJob == null)
            {
                Debug.LogWarning("Cant find job type of " + jobName + "with seed :" + seedToLoad);
                return false;
            }
            ClearJobData();

            LoadJob(foundJob);
            foundJob = null;
            return true;
        }
    }
}