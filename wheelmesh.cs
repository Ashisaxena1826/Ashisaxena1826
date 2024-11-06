using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelmesh : MonoBehaviour
{
    [Header("wheel meshes")]
    public Transform wm_frontleft;
    public Transform wm_frontright;
    public Transform wm_backleft;
    public Transform wm_backright;

    [Header("wheel collider")]
    public WheelCollider wc_frontleft;
    public WheelCollider wc_frontright;
    public WheelCollider wc_backright;
    public WheelCollider wc_backleft;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateWheelOrientation(wc_frontright, wm_frontright);
        UpdateWheelOrientation(wc_frontleft, wm_frontleft);
        UpdateWheelOrientation(wc_backleft, wm_backleft);
        UpdateWheelOrientation(wc_backright, wm_backright);
    }
    void Update()
    {

    }
    void UpdateWheelOrientation(WheelCollider wc, Transform wm)
    {
        Vector3 tempLocation;
        Quaternion tempRotation;

        wc.GetWorldPose(out tempLocation, out tempRotation);
        wm.position = tempLocation;
        wm.rotation = tempRotation;
    }

}
