using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OvrAvatarHand : MonoBehaviour
{
    private void Awake()
    {
       SphereCollider collider = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
       collider.radius = .4f;
       collider.isTrigger = true;
       Rigidbody rigid = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
       rigid.useGravity = false;
    }
}
