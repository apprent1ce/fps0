using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

//整个怪物类
public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;
    public bool destroyOnDeath;

    private Animator _animator;

    private NavMeshAgent _navmeshagent;
    private bool hasTarget;

    public float noticeDistance = 2f;
    public GameObject target;

    private float Attack_distance = 2f;
    public float ZmobieDamage = 2.0f;

    public GameObject door;
    public List<GameObject> players;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    private void OnChangeHealth(int currentHealth)
    {
        GetComponentInChildren<TextMesh>().text = currentHealth.ToString();
    }

    private void Awake()
    {
        door = myMain.instance.door;
        players = myMain.instance.players;
        target = myMain.instance.door;

        myMain.instance.monsters.Add(this.gameObject);

        //     _animator = GetComponent<Animator>();
        _navmeshagent = GetComponent<NavMeshAgent>();

        StartCoroutine(changeTarget());
    }

    public void OnDamage(DamageInfo damage) //伤害调用该函数
    {
        currentHealth -= (int)damage.damage;

        StartCoroutine(Onforce(damage));
        if (currentHealth <= 0)
        {
            RpcOnDeath();
        }
        else
        {
            RpcOnDamageAnimator();
        }
    }

    public IEnumerator Onforce(DamageInfo damage)
    {
        Vector3 movePerFixtime = new Vector3();
        movePerFixtime = _navmeshagent.velocity * (0.016f);

        _navmeshagent.isStopped = true;

        int n = (int)(60 * damage.time);

        movePerFixtime.x += damage.movement.x * (1 / (float)n);
        movePerFixtime.z += damage.movement.z * (1 / (float)n);
        for (int i = 0; i < n; i++)
        {
            transform.position += movePerFixtime;
            yield return new WaitForFixedUpdate();
        }
        while (!_navmeshagent.isOnNavMesh)
        {
            yield return new WaitForFixedUpdate();
        }
        _navmeshagent.isStopped = false;
        yield return null;
    }

    [ClientRpc]
    public void RpcOnDeath()//播放动画的代码
    { }

    [ClientRpc]
    public void RpcOnDamageAnimator()//播放动画的代码
    {
    }

    private IEnumerator changeTarget()
    {
        while (true)
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) > noticeDistance)
            {
                foreach (GameObject p in players)
                {
                    if (Vector3.Distance(p.transform.position, this.transform.position) < noticeDistance)
                    {
                        target = p;//change target
                        break;
                    }
                }
            }

            if (Vector3.Distance(target.transform.position, this.transform.position) < Attack_distance)
            {
                //   _animator.SetBool("Attack", true);
            }
            else
            {
                // _animator.SetBool("Attack", false);
                _navmeshagent.SetDestination(target.transform.position);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}