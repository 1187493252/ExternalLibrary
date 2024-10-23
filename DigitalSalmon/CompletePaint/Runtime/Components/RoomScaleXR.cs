using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RoomScaleXR : MonoBehaviour
{
	protected void Start() { XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale); }
}
