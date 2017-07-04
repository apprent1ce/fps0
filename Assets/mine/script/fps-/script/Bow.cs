using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bow : MyBaseWeapon
{
    // 60 pointer ,d position/dt=d v
    //d v/dt=g
    // after t>60 we set g=0
    //so we only need the ini v and g to simulate the laser
    // we set dt=0.16s or

    public float g = 10;

    public float maxV = 20;
    public float maxDamage;
    private float v, damage;

    public float intervalTime = 2;

    public GameObject preArrow;

    private bool onContinue;

    private void Awake()
    {
    }

    public void SetPointer()
    {
        Vector3 v3 = v / 60 * transform.forward;

        pointerPositions[0] = transform.position;

        for (int i = 1; i < 60; i++)
        {
            pointerPositions[i] = pointerPositions[i - 1] + v3;
            v3.y += -g / 60;
        }
        pointerRenderer.SetPositions(pointerPositions);
    }

    [Command]
    private void CmdFire()
    {
        GameObject ar = Instantiate(preArrow);

        ar.transform.position = fireTransform.position;
        ar.transform.rotation = fireTransform.rotation;

        ar.GetComponent<Arrow>().vPerFrame = v / 60 * fireTransform.forward;

        ar.GetComponent<Arrow>().damage = damage * maxDamage;

        ar.GetComponent<Arrow>().g = new Vector3(0, g, 0);

        NetworkServer.Spawn(ar);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        iniPointer(60);
        damage = 0;
        v = maxV / 4;
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))// fire E/grip
        {
            print(3);
            StartCoroutine(shoot());
        }
    }

    //Shoot

    // Use this for initialization

    // Update is called once per frame
    public override void Update()
    {
        if (!hasAuthority || !active)
        {
            return;
        }
        SetPointer();
        HandleInput();
    }

    private IEnumerator shoot()
    {
        damage = 0;
        v = maxV / 4;
        print(0);
        while (Input.GetKey(KeyCode.E))
        {
            pointerRenderActive = true;
            if (damage < maxDamage)
            {
                v += maxV / (80 * intervalTime);
                damage += maxDamage / (80 * intervalTime);
            }

            yield return new WaitForFixedUpdate();
        }
        pointerRenderActive = false;

        CmdFire();
        damage = 0;
        v = maxV / 4;
        yield return null;
    }
}