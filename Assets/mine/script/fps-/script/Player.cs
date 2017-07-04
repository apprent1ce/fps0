using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//handle input,caculate score,set camerig;
//its vr player

public class Player : NetworkBehaviour
{
    //vr
    public GameObject vrCameraRig;

    public GameObject weaponGameObjectProfab;
    private GameObject vrCameraRigInstance;

    // 游戏
    public int score;

    public MyBaseWeapon myBaseWeapon;

    public override void OnStartLocalPlayer()
    {
        if (!isClient)
            return;
        // delete main camera
        DestroyImmediate(Camera.main.gameObject);

        // create camera rig and attach player model to it
        vrCameraRigInstance = (GameObject)Instantiate(
            vrCameraRig,
            transform.position,
            transform.rotation);

        Transform bodyOfVrPlayer = transform.Find("VRPlayerBody");
        if (bodyOfVrPlayer != null)
            bodyOfVrPlayer.parent = null;

        GameObject head = vrCameraRigInstance.GetComponentInChildren<SteamVR_Camera>().gameObject;
        transform.parent = head.transform;
        transform.localPosition = new Vector3(0f, -0.03f, -0.06f);

        TryDetectControllers();
    }

    private void TryDetectControllers()
    {
        print("trying controllers");
        var controller = vrCameraRigInstance.GetComponentInChildren<SteamVR_TrackedObject>();
        if (controller != null)
        {
            CmdSpawnHands(netId);
        }
        else
        {
            Invoke("TryDetectControllers", 2f);
        }
    }

    [Command]
    private void CmdSpawnHands(NetworkInstanceId playerId)
    {
        print("spawning weapon");
        // instantiate controller
        // tell the server, to spawn  controller model prefabs on all clients
        // give the local player authority over the newly created controller models
        GameObject weaponGameObject = Instantiate(weaponGameObjectProfab);

        var weapon = weaponGameObject.GetComponent<MyBaseWeapon>();

        weapon.ownerId = playerId;

        NetworkServer.SpawnWithClientAuthority(weaponGameObject, base.connectionToClient);
    }

    // Use this for initialization
}