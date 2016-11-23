using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonKeyTest : MonoBehaviour
{
    [SerializeField]
    private Transform m_PicoVR;
    [SerializeField]
    private Transform m_PicoVRHead;

    private const float MOVE_SPEED = 0.03f;
    public Text m_uiText;
    private int m_iCount = 0;
    void Start()
    {
        //UnityEngine.AndroidJavaClass unityPlayer = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //UnityEngine.AndroidJavaObject activity = unityPlayer.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
        //UnityEngine.AndroidJavaClass javaVrActivityClass = new UnityEngine.AndroidJavaClass("com.picovr.picovrlib.VrActivity");
        //CallStaticMethod(javaVrActivityClass, "startLarkConnectService", activity);
    }

    //public bool CallStaticMethod(UnityEngine.AndroidJavaObject jobj, string name, params object[] args)
    //{
    //    try
    //    {
    //        jobj.CallStatic(name, args);
    //        return true;
    //    }
    //    catch (AndroidJavaException e)
    //    {
    //        Debug.LogError("CallStaticMethod  Exception calling activity method " + name + ": " + e);
    //        return false;
    //    }
    //}


    void Update()
    {
        if (m_uiText == null) return;
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))         //A
        {
            //Debug.Log("button A's keycode equals to Joystick1Button0");
            SetTextInfo("Joystick1Button0");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button1))    //B
        {
            //Debug.Log("button B's keycode equals to Joystick1Button1");
            SetTextInfo("Joystick1Button1");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2))    //X
        {
            //Debug.Log("button X's keycode equals to Joystick1Button2");
            SetTextInfo("Joystick1Button2");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button3))    //Y
        {
            //Debug.Log("button Y's keycode equals to Joystick1Button3");
            SetTextInfo("Joystick1Button3"); ;
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button4))    //L
        {
            //Debug.Log("button L's keycode equals to Joystick1Button4");
            SetTextInfo("Joystick1Button4");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button5))    //R
        {
            //Debug.Log("button R's keycode equals to Joystick1Button5");
            SetTextInfo("Joystick1Button5");
        }
        else if (Input.GetKeyDown(KeyCode.Menu))                //Menu
        {
            //Debug.Log("button Menu's keycode equals to Menu");
            SetTextInfo("KeyCode Menu");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))              //Escape
        {
            //Debug.Log("button Escape's keycode equals to Escape");
            SetTextInfo("KeyCode Escape");
        }

        if (Input.GetAxis("Horizontal") > 0)        // Right
        {
            SetTextInfo("Horizontal > 0");
            m_PicoVR.position += Vector3.ProjectOnPlane(m_PicoVRHead.right, Vector3.up).normalized * MOVE_SPEED;
        }
        else if (Input.GetAxis("Horizontal") < 0)   // Left
        {
            SetTextInfo("Horizontal < 0");
            m_PicoVR.position -= Vector3.ProjectOnPlane(m_PicoVRHead.right, Vector3.up).normalized * MOVE_SPEED;
        }

        if (Input.GetAxis("Vertical") > 0)          // Up
        {
            SetTextInfo("Vertical > 0");
            m_PicoVR.position += Vector3.ProjectOnPlane(m_PicoVRHead.forward, Vector3.up).normalized * MOVE_SPEED;
        }
        else if (Input.GetAxis("Vertical") < 0)     // Down
        {
            SetTextInfo("Vertical < 0");
            m_PicoVR.position -= Vector3.ProjectOnPlane(m_PicoVRHead.forward, Vector3.up).normalized * MOVE_SPEED;
        }

    }

    public void SetTextInfo(string info)
    {
        m_uiText.text = info;
    }
}
