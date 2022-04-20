// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    public class CheckForCollisions2D : CheckForCollisions
    {
        private Rigidbody2D rb;

        protected override void Awake()
        {
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

            foreach (Collider2D collider in colliders)
            {
                collider.isTrigger = true;
            }

            if (GetComponent<Rigidbody2D>() == null)
                rb = gameObject.AddComponent<Rigidbody2D>();
            else
                rb = GetComponent<Rigidbody2D>();

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.gravityScale = 0f;
        }

        public override IEnumerator IsColliding(PositionCheck positionChecks)
        {
            managerPositionChecks = positionChecks;

            transform.SetPositionAndRotation(positionChecks.randomPosition, positionChecks.normalizedRotation);
            rb.position += Vector2.zero;

            yield return new WaitForFixedUpdate();

            managerPositionChecks = null;
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (managerPositionChecks != null)
                managerPositionChecks.PositionIsAvailable(false);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (managerPositionChecks != null)
                managerPositionChecks.PositionIsAvailable(false);
        }
    }
}