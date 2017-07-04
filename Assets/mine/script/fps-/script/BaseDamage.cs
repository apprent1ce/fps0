using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// the information of all damage，all damage online base on this；
// attach on object with damage like  bullet or knife
public enum DamageType
{
}//可设置攻击类型

public struct DamageInfo
{
    public float damage;
    public float time;

    public Vector3 movement;
    public DamageType damageType;

    public DamageInfo(BaseDamage a)
    {
        damage = a.damage;
        time = a.time;
        movement = a.movement;
        damageType = a.damageType;
    }
}

public class BaseDamage : NetworkBehaviour
{
    public float might; //movement always propotional to damage
    public float damage;

    public float time;
    public Vector3 movement;
    public DamageType damageType;
    public GameObject player;

    public int weaknessTimes = 2;

    private void Awake()
    {
    }

    [Command]
    virtual public void CmdTryAttack(GameObject target, DamageInfo info)
    {
        print(this.gameObject + " CmdTryAttack   " + target);
        var weakness = target.GetComponent<Weakness>();
        var health = target.GetComponent<Health>();

        if (weakness != null)
        {
            info.damage *= weaknessTimes;
            target.GetComponentInParent<Health>().OnDamage(info);
        }
        else if (health != null)
        {
            health.OnDamage(info);
        }
    }

    private void DestroyMe()
    {
        NetworkServer.Destroy(gameObject);
    }

    public Vector3 GetMovement(Vector3 p, float damage_)
    {
        return might * damage_ * (p - transform.position);
    }

    public Vector3 GetMovement(Vector3 p)
    {
        return might * damage * (transform.position - p);
    }
}