using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour
{
    static public GameObject prefab;

    virtual public GameObject spawn(GameObject g)
    {
        GameObject s = Instantiate(g);
        return s;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}