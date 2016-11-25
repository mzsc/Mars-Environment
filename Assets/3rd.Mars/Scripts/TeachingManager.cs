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

    void Awake() {
        gameObject.SetActive(false);
        TakeAWalk.TakeAWalkStopEvent += ShowPanel;
    }

    void OnDestroy() {
        TakeAWalk.TakeAWalkStopEvent -= ShowPanel;
    }

    private void ShowPanel() {
        gameObject.SetActive(true);
    }

    public void OnClickButton(string SceneName) {
        TeachingControl.MoveToTarget(SceneName);
    }
}
