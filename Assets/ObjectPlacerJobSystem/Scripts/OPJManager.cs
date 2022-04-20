// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class OPJManager : MonoBehaviour
    {
        public static OPJManager Instance
        {
            get { return instance; }
        }

        private static OPJManager instance;

        public Dictionary<string, ObjectPlacerJob> jobs = new Dictionary<string, ObjectPlacerJob>();

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("There is already instance of OPJManager");
                Destroy(this);
            }

            instance = this;
        }

        public bool CreateJob(string name, ObjectPlacerJob job)
        {
            if (name == string.Empty)
            {
                Debug.LogWarning("Name is empty");
                return false;
            }

            if (jobs.ContainsKey(name))
            {
                Debug.LogWarning("There already is job with same name.");
                return false;
            }

            jobs.Add(name, job);
            return true;
        }

        public bool CreateJob(string name, GameObject[] prefabsToSpawn, int spawnCount, int maximumRetry, bool allowOverdraw,
            Func<PositionCheck, bool> RandomizeMethod, Func<GameObject, bool> InitializeSpawnedObject)
        {
            if (jobs.ContainsKey(name))
            {
                Debug.LogWarning("There already is job with same name.");
                return false;
            }

            EmptyOPJ newJob = gameObject.AddComponent<EmptyOPJ>();
            newJob.Initialize(prefabsToSpawn, spawnCount, maximumRetry, allowOverdraw, RandomizeMethod, InitializeSpawnedObject);

            jobs.Add(name, newJob);
            return true;
        }

        public bool TryRunJob<T>(string name, ref T job) where T : ObjectPlacerJob
        {
            if (!jobs.ContainsKey(name))
            {
                Debug.LogWarning("There isnt job with that name. First create job by CreateJob method.");
                return false;
            }

            ObjectPlacerJob jobToRun = jobs[name];
            jobToRun.Run();
            job = jobToRun as T;
            return true;
        }

        public bool TryRunJob(string name)
        {
            if (!jobs.ContainsKey(name))
            {
                Debug.LogWarning("There isnt job with name " + name + " .First create job by CreateJob method.");
                return false;
            }

            ObjectPlacerJob jobToRun = jobs[name];
            jobToRun.Run();
            return true;
        }

        public bool RemoveJob(string name)
        {
            if (!jobs.ContainsKey(name))
            {
                Debug.LogWarning("There isnt job with that name. First create job by CreateJob method.");
                return false;
            }

            ObjectPlacerJob jobToRun = jobs[name];
            GameObject.Destroy(jobs[name]);
            jobs.Remove(name);
            return true;
        }

    }
}