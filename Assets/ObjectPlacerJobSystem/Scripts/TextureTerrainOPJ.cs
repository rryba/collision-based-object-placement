// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class TextureTerrainOPJ : BasicOPJ
    {
        public int allowedTerraniIndex;
        public Terrain terrain;

        public override void Run()
        {
            _seed = 0;
            StartCoroutine(SpawnObjects<CheckForCollisions>(prefabsToSpawn, spawnCount, tries, allowOverdraw, RandomizePosition, InitializeSpawnedObject));
        }

        protected override bool InitializeSpawnedObject(GameObject spawnedObject)
        {
            if (spawnedObject.GetComponent<Rigidbody>() != null)
            {
                PhysicsObject temp = spawnedObject.AddComponent<PhysicsObject>();
                OnComplete += temp.InitPhysics;
            }
            return true;
        }

        #region Texture randomizator

        protected override bool RandomizePosition(PositionCheck positionCheck)
        {
            float minX = 0f;
            float maxX = 100f;
            float minY = 0f;
            float maxY = 100f;

            Vector3 randomPosition = new Vector3(_random.Range(minX, maxX), 0, _random.Range(minY, maxY));

            if (TextureChecker.GetActiveTerrainTextureIdx(randomPosition) != allowedTerraniIndex)
                return false;

            RaycastHit hit;
            Physics.Raycast(new Vector3(randomPosition.x, 100, randomPosition.z), -Vector3.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"));

            randomPosition.y = hit.point.y;
            Quaternion normalizedRotation = AlignToTerrain(terrain, randomPosition, 20);

            positionCheck.Set(randomPosition, normalizedRotation);
            return true;
        }

        private Quaternion AlignToTerrain(Terrain terrain, Vector3 randomPosition, float maxAngle, bool checkAngle = true)
        {
            Vector3 normalizedRotationAtPoint = terrain.terrainData.GetInterpolatedNormal(randomPosition.x / terrain.terrainData.size.x, randomPosition.z / terrain.terrainData.size.z);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normalizedRotationAtPoint);

            if (checkAngle && Quaternion.Angle(rotation, Quaternion.identity) > maxAngle)
                rotation = Quaternion.AngleAxis(maxAngle, Vector3.up);

            return rotation * Quaternion.Euler(Vector3.up * _random.Range(-180f, 180f));
        }

        #endregion
    }
}
