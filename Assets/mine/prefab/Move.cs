using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// rifle on 76

public class Move : NetworkBehaviour
{
    private MouseLook mouseLook;

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(-1, 1f, -1f);
        Camera.main.transform.localRotation = Quaternion.Euler(6.31f, 0, 0);

        mouseLook = new MouseLook();
        mouseLook.Init(transform, Camera.main.transform);
    }

    private void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        transform.Translate(x, 0, z);

        mouseLook.LookRotation(transform, Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            myMain.instance.Cmdc();
        }
    }
}