// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class MeshOPJ : BasicOPJ
    {
        public MeshFilter meshFilter;

        public override void Run()
        {
            _seed = 0;
            StartCoroutine(SpawnObjects<CheckForCollisions>(prefabsToSpawn, spawnCount, tries, allowOverdraw, RandomizePosition, InitializeSpawnedObject));
        }

        protected override bool InitializeSpawnedObject(GameObject spawnedObject)
        {
            if (spawnedObject.GetComponent<Rigidbody>() != null)
            {
                CustomPhysicsObject temp = spawnedObject.AddComponent<CustomPhysicsObject>();
                temp.Initialize(meshFilter.transform);
                OnComplete += temp.InitPhysics;
            }

            return true;
        }

        #region Mesh randomizator

        protected override bool RandomizePosition(PositionCheck positionCheck)
        {
            if (meshFilter == null)
                return false;

            Mesh mesh = meshFilter.mesh;
            int[] triangles = mesh.triangles;
            Vector3[] vertices = mesh.vertices;

            int max = Mathf.FloorToInt(triangles.Length / 3f);
            int rnd = _random.Next(0, max);

            Vector3 a = meshFilter.transform.position + vertices[triangles[(rnd * 3)]];
            Vector3 b = meshFilter.transform.position + vertices[triangles[(rnd * 3) + 1]];
            Vector3 c = meshFilter.transform.position + vertices[triangles[(rnd * 3) + 2]];

            Vector3 side1 = b - a;
            Vector3 side2 = c - a;
            Vector3 perp = Vector3.Cross(side1, side2);
            Vector3 normal = perp.normalized;

            var rndA = Random.value;
            var rndB = Random.value;
            var rndC = Random.value;

            Vector3 rndTriPoint = (rndA * a + rndB * b + rndC * c) / (rndA + rndB + rndC);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

            positionCheck.Set(rndTriPoint, rotation);
            return true;
        }

        #endregion
    }
}