using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class myMain : NetworkBehaviour
{
    public GameObject prefabmonster, prefabg;

    private List<Vector3> preTransform;

    static public myMain instance;

    public GameObject door;

    public List<GameObject> monsters;

    public List<GameObject> players;
    // Use this for initialization

    public void Awake()
    {
        instance = this;
        monsters = new List<GameObject>();
        //   players = new List<GameObject>();
        addpreTransform();
    }

    public void iniPlayer()
    {
    }

    public override void OnStartServer()
    {
    }

    //  [ServerCallback]
    private void addpreTransform()
    {
        preTransform = new List<Vector3>();
        var ps = FindObjectsOfType<monsterPoint>();
        foreach (var p in ps)
        {
            preTransform.Add(p.transform.position);
        }
    }

    // Update is called once per frame
    [ClientCallback]
    private void Update()
    {
    }

    //   [Command]
    public void Cmdc()
    {
        GameObject monster = Instantiate(prefabmonster);
        monster.transform.position = preTransform[0];
        NetworkServer.Spawn(monster);
        // Destroy(monster);
    }

    public void Cmda()
    {
        GameObject g = Instantiate(prefabg);
        g.transform.position = preTransform[0];
        NetworkServer.Spawn(g);
        // Destroy(monster);
    }

    private IEnumerator spawnMonster(GameObject premonster, int time, int n)
    {
        for (int i = 0; i < n; i++)
        {
            GameObject monster = Instantiate(premonster);
            monster.transform.position = preTransform[i % preTransform.Count];

            NetworkServer.Spawn(monster);

            yield return new WaitForSeconds((float)time / n);
        }
    }
}