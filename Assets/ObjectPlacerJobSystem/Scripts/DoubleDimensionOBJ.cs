// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class DoubleDimensionOBJ : BasicOPJ
    {
        public string jobName;

        public override void Run()
        {
            _seed = 0;
            StartCoroutine(SpawnObjects<CheckForCollisions2D>(prefabsToSpawn, spawnCount, tries, allowOverdraw, RandomizePosition, InitializeSpawnedObject));
        }

        protected override bool InitializeSpawnedObject(GameObject spawnedObject)
        {
            Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0;
                OnComplete += () =>
                {
                    rb.gravityScale = 1f;
                };
            }

            return true;
        }

        protected override bool RandomizePosition(PositionCheck positionCheck)
        {
            float minX = 0f;
            float maxX = 10f;
            float minY = 0f;
            float maxY = 10f;

            Vector3 randomPosition = new Vector3(_random.Range(minX, maxX), _random.Range(minY, maxY), 0);
            Quaternion normalizedRotation = Quaternion.identity;

            positionCheck.Set(randomPosition, normalizedRotation);
            return true;
        }
    }
}