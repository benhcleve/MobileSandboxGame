using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 10f;

    public float OrbitSensitivity = .2f;
    public float ZoomSensitivity = 2f;
    public float OrbitDampening = 10f;
    public float ZoomDampening = 6f;

    public bool CameraDisabled = false;


    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
    }


    void LateUpdate()
    {
        CameraRotation();
        CameraZoom();

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ZoomDampening));
        }
    }

    void CameraRotation()
    {
        //DESKTOP
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _LocalRotation.x += Input.GetAxis("Mouse X") * (OrbitSensitivity * 5); //Multiply for desktop;
            _LocalRotation.y -= Input.GetAxis("Mouse Y") * (OrbitSensitivity * 5); //Multiply for desktop;

            //Clamp the y Rotation to horizon and not flipping over at the top
            if (_LocalRotation.y < 0f)
                _LocalRotation.y = 0f;
            else if (_LocalRotation.y > 90f)
                _LocalRotation.y = 90f;
        }
#endif
        //MOBILE

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            _LocalRotation.x += (Input.touches[0].deltaPosition.x / 10) * OrbitSensitivity; //Divided by 2 to reduce speed
            _LocalRotation.y -= (Input.touches[0].deltaPosition.y / 10) * OrbitSensitivity; //Divided by 2 to reduce speed

            //Clamp the y Rotation to horizon and not flipping over at the top
            if (_LocalRotation.y < 25f)
                _LocalRotation.y = 25f;
            else if (_LocalRotation.y > 90f)
                _LocalRotation.y = 90f;
        }
#endif

    }



    void CameraZoom()
    {
        //Zooming Input from our Mouse Scroll Wheel
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float zoomAmount = Input.GetAxis("Mouse ScrollWheel") * (ZoomSensitivity * 5); //Multiply for desktop

            zoomAmount *= (this._CameraDistance * 0.3f);

            this._CameraDistance += zoomAmount * -1f;

            this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);


        }
#endif

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = (currentMagnitude - prevMagnitude);

            float zoomAmount = (difference / 100) * ZoomSensitivity;//Divided because it moves to fast

            zoomAmount *= (this._CameraDistance * 0.3f);

            this._CameraDistance += zoomAmount * -1f;

            this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 15f);
        }
    }
#endif


}