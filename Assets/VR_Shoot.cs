using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Shoot : MonoBehaviour
{
    private Animator _animator;
    public GameObject muzzle;
    public GameObject Fire;

    public GameObject ShellPrefab;
    public AudioClip ShotSound;
    public AudioClip ReloadSound;
    private AudioSource _audioSource;
    private SteamVR_TrackedObject tracked;

    private Vector3 lastTimePositon;//v
    private Vector3 v;
    private float updateTime;

    // Use this for initialization
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = this.gameObject.AddComponent<AudioSource>();
        tracked = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        v = (transform.position - lastTimePositon) / Time.unscaledDeltaTime;
        shoot();
        Reload();
        lastTimePositon = transform.position;
    }

    private void shoot()
    {
        var device = SteamVR_Controller.Input((int)tracked.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) || device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            _animator.SetBool("Shoot", true);
        }
        else
        {
            _animator.SetBool("Shoot", false);
        }
    }

    private void Reload()
    {
        var device = SteamVR_Controller.Input((int)tracked.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
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

        Vector3 r = Vector3.Cross(muzzle.transform.position, new Vector3(Random.Range(0, 1), 1, Random.Range(0, 1)));
        print(r.x);
        Vector3 force = 0.5f * r;   // we have no shell eject object: use old logic

        // toss the shell
        m_ShellRigidbody.AddForce(force, ForceMode.Impulse);

        m_ShellRigidbody.AddForce(v, ForceMode.VelocityChange);

        // add random spin if user-defined
    }
}