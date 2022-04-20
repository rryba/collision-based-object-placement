// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class PositionCheck
    {
        public bool isPositionAvailable { get; private set; }
        public Vector3 randomPosition { get; private set; }
        public Quaternion normalizedRotation { get; private set; }

        public PositionCheck(Vector3 position, Quaternion rotation)
        {
            this.randomPosition = position;
            this.normalizedRotation = rotation;

            this.isPositionAvailable = true;
        }

        public void Set(Vector3 position, Quaternion rotation)
        {
            this.randomPosition = position;
            this.normalizedRotation = rotation;
        }

        public void PositionIsAvailable(bool status)
        {
            isPositionAvailable = status;
        }

        public void PossitionAllowed()
        {
            isPositionAvailable = true;
        }

        public void Reset()
        {
            isPositionAvailable = true;

            randomPosition = Vector3.zero;
            normalizedRotation = Quaternion.identity;
        }
    }
}
