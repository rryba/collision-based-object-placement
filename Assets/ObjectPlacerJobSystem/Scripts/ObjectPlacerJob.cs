// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace ObjectPlacerJobSystem
{
    public class ObjectPlacerJob : MonoBehaviour
    {
        [Tooltip("Resources path where the job gonna be saved and from where can be loaded.")]
        [SerializeField] protected string resourcesPath;
        
        public bool IsWorking => coroutines != null && coroutines.Any();
#if UNITY_EDITOR
        public bool HasData => placedObjectsData != null && placedObjectsData.Count > 0;
#endif
        public event Action OnCompleteObject;
        public event Action OnComplete;
        public event Action<GameObject> OnSpawnedObject;

        protected System.Random _random;
        protected int _seed;
        private GameObject _jobParentObject;

        private List<Coroutine> coroutines;
#if UNITY_EDITOR
        protected List<PlacedObjectData> placedObjectsData;
#endif
        public virtual void Run()
        {
            Debug.LogWarning("This is empty job. You should run it by passing parameters to SpawnObject function.");
            Debug.LogWarning("Run job is stopped.");
        }

        public virtual void Run(int seed)
        {
            this._seed = seed;
            Run();
        }

        public virtual void Stop()
        {
            if (!IsWorking)
                return;

            foreach (var c in coroutines.Where(c => c != null))
            {
                StopCoroutine(c);
            }

        

            coroutines = null;
        }

        public IEnumerator SpawnObjects<T>(GameObject[] prefabsToSpawn, int spawnCount, int maximumRetry, bool allowOverdraw,
            Func<PositionCheck, bool> randomizeMethod, Func<GameObject, bool> initializeSpawnedObject, GameObject parent = null)
            where T : CheckForCollisions
        {
            _seed = _seed != 0 ? _seed : Guid.NewGuid().GetHashCode();
            _random = new System.Random(_seed);
#if UNITY_EDITOR
            float timestamp = Time.time;
            Debug.Log($"Run with objects count = {spawnCount}, max possible tries = {maximumRetry}");

            placedObjectsData = new List<PlacedObjectData>();
#endif
            if (parent == null)
            {
                Debug.LogWarning($"Passed parent for spawned object by objects placer job is null. " +
                                 $"Used empty parent named `PlacementObjectJobContainer` instead.");
                parent = new GameObject($"{gameObject.name} PlacementObjectJobContainer");
            }

            _jobParentObject = parent;
            
            coroutines = new List<Coroutine>();
            for (int i = 0; i < prefabsToSpawn.Length; i++)
            {
                int offset = Mathf.CeilToInt(_random.Next(-spawnCount / prefabsToSpawn.Length, spawnCount / prefabsToSpawn.Length));
                PositionCheck positionCheck = new PositionCheck(Vector3.zero, Quaternion.identity);

                Coroutine c = StartCoroutine(SpawnObject<T>(i, prefabsToSpawn[i], positionCheck,
                    Mathf.RoundToInt(spawnCount / prefabsToSpawn.Length) + offset, maximumRetry, allowOverdraw, randomizeMethod,
                    initializeSpawnedObject));

                coroutines.Add(c);
            }

            foreach (Coroutine cor in coroutines)
                yield return cor;

            if (OnComplete != null)
                OnComplete();

            coroutines = null;

#if UNITY_EDITOR
            Debug.Log($"Time to generate = {Time.time - timestamp}");
#endif
        }

        private IEnumerator SpawnObject<T>(int index, GameObject prefabToSpawn, PositionCheck positionCheck, int spawnCount, int maximumRetry, bool allowOverdraw,
            Func<PositionCheck, bool> randomizeMethod, Func<GameObject, bool> initializeSpawnedObject)
            where T : CheckForCollisions
        {
            int currentObjectsCount = 0, currentRetry = 0;

            CheckForCollisions template = Instantiate(prefabToSpawn).AddComponent<T>();

            while (currentObjectsCount < spawnCount && currentRetry < maximumRetry)
            {
                positionCheck.Reset();

                if (!randomizeMethod(positionCheck))
                {
                    currentRetry++;
                    continue;
                }

                if (!allowOverdraw)
                {
                    yield return StartCoroutine(IsPositionAvailable(template, positionCheck, allowOverdraw));

                    if (!positionCheck.isPositionAvailable)
                    {
                        currentRetry++;
                        continue;
                    }
                }

                GameObject obj = Instantiate(prefabToSpawn, positionCheck.randomPosition, positionCheck.normalizedRotation, _jobParentObject.transform);
                initializeSpawnedObject(obj);

                if (OnSpawnedObject != null)
                    OnSpawnedObject(obj);

#if UNITY_EDITOR
                PlacedObjectData objectData = new PlacedObjectData(index, prefabToSpawn.name, positionCheck.randomPosition, positionCheck.normalizedRotation);
                objectData.SetReferalObject(obj);
                placedObjectsData.Add(objectData);
#endif

                currentObjectsCount++;
            }

            Destroy(template.gameObject);

            if (OnCompleteObject != null)
                OnCompleteObject();
        }

        private IEnumerator IsPositionAvailable(CheckForCollisions template, PositionCheck positionCheck, bool allowOverdraw = false)
        {
            if (allowOverdraw)
            {
                positionCheck.PossitionAllowed();
                yield break;
            }

            yield return StartCoroutine(template.IsColliding(positionCheck));
        }

        public void LoadJob(JobData jobData, GameObject parent = null)
        {
            if (parent == null)
            {
                Debug.LogWarning("Passed parent for spawned object by objects placer job is null. Used empty parent named `PlacementObjectJobContainer` instead. ");
                parent = new GameObject(gameObject.name + " PlacementObjectJobContainer");
            }

            _jobParentObject = parent;

            placedObjectsData = new List<PlacedObjectData>();
            for (int i = 0; i < jobData.placedObjectsData.Count; i++)
            {
                PlacedObjectData objectData = jobData.placedObjectsData[i].Clone();
                GameObject obj = Instantiate(jobData.prefabs[objectData.prefabIndex], objectData.position.V3, objectData.rotation.Quaternion,
                    _jobParentObject.transform);
#if UNITY_EDITOR
                objectData.SetReferalObject(obj);
                placedObjectsData.Add(objectData);
#endif
                if (OnSpawnedObject != null)
                    OnSpawnedObject(obj);
            }

            if (OnComplete != null)
                OnComplete();
        }

        public bool TryLoadJob(string jobId, GameObject parent = null)
        {
            JobData[] datas = Resources.LoadAll<JobData>("");
            JobData foundJob = null;
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].ID == jobId)
                {
                    foundJob = datas[i];
                    break;
                }
            }

            if (foundJob == null)
            {
                Debug.LogWarning("Cant find job with id :" + jobId);
                return false;
            }

            LoadJob(foundJob, parent);
            
            Resources.UnloadUnusedAssets();
            return true;
        }

        public void ClearJobData()
        {
            if (IsWorking)
            {
                Debug.LogWarning("Job is currently working...");
                return;
            }

            if (placedObjectsData == null || placedObjectsData.Count <= 0)
            {
                Debug.LogWarning("Nothing to clear. Consider run a job first.");
                return;
            }

            for (int i = 0; i < placedObjectsData.Count; i++)
            {
                if (placedObjectsData[i].referalObject != null)
                    Destroy(placedObjectsData[i].referalObject);

                placedObjectsData[i] = null;
            }
            
            Destroy(_jobParentObject);

            placedObjectsData.Clear();
            placedObjectsData = null;
            OnComplete = null;
        }
    }
}