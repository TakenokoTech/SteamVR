using UnityEngine;

public struct AverterParam {

    public struct PositionEntity {
        Vector3 vec;
        Quaternion qua;
    }

    public PositionEntity eyeCamera;
    public PositionEntity handLeft;
    public PositionEntity handRight;

    // 右人差し指トリガー
    public float rTrigger1; // = OVRInput.Get (OVRInput.RawAxis1D.RIndexTrigger);

    // 左人差し指トリガー
    public float lTrigger1; // = OVRInput.Get (OVRInput.RawAxis1D.LIndexTrigger);

    // 左手,右手のアナログスティックの向きを取得
    public Vector2 stickL; // = OVRInput.Get (OVRInput.RawAxis2D.LThumbstick);
    public Vector2 stickR; // = OVRInput.Get (OVRInput.RawAxis2D.RThumbstick);

}