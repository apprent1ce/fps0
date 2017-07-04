using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//public struct ClickedEventArgs
//{
//    public uint controllerIndex;
//    public uint flags;
//    public float padX, padY;
//}

public class playerInfo : NetworkBehaviour
{
    //计算分数，大招，子弹
    [SyncVar]
    public int health;

    public GameObject shooter;

    // Use this for initialization
    private void Start()
    {
        //  GetComponentInChildren<SteamVR_TrackedController>().TriggerClicked += new ClickedEventHandler(fire1);
    }

    private void fire1(object sender, ClickedEventArgs e)
    { }

    // Update is called once per frame
    private void Update()
    {
    }
}