using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoamManager : MonoBehaviour {

    public TakeAWalk RoamControl;
    public Button RoamBtn;
    public Button PauseBtn;
    public Button ContinueBtn;
    public Button StopBtn;

    void Awake() {
        TakeAWalk.TakeAWalkStopEvent += HidePanel;
        ButtonControl(true, false, false, false);
    }

    void OnDestroy() {
        TakeAWalk.TakeAWalkStopEvent -= HidePanel;
    }

    private void HidePanel() {
        gameObject.SetActive(false);
    }

    private void ButtonControl(bool IsRoam, bool IsPause, bool IsContinue, bool IsStop) {
        RoamBtn.gameObject.SetActive(IsRoam);
        PauseBtn.gameObject.SetActive(IsPause);
        ContinueBtn.gameObject.SetActive(IsContinue);
        StopBtn.gameObject.SetActive(IsStop);
    }

    public void OnClickRoamBtn() {
        RoamControl.StartTakeAWalk();
        ButtonControl(false, true, false, true);
    }

    public void OnClickPauseBtn() {
        RoamControl.PauseTakeAWalk();
        ButtonControl(false, false, true, true);
    }

    public void OnClickContinueBtn() {
        RoamControl.ContinueTakeAWalk();
        ButtonControl(false, true, false, true);
    }

    public void OnClickStop() {
        RoamControl.StopTakeAWalk();
        ButtonControl(false, false, false, false);
    }
}
