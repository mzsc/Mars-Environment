using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeachingLabelManager : MonoBehaviour {
    public Text TeachingLable;
    public GameObject SpyeyeLabel;

    void Awake() {
        gameObject.SetActive(false);
        TeachingLable.gameObject.SetActive(false);
        SpyeyeLabel.SetActive(false);
        TakeAWalk.TakeAWalkStopEvent += ShowPanel;
        ToTheTarget.ArriveTargetEvent += ShowLable;
        ToTheTarget.ToTheTargetEvent += HideLable;
    }

    void OnDestroy() {
        ToTheTarget.ToTheTargetEvent -= HideLable;
        ToTheTarget.ArriveTargetEvent -= ShowLable;
        TakeAWalk.TakeAWalkStopEvent -= ShowPanel;
    }

    private void ShowPanel() {
        gameObject.SetActive(true);
    }

    private void ShowLable(string TargetId, string TargetName) {
        if(TargetId == "Spyeye") {
            SpyeyeLabel.SetActive(true);
        } else {
            TeachingLable.gameObject.SetActive(true);
            TeachingLable.text = TargetName;
        }
    }

    private void HideLable() {
        TeachingLable.gameObject.SetActive(false);
        SpyeyeLabel.SetActive(false);
    }
}
