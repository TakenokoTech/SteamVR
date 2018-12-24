using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImplAveter : AveterMotion {

	public GameObject eyeCamera;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject leftLeg;
	public GameObject rightLeg;

	private GameObject leftLegTarget;
	private GameObject rightLegTarget;

	private JsonEntity entity = new JsonEntity ();
	private Thread thread;

	protected override void OnStart () {

	}

	protected override void OnUpdate () {
		thread = new Thread (DoHeavyProcess);
		thread.Start ();

		try {
			// VrUtil.tracePosition (eyeCamera, headTarget);
			// VrUtil.tracePosition (leftHand, leftHandTarget);
			// VrUtil.tracePosition (rightHand, rightHandTarget);
			VrUtil.tracePosition (leftLeg, leftLegTarget);
			VrUtil.tracePosition (rightLeg, rightLegTarget);

			eyeCamera.transform.position = entity.headVec;
			eyeCamera.transform.rotation = entity.headQua;
			VrUtil.tracePosition (eyeCamera, headTarget);

			leftHand.transform.position = entity.leftHandVec;
			leftHand.transform.rotation = entity.leftHandQua;
			VrUtil.tracePosition (leftHand, leftHandTarget);

			rightHand.transform.position = entity.rightHandVec;
			rightHand.transform.rotation = entity.rightHandQua;
			VrUtil.tracePosition (rightHand, rightHandTarget);

		} catch (NullReferenceException e) {
			Debug.Log (e);
		}
	}

	override protected void CreateTempObj () {
		base.CreateTempObj ();

		leftLegTarget = new GameObject ();
		leftLegTarget.name = "leftLegTarget";
		leftLegTarget.transform.position = new Vector3 (-0.1f, 0f, 0.2f);
		leftLegTarget.transform.parent = transform;

		rightLegTarget = new GameObject ();
		rightLegTarget.name = "rightLegTarget";
		rightLegTarget.transform.position = new Vector3 (0.1f, 0f, 0.2f);
		rightLegTarget.transform.parent = transform;
	}

	override protected void SetupVRIK () {
		base.SetupVRIK ();
		var vrIK = avatar.GetComponent<VRIK> ();

		vrIK.solver.leftLeg.stretchCurve = new AnimationCurve ();
		vrIK.solver.leftLeg.target = leftLegTarget.transform;
		vrIK.solver.leftLeg.positionWeight = 1F;

		vrIK.solver.rightLeg.stretchCurve = new AnimationCurve ();
		vrIK.solver.rightLeg.target = rightLegTarget.transform;
		vrIK.solver.rightLeg.positionWeight = 1F;
	}

	private void DoHeavyProcess () {
		string jsonStr = apiprotocol.GrpcApi.Get ();
		entity = JsonUtility.FromJson<JsonEntity> (jsonStr);
		Debug.Log (entity.headVec.ToString ());
	}
}