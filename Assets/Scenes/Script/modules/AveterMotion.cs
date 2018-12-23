using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

abstract public class AveterMotion : MonoBehaviour {

	public GameObject avatar;

	private Vector3 quaLeft;
	private Vector3 quaRight;

	protected GameObject headTarget;
	protected GameObject leftHandTarget;
	protected GameObject rightHandTarget;
	protected FingerController fingerController;

	abstract protected void OnStart ();
	abstract protected void OnUpdate ();

	// Use this for initialization
	void Start () {
		CreateTempObj ();
		SetupVRIK ();
		OnStart ();
	}

	// Update is called once per frame
	void Update () {
		OnUpdate ();
	}

	protected virtual void CreateTempObj () {
		// ターゲットを仮作成
		headTarget = new GameObject ();
		headTarget.name = "headTarget";
		headTarget.transform.position = new Vector3 (0, 1.5f, 0);
		headTarget.transform.parent = transform;

		leftHandTarget = new GameObject ();
		leftHandTarget.name = "leftHandTarget";
		leftHandTarget.transform.position = new Vector3 (-0.5f, 0.3f, 0.2f);
		leftHandTarget.transform.parent = transform;

		rightHandTarget = new GameObject ();
		rightHandTarget.name = "rightHandTarget";
		rightHandTarget.transform.position = new Vector3 (0.5f, 0.3f, 0.2f);
		rightHandTarget.transform.parent = transform;
	}

	protected virtual void SetupVRIK () {

		// VRIKを設定
		var vrIK = avatar.AddComponent<VRIK> ();

		// リファレンス紐付け
		vrIK.AutoDetectReferences ();
		vrIK.solver.spine.headTarget = headTarget.transform;

		vrIK.solver.leftArm.stretchCurve = new AnimationCurve ();
		vrIK.solver.leftArm.target = leftHandTarget.transform;
		// vrIK.solver.leftArm.rotationWeight = 0.8f;

		vrIK.solver.rightArm.stretchCurve = new AnimationCurve ();
		vrIK.solver.rightArm.target = rightHandTarget.transform;
		// vrIK.solver.rightArm.rotationWeight = 0.8f;

		// 歩幅の設定            
		// vrIK.solver.locomotion.footDistance = 0.05f;
	}

	protected virtual void Calibration () {
		Vector3 pos1 = headTarget.transform.position;
		pos1.z -= 0.2f; // pos.y = 1.5f;
		headTarget.transform.position = pos1;
		Vector3 pos2 = leftHandTarget.transform.position;
		pos2.z -= 0.2f; // pos2.y = 1.5f;
		leftHandTarget.transform.position = pos2;
		Vector3 pos3 = rightHandTarget.transform.position;
		pos3.z -= 0.2f; // pos2.y = 1.5f;
		rightHandTarget.transform.position = pos3;

		Quaternion rot1 = leftHandTarget.transform.rotation;
		rot1 = rot1 * Quaternion.Euler (quaLeft.x, quaLeft.y, quaLeft.z);
		leftHandTarget.transform.rotation = rot1;

		Quaternion rot2 = rightHandTarget.transform.rotation;
		rot2 = rot2 * Quaternion.Euler (quaRight.x, quaRight.y, quaRight.z);
		rightHandTarget.transform.rotation = rot2;
	}

	private void Controller (AverterParam p) {
		fingerController.FingerRotation (FingerController.FingerType.RightAll, p.rTrigger1);
		fingerController.FingerRotation (FingerController.FingerType.LeftAll, p.lTrigger1);

		Vector3 vec1 = this.transform.position;
		vec1.z += (p.stickL.y > 0) ? p.stickL.y * 0.1f : p.stickL.y * 0.05f;
		vec1.z += (p.stickR.y > 0) ? p.stickR.y * 0.1f : p.stickR.y * 0.05f;
		vec1.x += p.stickL.x * 0.05f;
		vec1.x += p.stickR.x * 0.05f;
		this.transform.position = vec1;

		Vector3 pos = headTarget.transform.position;
		pos.z += p.stickR.y * 0.07f;
		pos.z += p.stickL.y * 0.07f;
		headTarget.transform.position = pos;

		Quaternion quaternion = new Quaternion (0, 0, 0, 0);
		this.transform.rotation = quaternion;
	}
}