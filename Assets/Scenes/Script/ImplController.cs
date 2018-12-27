using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class ImplController : MonoBehaviour {

	public GameObject head;
	public GameObject leftHand;
	public GameObject rightHand;

	public SteamVR_ActionSet ActionSet;
	public SteamVR_Action_Single Trigger;
	// public SteamVR_Action_Boolean Trigger_Touch;
	// public SteamVR_Action_Single Grip;
	// public SteamVR_Action_Vector2 JoyStick;
	// public SteamVR_Action_Boolean JoyStick_Touch;
	// public SteamVR_Action_Boolean JoyStick_Click;
	// public SteamVR_Action_Boolean Button_X;
	// public SteamVR_Action_Boolean Button_X_Touch;
	// public SteamVR_Action_Boolean Button_Y;
	// public SteamVR_Action_Boolean Button_Y_Touch;
	// public SteamVR_Action_Boolean Button_A;
	// public SteamVR_Action_Boolean Button_B;

	public SteamVR_Action_Skeleton SkeletonLeftHand;
	public SteamVR_Action_Skeleton SkeletonRightHand;
	public SteamVR_Action_Boolean Button;

	private Vector3 calibrationVec = new Vector3 (0, 0, 0);
	private SteamVR_Input_Sources InputSources = SteamVR_Input_Sources.Any;
	private JsonEntity entity = new JsonEntity ();
	private Thread thread;

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
		ActionSet.ActivatePrimary (true);
	}

	// Update is called once per frame
	void Update () {
		entity.headVec = head.transform.position;
		entity.headQua = head.transform.rotation;

		//
		// entity.leftHandVec = leftHand.transform.position;
		// entity.leftHandQua = leftHand.transform.rotation;
		// Debug.Log ("SkeletonLeftHand  = " + SkeletonLeftHand.GetLastLocalPosition (InputSources).ToString () + "\n");
		// Debug.Log ("SkeletonLeftHand  = " + SkeletonLeftHand.GetLastLocalRotation (InputSources).ToString () + "\n");
		entity.leftHandVec = SkeletonLeftHand.GetLastLocalPosition (InputSources);
		entity.leftHandQua = SkeletonLeftHand.GetLastLocalRotation (InputSources);

		// entity.rightHandVec = rightHand.transform.position;
		// entity.rightHandQua = rightHand.transform.rotation;
		// Debug.Log ("SkeletonRightHand  = " + SkeletonRightHand.GetLastLocalPosition (InputSources).ToString () + "\n");
		// Debug.Log ("SkeletonRightHand  = " + SkeletonRightHand.GetLastLocalRotation (InputSources).ToString () + "\n");
		entity.rightHandVec = SkeletonRightHand.GetLastLocalPosition (InputSources);
		entity.rightHandQua = SkeletonRightHand.GetLastLocalRotation (InputSources);
		Calibration ();

		// entity.headVec.z = 0;
		// entity.leftHandVec.z = 0;
		// entity.rightHandVec.z = 0;
		thread = new Thread (DoHeavyProcess);
		thread.Start ();
	}

	private void DoHeavyProcess () {
		string json = JsonUtility.ToJson (entity);
		apiprotocol.GrpcApi.Update (json);
	}

	protected virtual void Calibration () {
		if (Button.GetLastState (InputSources)) {
			Debug.Log ("Button");
			calibrationVec = new Vector3 (0, 1.35F, 0) - entity.headVec;
		}
		entity.headVec = entity.headVec + calibrationVec;
		entity.leftHandVec = entity.leftHandVec + calibrationVec;
		entity.rightHandVec = entity.rightHandVec + calibrationVec;
	}
}