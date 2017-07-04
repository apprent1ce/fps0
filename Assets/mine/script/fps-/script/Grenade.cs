using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : BaseDamage
{
    public float redius;
    public float minDamage;

    private float k;//demage=damage/(kr+1)

    private void Start()
    {
        k = ((damage / minDamage) - 1) / redius;
        Invoke("OnExplotion", 1f);
    }

    private Collision a;

    private void OnExplotion()
    {
        foreach (var monster in myMain.instance.monsters)
        {
            float r = Vector3.Distance(monster.transform.position, transform.position);
            //   print(r);
            if (r < redius)
            {
                DamageInfo info = new DamageInfo(this);
                info.damage = damage / (k * r + 1);

                info.movement = GetMovement(monster.transform.position, info.damage);

                CmdTryAttack(monster, info);
            }
        }
        Invoke("OnExplotion", 1f);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}