using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// rifle on 76

public class Shooter : BaseDamage
{
    private MouseLook mouseLook;

    private LineRenderer Parabola;

    private LineRenderer sightBead;

    public GameObject sightBeadGameObject, p;

    private Vector3[] Parabolas;//20

    private Vector3[] sightBeads;//20

    private float offset = 0.005f;

    private void SetParabolaRender(Vector3 v0, Vector3 f0, Vector3 v1, int n)
    {
        Vector3 k0 = Vector3.Dot((v1 - v0), f0) * f0 / n;
        Vector3 k1 = ((v1 - v0) - Vector3.Dot((v1 - v0), f0) * f0) / (n * n);

        for (int i = 0; i < n; i++)
        {
            Parabolas[i] = i * i * k1 + i * k0 + v0;
        }

        Parabola.SetPositions(Parabolas);
    }

    public override void OnStartLocalPlayer()
    {
        Parabola = GetComponent<LineRenderer>();
        Parabola.material = new Material(Shader.Find("Sprites/Default"));
        Parabolas = new Vector3[20];
        Parabola.positionCount = 20;

        sightBead = sightBeadGameObject.GetComponent<LineRenderer>();
        sightBead.material = new Material(Shader.Find("Sprites/Default"));
        sightBeads = new Vector3[20];
        sightBead.positionCount = 20;

        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0, 1.33f, -0.69f);
        Camera.main.transform.localRotation = Quaternion.Euler(6.31f, 0, 0);

        mouseLook = new MouseLook();
        mouseLook.Init(transform, Camera.main.transform);
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        transform.Translate(x, 0, z);

        mouseLook.LookRotation(transform, Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        //SetParabolaRender(transform.position, transform.forward, new Vector3(0, 0, 0), 20);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            myMain.instance.Cmdc();
        }
    }
}