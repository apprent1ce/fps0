using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4A1 : MyBaseWeapon
{
    private BaseDamage baseDamage;

    public int maxBulletNumber;
    public int currentBulletNumber;

    public float intervalTime = 2;
    private float offset = 0.005f;
    private Vector3 h;

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("       if (Input.GetKeyDown(KeyCode.Space)) ");
            StartCoroutine(shoot());
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            print("         if (Input.GetKeyDown(KeyCode.Z))  ");
            Reload();
        }
    }

    private void Reload()
    {
        print("1");
        if (currentBulletNumber == maxBulletNumber)
        { return; }
        print("2");
        currentBulletNumber = maxBulletNumber;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        baseDamage = GetComponent<BaseDamage>();
        iniPointer(2);
    }

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

    public void SetPointer()
    {
        pointerPositions[0] = (transform.position);
        pointerPositions[1] = (transform.position) + 10 * (transform.forward + h);
        pointerRenderer.SetPositions(pointerPositions);
    }

    private IEnumerator shoot()
    {
        float n = 1;
        while (Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForSeconds(0.1f);
            h = Vector3.Cross(transform.forward, Random.rotation * new Vector3(1, 0, 0)) * n * offset;
            if (n < 20)
            {
                n = 1.2f * n + 0.5f;
            }
            print("n = 1.2f * n + 0.5f=");
            if (currentBulletNumber > 0)
            {
                RaycastHit info;
                if (Physics.Raycast(transform.position, transform.forward + h, out info))
                {
                    DamageInfo damageInfo = new DamageInfo(baseDamage);
                    damageInfo.movement = (transform.forward + h) * baseDamage.might * baseDamage.damage;
                    print(" (transform.forward + h)" + (transform.forward + h));
                    print("baseDamage.might " + baseDamage.might);
                    print("baseDamage.damage" + baseDamage.damage);

                    baseDamage.CmdTryAttack(info.transform.gameObject, damageInfo);

                    print("damageInfo.movement" + damageInfo.movement);
                }
                currentBulletNumber--;
                print("0");
            }
            else
            {
                Reload();
            }
        }
        h = Vector3.zero;
        yield return null;
    }
}