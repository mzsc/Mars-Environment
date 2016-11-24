using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeachingManager : MonoBehaviour {

    public ToTheTarget TeachingControl;
    public Button CraterBtn;//环形山
    public Button CanyonBtn;//峡谷
    public Button PlainBtn;//平原
    public Button MountainBtn;//山脉
    public Button DustBtn;//尘暴
    public Button SpyeyeBtn;//鸟览
    public Text TeachingLable;

    private string TargetName;
    private string TargetId;

    void Awake() {
        gameObject.SetActive(false);
        TeachingLable.gameObject.SetActive(false);
        TakeAWalk.TakeAWalkStopEvent += ShowPanel;
        ToTheTarget.ArriveTargetEvent += ShowLable;
    }

    void OnDestroy() {
        ToTheTarget.ArriveTargetEvent -= ShowLable;
        TakeAWalk.TakeAWalkStopEvent -= ShowPanel;
    }

    private void ShowPanel() {
        gameObject.SetActive(true);
    }

    private void ShowLable() {
        TeachingLable.gameObject.SetActive(true);
        TeachingLable.text = TargetName;
    }

    public void OnClickButton(string SceneName) {
        TargetId = SceneName;
        TargetName = TeachingControl.MoveToTarget(TargetId);
        TeachingLable.gameObject.SetActive(false);
    }
}
