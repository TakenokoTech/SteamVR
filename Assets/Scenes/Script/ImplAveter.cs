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

	public Quaternion quaR = new Quaternion (0, 90, 180, -180);
	public Quaternion quaL = new Quaternion (0, 90, 180, 180);

	private GameObject leftLegTarget;
	private GameObject rightLegTarget;

	private JsonEntity entity = null;
	private Thread thread;

	protected override void OnStart () {

	}

	protected override void OnUpdate () {
		thread = new Thread (DoHeavyProcess);
		thread.Start ();

		VrUtil.tracePosition (eyeCamera, headTarget);
		VrUtil.tracePosition (leftHand, leftHandTarget);
		VrUtil.tracePosition (rightHand, rightHandTarget);
		VrUtil.tracePosition (leftLeg, leftLegTarget);
		VrUtil.tracePosition (rightLeg, rightLegTarget);

		if (entity == null) {
			return;
		}

		try {
			eyeCamera.transform.position = entity.headVec;
			eyeCamera.transform.rotation = entity.headQua;
			VrUtil.tracePosition (eyeCamera, headTarget);

			leftHand.transform.position = entity.leftHandVec;
			leftHand.transform.rotation = entity.leftHandQua * quaL;
			VrUtil.tracePosition (leftHand, leftHandTarget);

			rightHand.transform.position = entity.rightHandVec;
			rightHand.transform.rotation = entity.rightHandQua * quaR;
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

		TwistRelaxer leftTwistRelaxer = vrIK.references.leftForearm.gameObject.AddComponent<TwistRelaxer> ();
		leftTwistRelaxer.weight = 0.4F;
		leftTwistRelaxer.parentChildCrossfade = 1.0F;
		TwistRelaxer rightTwistRelaxer = vrIK.references.rightForearm.gameObject.AddComponent<TwistRelaxer> ();
		rightTwistRelaxer.weight = 0.4F;
		rightTwistRelaxer.parentChildCrossfade = 1.0F;
	}

	private void DoHeavyProcess () {
		string jsonStr = apiprotocol.GrpcApi.Get ();
		if (jsonStr != null) {
			entity = JsonUtility.FromJson<JsonEntity> (jsonStr);
			Debug.Log (entity.headVec.ToString ());
		}
	}
}