using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BaseDamage
{
    public Vector3 vPerFrame;
    public Vector3 g;
    private bool free = true;

    // Use this for initialization
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (free)
        {
            transform.position += vPerFrame;
            vPerFrame += -g / 60;
            transform.forward = vPerFrame;
        }

        RaycastHit info;
        if (Physics.Raycast(transform.position, vPerFrame, out info, 0.1f))
        {
            CmdTryAttack(info.transform.gameObject, new DamageInfo(this));

            transform.position = info.point;
            transform.parent = info.transform;

            Destroy(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}