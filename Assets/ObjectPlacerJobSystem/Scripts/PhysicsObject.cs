// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class PhysicsObject : MonoBehaviour
    {

        protected Rigidbody rb;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public virtual void InitPhysics()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}