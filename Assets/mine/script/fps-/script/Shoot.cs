using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//主射击类与变换颜色
public class Shoot : NetworkBehaviour
{
    private Animator _animator;
    public GameObject muzzle;
    public GameObject Fire;
    public AudioClip ShotSound;
    public AudioClip ReloadSound;
    private AudioSource _audioSource;
    public GameObject ShellPrefab;
    public ParticleSystem FireEffect;

    public Transform target;  //current target;

    private void pointerIn(object sender, PointerEventArgs e)
    {
        // GameObject.CreatePrimitive( PrimitiveType. )
    }

    private void pointerOut(object sender, PointerEventArgs e)
    {
    }

    // Use this for initialization
    private void Start()
    {
        GetComponentInChildren<SteamVR_LaserPointer>().PointerIn += new PointerEventHandler(pointerIn);
        GetComponentInChildren<SteamVR_LaserPointer>().PointerIn += new PointerEventHandler(pointerOut);

        _animator = GetComponent<Animator>();
        _audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.anyKey)
            {
                transform.position += new Vector3(0, 1f, 1f);
                Cmda(gameObject);
            }
            shoot();
            shooot();
            Reload();
        }
    }

    [ClientRpc]
    private void aa(string a)
    {
        print(a);
    }

    [Command]
    private void Cmda(GameObject a)
    {
        aa(myMain.instance.gameObject.name);

        myMain.instance.Cmdc();
    }

    private void shoot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("Shoot", true);
        }
        else
        {
            _animator.SetBool("Shoot", false);
        }
    }

    private void shooot()
    {
        if (Input.GetMouseButton(0))
        {
            _animator.SetBool("Shooot", true);
        }
        else
        {
            _animator.SetBool("Shooot", false);
        }
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetBool("Reload", true);
            _audioSource.clip = ReloadSound;
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            //ReloadSound.Play();
        }
        else
        {
            _animator.SetBool("Reload", false);
        }
    }

    private void EndFire()
    {
        GameObject a = Instantiate(Fire, muzzle.transform.position, muzzle.transform.rotation);
        a.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * 100000);

        _audioSource.clip = ShotSound;
        _audioSource.Play();

        // FireEffect.Play();
        EjectShell();
    }

    protected virtual void EjectShell()
    {
        if (this == null)
            return;
        if (ShellPrefab == null)
            return;

        GameObject shell = Instantiate(ShellPrefab, ShellPrefab.transform.position, ShellPrefab.transform.rotation);
        shell.SetActive(true);
        // send it flying
        Rigidbody m_ShellRigidbody = shell.GetComponent<Rigidbody>();
        if (m_ShellRigidbody == null)
        {
            m_ShellRigidbody.gameObject.AddComponent<Rigidbody>();
        }

        Vector3 r = Vector3.Cross(muzzle.transform.position, new Vector3(Random.value, -1, Random.value));

        Vector3 force = 0.1f * r;   // we have no shell eject object: use old logic

        // toss the shell
        m_ShellRigidbody.AddForce(force, ForceMode.Impulse);

        m_ShellRigidbody.AddForce(gameObject.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);

        // add random spin if user-defined
    }
}