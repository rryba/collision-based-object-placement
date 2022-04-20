// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class CustomPhysicsObject : PhysicsObject
    {
        private bool addCustomGravity;
        private Transform gravityCenter;

        protected override void Awake()
        {
            base.Awake();
            addCustomGravity = false;
        }

        public void Initialize(Transform planet)
        {
            gravityCenter = planet;
        }

        public override void InitPhysics()
        {
            base.InitPhysics();
            addCustomGravity = true;
        }

        private void FixedUpdate()
        {
            if (!addCustomGravity)
                return;

            rb.AddForce((gravityCenter.position - transform.position).normalized * 9.8f);
        }

        void OnDisable()
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
