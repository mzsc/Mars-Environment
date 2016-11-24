using UnityEngine;
using System.Collections;

public class ToTheTarget : MonoBehaviour {

    public static event MethodtDelegate ArriveTargetEvent;

    public Transform MoveObject;
    public Transform[] TargetTfArray;
    public string[] TargetIdArray;
    public string[] TargetNameArray;
    private int TargetNum;
    public float Speed = 1;
    private float AngleSpeed;
    private bool IsEnd = true;

    private bool IsArrive {
        get {
            return (MoveObject.position.x < TargetTfArray[TargetNum].position.x + 1) &&
                (MoveObject.position.x > TargetTfArray[TargetNum].position.x - 1) &&
                (MoveObject.position.z < TargetTfArray[TargetNum].position.z + 1) &&
                (MoveObject.position.z > TargetTfArray[TargetNum].position.z - 1)&&
                (MoveObject.position.y < TargetTfArray[TargetNum].position.y + 1) &&
                (MoveObject.position.y > TargetTfArray[TargetNum].position.y - 1);
        }
    }

    public string MoveToTarget(string TargetId) {
        int i = 0;
        for(i = 0; i < TargetIdArray.Length; i++) {
            if(TargetIdArray[i] == TargetId) {
                TargetNum = i;
                CalculateAngularSpeed();
                IsEnd = false;
                return TargetNameArray[i];
            }
        }
        return null;
    }

	void Update () {
        if(!IsEnd) {
            MoveObject.position = Vector3.MoveTowards(MoveObject.position, TargetTfArray[TargetNum].position, Speed);
            MoveObject.rotation = Quaternion.RotateTowards(MoveObject.rotation, TargetTfArray[TargetNum].rotation, AngleSpeed);
            CheckArrive();
        }
    }

    private void CheckArrive() {
        if(IsArrive) {           
            IsEnd = true;
            ArriveTargetEvent();
        }
    }

    private void CalculateAngularSpeed() {
        float TargetAngle = Quaternion.Angle(MoveObject.rotation, TargetTfArray[TargetNum].rotation);
        float TargetDistance = Vector3.Distance(MoveObject.position, TargetTfArray[TargetNum].position);
        AngleSpeed = TargetAngle / (TargetDistance / Speed);
    }
}
