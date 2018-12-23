using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImplAveter : AveterMotion {

	public GameObject eyeCamera;
	public GameObject handLeft;
	public GameObject handRight;

	protected override void OnStart () { }

	protected override void OnUpdate () {
		VrUtil.tracePosition (eyeCamera, headTarget);
		VrUtil.tracePosition (handLeft, leftHandTarget);
		VrUtil.tracePosition (handRight, rightHandTarget);
	}

}