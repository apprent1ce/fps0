using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkTransform))]
public class myBaseMonster : NetworkBehaviour
{
    [SyncVar]
    public int hp;

    //all of the monster base on this
    private void Start()
    {
    }

    public myBaseMonster clone()
    {
        return new myBaseMonster();
    }

    // Update is called once per frame

    [ServerCallback]
    private void Update()
    {
    }

    [Command]
    private void CmdDamagemy(int bullet)
    {
        hp -= bullet;
        if (hp <= 0)
        {
            Die();
        }
        else
        {
            Die();
        }
    }

    [Server]
    private void Chase()
    {
    }

    [ClientCallback]
    private void Die()
    { }

    [ServerCallback]
    private void Attack()
    {
    }
}