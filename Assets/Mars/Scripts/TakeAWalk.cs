using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void MethodtDelegate();

public class TakeAWalk : MonoBehaviour {

    public static event MethodtDelegate TakeAWalkStopEvent;

    public Transform TakeAWalkObject;
    public Transform[] TargetList;
    public bool[] IsCenter;
    public float Speed = 1.0f;
    private float AngleSpeed;
    private float CenterSpeed;
    private Transform TargetTf;
    private Transform CenterTf;
    private int TargetCount;
    private int TargetNum;
    private bool IsEnd = true;

    private bool IsArrive {
        get {
            return (TakeAWalkObject.position.x < TargetTf.position.x + 1) &&
                (TakeAWalkObject.position.x > TargetTf.position.x - 1) &&
                (TakeAWalkObject.position.z < TargetTf.position.z + 1) &&
                (TakeAWalkObject.position.z > TargetTf.position.z - 1);
        }
    }

    public void StartTakeAWalk() {
        TargetCount = TargetList.Length;
        TargetNum = 0;
        if(TargetCount > 0) {
            TargetTf = TargetList[0];
            CalculateAngularSpeed();
            IsEnd = false;
        } else {
            IsEnd = true;
            Debug.LogError("TargetList Null!!!");
        }
    }

    public void PauseTakeAWalk() {
        IsEnd = true;
    }

    public void ContinueTakeAWalk() {
        IsEnd = false;
    }

    public void StopTakeAWalk() {
        IsEnd = true;
        TakeAWalkStopEvent();
    }

    void Update () {
        if(!IsEnd) {
            if((CenterTf != null) && IsCenter[TargetNum - 1]) {
                TakeAWalkObject.RotateAround(CenterTf.position, Vector3.up, - Speed / Mathf.PI / 1.5f);
            } else {
                TakeAWalkObject.position = Vector3.MoveTowards(TakeAWalkObject.position, TargetTf.position, Speed);
                TakeAWalkObject.rotation = Quaternion.RotateTowards(TakeAWalkObject.rotation, TargetTf.rotation, AngleSpeed);
            }           
            CheckArrive();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }           
    }

    private void CheckArrive() {
        if(IsArrive) {
            TargetNum++;
            if(TargetNum < TargetCount) {
                if(IsCenter[TargetNum]) {
                    CenterTf = TargetList[TargetNum];
                    //CalculateCenterSpeed();
                    TargetNum++;
                }
                TargetTf = TargetList[TargetNum];
                CalculateAngularSpeed();
            } else {
                IsEnd = true;
                TakeAWalkStopEvent();
            }
        }
    }

    private void CalculateAngularSpeed() {
        float TargetAngle = Quaternion.Angle(TakeAWalkObject.rotation, TargetTf.rotation);
        float TargetDistance = Vector3.Distance(TakeAWalkObject.position, TargetTf.position);
        AngleSpeed = TargetAngle / (TargetDistance / Speed);
    }

    private void CalculateCenterSpeed() {
        float Radius = Vector3.Distance(CenterTf.position, TargetTf.position);
        CenterSpeed = - Speed / Radius;
    }
}
