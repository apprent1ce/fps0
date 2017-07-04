using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// all weapon base on it

public class MyBaseWeapon : NetworkBehaviour
{
    [SyncVar]
    public NetworkInstanceId ownerId;

    private GameObject trackedController;
    private SteamVR_Controller.Device steamDevice;
    private Player localPlayer;

    public Transform fireTransform;
    public LineRenderer pointerRenderer;

    public bool pointerRenderActive;
    public bool active;

    public Vector3[] pointerPositions;

    public virtual void iniPointer(int n)
    {
        pointerRenderer = GetComponent<LineRenderer>();
        pointerRenderer.material = new Material(Shader.Find("Sprites/Default"));
        // Set some positions
        pointerPositions = new Vector3[n];
        pointerRenderer.positionCount = n;
    }

    private bool a;

    public override void OnStartAuthority()
    {
        // attach the controller model to the tracked controller object on the local client

        //if (hasAuthority)
        //{
        //    trackedController = GameObject.Find(string.Format("Controller ({0})", side.ToString("G").ToLowerInvariant()));

        //    localPlayer = ClientScene.FindLocalObject(ownerId).GetComponent<Player>();

        //    steamDevice = SteamVR_Controller.Input((int)trackedController.GetComponent<SteamVR_TrackedObject>().index);
        //}
        fireTransform = transform;
        active = true;
        print("ONstartauthority, baseweapon");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!hasAuthority || !active)
        {
            return;
        }
    }
}