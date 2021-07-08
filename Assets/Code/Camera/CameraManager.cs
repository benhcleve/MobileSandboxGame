using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum State { Default, Behind }
    public State state = State.Default;
    public Camera cam;
    public Transform followTransform;
    public CameraOrbit cameraOrbit;

    private static CameraManager _instance;
    public static CameraManager instance { get { return _instance; } }

    public void Awake() => CreateInstance();
    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Update()
    {
        ManageState();
    }

    private void LateUpdate()
    {
        if (state == State.Default)
            transform.position = followTransform.position;
    }

    void ManageState()
    {
        if (state == State.Default && !cameraOrbit.enabled)
        {
            cameraOrbit.enabled = true;
        }
        else if (state == State.Behind)
        {
            CameraBehind();
            if (cameraOrbit.enabled)
                cameraOrbit.enabled = false;
        }
    }

    public Vector3 behindOffset;
    void CameraBehind()
    {
        Transform player = PlayerMovement.instance.transform;
        cam.transform.position = player.position + (-player.forward * behindOffset.z) + ((player.right * behindOffset.x)) + new Vector3(0, behindOffset.y, 0);
        cam.transform.rotation = player.rotation;
    }
}
