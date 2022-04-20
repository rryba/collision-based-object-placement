// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    [Serializable]
    public class PlacedObjectData
    {
        public string prefabName;
        public int prefabIndex;
        public Vector3Serializer position;
        public QuaternionSerializer rotation;

        [NonSerialized] public GameObject referalObject;

        public PlacedObjectData(int prefabIndex, string prefabName, Vector3 position, Quaternion rotation)
        {
            this.prefabIndex = prefabIndex;
            this.prefabName = prefabName;
            this.position = new Vector3Serializer(position);
            this.rotation = new QuaternionSerializer(rotation);
        }

        public void SetReferalObject(GameObject referalObject)
        {
            this.referalObject = referalObject;
        }

        public PlacedObjectData Clone()
        {
            return (PlacedObjectData)this.MemberwiseClone();
        }
    }

    [System.Serializable]
    public struct Vector3Serializer
    {
        public float x;
        public float y;
        public float z;

        public Vector3Serializer(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public void Fill(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public Vector3 V3 => new Vector3(x, y, z);
    }

    [System.Serializable]
    public struct QuaternionSerializer
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public QuaternionSerializer(Quaternion q)
        {
            this.x = q.x;
            this.y = q.y;
            this.z = q.z;
            this.w = q.w;
        }

        public void Fill(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }

        public Quaternion Quaternion => new Quaternion(x, y, z, w);
    }
}