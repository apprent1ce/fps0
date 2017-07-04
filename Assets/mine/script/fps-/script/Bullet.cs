using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class baseBullet : MonoBehaviour
{
    public int damage;
    public float might;
    public float lifeTime = 10f;

    private void Awake()
    {
    }

    private void DestroyMe()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        NetworkServer.Destroy(gameObject);
        var health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            //health.CmdOnDamage(damage, transform.forward * might);
        }
    }
}