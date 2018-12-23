using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ImplController : MonoBehaviour {

	public GameObject head;
	public GameObject leftHand;
	public GameObject rightHand;

	private JsonEntity entity = new JsonEntity ();
	private Thread thread;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		entity.headVec = head.transform.position;
		entity.headQua = head.transform.rotation;
		entity.leftHandVec = leftHand.transform.position;
		entity.leftHandQua = leftHand.transform.rotation;
		entity.rightHandVec = rightHand.transform.position;
		entity.rightHandQua = rightHand.transform.rotation;
		thread = new Thread (DoHeavyProcess);
		thread.Start ();
	}

	private void DoHeavyProcess () {
		string json = JsonUtility.ToJson (entity);
		apiprotocol.GrpcApi.Update (json);
	}
}