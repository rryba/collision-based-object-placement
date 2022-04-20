// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class CheckForCollisions : MonoBehaviour
    {
        protected PositionCheck managerPositionChecks;

        protected virtual void Awake()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                collider.isTrigger = true;
            }

            Rigidbody rb;
            if (GetComponent<Rigidbody>() == null)
                rb = gameObject.AddComponent<Rigidbody>();
            else
                rb = GetComponent<Rigidbody>();

            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public virtual IEnumerator IsColliding(PositionCheck positionChecks)
        {
            managerPositionChecks = positionChecks;

            transform.SetPositionAndRotation(positionChecks.randomPosition, positionChecks.normalizedRotation);
            transform.position += Vector3.zero;

            yield return new WaitForFixedUpdate();

            managerPositionChecks = null;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (managerPositionChecks != null)
                managerPositionChecks.PositionIsAvailable(false);
        }
    }
}