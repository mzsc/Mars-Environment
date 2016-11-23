﻿#if !UNITY_EDITOR
#if UNITY_ANDROID
#define ANDROID_DEVICE
#elif UNITY_IPHONE
#define IOS_DEVICE
#elif UNITY_STANDALONE_WIN
#define WIN_DEVICE
#endif
#endif
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine.UI;


/// <summary>
/// 总控Manager
/// </summary>
public class PicoVRManager : MonoBehaviour
{
    /************************************    Properties  *************************************/
    #region Properties
    /// <summary>
    /// resume 标志位
    /// </summary>
    public bool onResume = false;

	public const int SERVICE_STARTED = 0;
	public const int CONNECTE_SUCCESS = 1;
	public const int DISCONNECTE = 2;
	public const int CONNECTE_FAILED = 3;
	public const int NO_DEVICE = 4;

    /// <summary>
    /// SDK 单例
    /// </summary>
    private static PicoVRManager sdk = null;
    public static PicoVRManager SDK
    {
        get
        {
            if (sdk == null)
            {
                sdk = UnityEngine.Object.FindObjectOfType<PicoVRManager>();
            }
            if (sdk == null)
            {
                var go = new GameObject("PicoVRManager");
                sdk = go.AddComponent<PicoVRManager>();
                go.transform.localPosition = Vector3.zero;
            }
            return sdk;
        }
    }

    /// <summary>
    /// 是否使用Falcon box sensor
    /// </summary>
    [SerializeField]
    private bool useFalconBoxSensor = false;
    public bool UseFalconBoxSensor
    {
        get
        {
            return useFalconBoxSensor;
        }
        set
        {
            if (value != useFalconBoxSensor)
            {
                useFalconBoxSensor = value;
            }
        }
    }

    /// <summary>
    /// 显示FPS
    /// </summary>
    [SerializeField]
    private bool showFPS = false;

   
    public bool ShowFPS
    {
        get
        {            
            return showFPS;
        }
        set
        {
            if (value != showFPS)
            {
                showFPS = value;               
            }
        }
    }
    
    /// <summary>
    /// 配置文件
    /// </summary>
    public PicoVRConfigProfile picoVRProfile;

    /// <summary>
    /// Head Pose
    /// </summary>
    public PicoVRPose headPose;

    /// <summary>
    /// 当前设备
    /// </summary>
    public PicoVRBaseDevice currentDevice;

    /// <summary>
    /// 适配HMD类型
    /// </summary>
    [SerializeField]
    private PicoVRConfigProfile.DeviceTypes deviceType = PicoVRConfigProfile.DeviceTypes.PICOVR_I;
    public PicoVRConfigProfile.DeviceTypes DeviceType
    {
        get
        {
            return deviceType;
        }
        set
        {
            if (value != deviceType)
            {
                deviceType = value;
            }
        }
    }

    /// <summary>
    /// 抗锯齿倍数
    /// </summary>
    [SerializeField]
    private PicoVRBaseDevice.RenderTextureAntiAliasing rtAntiAlising = PicoVRBaseDevice.RenderTextureAntiAliasing.X_2;
    public PicoVRBaseDevice.RenderTextureAntiAliasing RtAntiAlising
    {
        get
        {
            return rtAntiAlising;
        }
        set
        {
            if (value != rtAntiAlising)
            {
                rtAntiAlising = value;

            }
        }
    }

    /// <summary>
    /// RT 位深
    /// </summary>
    [SerializeField]
    private PicoVRBaseDevice.RenderTextureDepth rtBitDepth = PicoVRBaseDevice.RenderTextureDepth.BD_24;
    public PicoVRBaseDevice.RenderTextureDepth RtBitDepth
    {
        get
        {
            return rtBitDepth;
        }
        set
        {
            if (value != rtBitDepth)
                rtBitDepth = value;

        }
    }

    /// <summary>
    /// RT 类型
    /// </summary>
    [SerializeField]
    private RenderTextureFormat rtFormat = RenderTextureFormat.Default;
    public RenderTextureFormat RtFormat
    {
        get
        {
            return rtFormat;
        }
        set
        {
            if (value != rtFormat)
                rtFormat = value;

        }
    }

    /// <summary>
    /// VR （Unity Editor 模拟）
    /// 其他默认为true
    /// </summary>
    [SerializeField]
    private bool vrModeEnabled = true;
    public bool VRModeEnabled
    {

        get
        {
            return vrModeEnabled;
        }
        set
        {
            if (value != vrModeEnabled)
                vrModeEnabled = value;

        }
    }
    /// <summary>
    /// 开启编辑器调试
    /// </summary>
    [SerializeField]
    private bool isVREditorDebug = false;

    public bool IsVREditorDebug
    {
        get
        {
            return isVREditorDebug;
        }
        set
        {
            if (value != isVREditorDebug)
            {
                isVREditorDebug = value;
            }
        }
    }
    /// <summary>
    /// 消息传递参数WarpID
    /// </summary>
    public int timewarpID = 0;

    /// <summary>
    /// 更新标志位
    /// </summary>
    private bool upDated = false;

    /// <summary>
    /// 状态更新标志位
    /// </summary>
    public bool upDateState = false;

    /// <summary>
    /// RenderTexture （Unity Editor 模拟）
    /// </summary>
    [HideInInspector]
    public RenderTexture stereoScreen;
    public RenderTexture StereoScreen
    {
        get
        {
            if (!vrModeEnabled)
            {
                return null;
            }
            if (stereoScreen == null)
            {
                stereoScreen = CreateStereoScreen();
            }

            return stereoScreen;
        }
        set
        {
            try
            {
                if (value != stereoScreen)
                {
                    stereoScreen = value;
                }
            }
            catch (Exception)
            {

                Debug.LogError("StereoScreen ERROR!");
            }
        }
    }     
    public RenderTexture CreateStereoScreen()
    {
        Vector2 renderTexSize = currentDevice.GetStereoScreenSize();
        int x = (int)renderTexSize.x;
        int y = (int)renderTexSize.y;

        if (currentDevice.CanConnecttoActivity && SystemInfo.supportsRenderTextures)
        {
            var steroscreen = new RenderTexture(x, y, (int)SDK.RtBitDepth, SDK.RtFormat);
            steroscreen.anisoLevel = 0;
            steroscreen.antiAliasing = Mathf.Max(QualitySettings.antiAliasing, (int)SDK.RtAntiAlising);
            Debug.Log("steroscreen ok");
            return steroscreen;
        }
        else
           return null;
    }

    /// <summary>
    /// 左右眼标志位
    /// </summary>
    public enum Eye
    {
        LeftEye,
        RightEye
    }

    /// <summary>
    /// 左右眼投影矩阵（Unity Editor 模拟）
    /// </summary>
    public Matrix4x4 Projection(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeProj : rightEyeProj;
    }
    [HideInInspector]
    public Matrix4x4 leftEyeProj;
    [HideInInspector]
    public Matrix4x4 rightEyeProj;

    /// <summary>
    /// 左右眼无畸变矩阵（Unity Editor 模拟）
    /// </summary>
    public Matrix4x4 UndistortedProjection(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeUndistortedProj : rightEyeUndistortedProj;
    }
    [HideInInspector]
    public Matrix4x4 leftEyeUndistortedProj;
    [HideInInspector]
    public Matrix4x4 rightEyeUndistortedProj;

    /// <summary>
    /// 凝视事件标志位
    /// </summary>
    public bool picovrTriggered { get; private set; }
    private bool newPicovrTriggered = false;
    public bool inPicovr { get; set; }
    private bool newInPicovr;

    /// <summary>
    /// 左右眼偏移量
    /// </summary>
    public Vector3 EyeOffset(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeOffset : rightEyeOffset;
    }
    public Vector3 leftEyeOffset;
    public Vector3 rightEyeOffset;

    /// <summary>
    /// 左右眼view rect（Unity Editor 模拟）
    /// </summary>
    public Rect EyeRect(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeRect : rightEyeRect;
    }
    [HideInInspector]
    public Rect leftEyeRect;
    [HideInInspector]
    public Rect rightEyeRect;

    /// <summary>
    /// headView 矩阵
    /// </summary>
    [HideInInspector]
    public Matrix4x4 headView;


    /// <summary>
    /// 左右眼view 矩阵
    /// </summary>
    [HideInInspector]
    public Matrix4x4 leftEyeView;
    [HideInInspector]
    public Matrix4x4 rightEyeView;

    /// <summary>
    /// FOV
    /// </summary>
    [HideInInspector]
    public float eyeFov = 90.0f;


    /// <summary>
    /// SimulateInput 参数
    /// </summary>
    private const float TOUCH_TIME_LIMIT = 0.2f;
    private float touchStartTime = 0;

    /// <summary>
    /// reset head 标志
    /// </summary>
    public bool reStartHead = false;
    #endregion

    /************************************ Process Interface  *********************************/
    #region Process Interface
    /// <summary>
    /// 初始化设备接口
    /// </summary>
    private void InitDevice()
    {
        if (currentDevice != null)
        {
            currentDevice.Destroy();
        }
        currentDevice = PicoVRBaseDevice.GetDevice();
    }

    /// <summary>
    /// 更新状态
    /// </summary>
    public bool UpdateState()
    {
        if (upDated)
        {
            return true;
        }
        currentDevice.UpdateState();
        leftEyeOffset = leftEyeView.GetColumn(3);
        rightEyeOffset = rightEyeView.GetColumn(3);
        upDated = true;
        return upDated;
    }

    /// <summary>
    /// reset head tracking
    /// </summary>
    public void ResetHead()
    {
        reStartHead = true;
        currentDevice.ResetHeadTrack();
    }
    #endregion

    /*************************************Unity Editor ***************************************/
    #region UnityEditor
    /// <summary>
    /// 模拟输入（Unity Editor 模拟）
    /// </summary>
    private void SimulateInput()
    {

        if (Input.GetMouseButtonDown(0)
            && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
        {
            if (inPicovr)
            {
                OnRemovedFromPicovrInternal();
            }
            else
            {
                OnInsertedIntoPicovrInternal();
            }
            VRModeEnabled = !VRModeEnabled;
            return;
        }
        if (!inPicovr)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            touchStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - touchStartTime <= TOUCH_TIME_LIMIT)
            {
                newPicovrTriggered = true;
            }
            touchStartTime = 0;
        }
    }

    /// <summary>
    ///  模拟输入调用（Unity Editor 模拟）
    /// </summary>
    void OnRemovedFromPicovrInternal()
    {
        newInPicovr = false;
    }

    /// <summary>
    ///  模拟输入调用（Unity Editor 模拟）
    /// </summary>
    void OnInsertedIntoPicovrInternal()
    {
        newInPicovr = true;
    }

    /// <summary>
    /// 切换VR模型（Unity Editor 模拟）
    /// </summary>
    public void ToggleVRMode()
    {
        vrModeEnabled = !vrModeEnabled;
    }
    #endregion

    /*************************** Public Interfaces  (android ok) *****************************/
    #region Interfaces

    /// <summary>
    /// 电池电量发生变化获取当前电量值
    /// </summary>
    /// 
    public void setBattery(string s)
    {
        Debug.Log(s.ToString() + "Battery");

    }

    /// <summary>
    /// 音量发生变化获取当前音量值
    /// </summary>
    public void setAudio(string s)
    {
        Debug.Log(s.ToString() + "Audio");
    }

	/// <summary>
	/// called from android to set BLEStatus
	/// </summary>
	public void BLEStatusCallback(string s)
	{
		switch(int.Parse(s)){
		case(SERVICE_STARTED):
			Debug.Log("BLE_SERVICE_STARTED");
			break;
		case(CONNECTE_SUCCESS):
			Debug.Log("BLE_CONNECTE_SUCCESS");
			break;
		case(DISCONNECTE):
			Debug.Log("BLE_DISCONNECTE");
			break;
		case(CONNECTE_FAILED):
			Debug.Log("BLE_CONNECTE_FAILED");
			break;
		case(NO_DEVICE):
			Debug.Log("BLE_NO_DEVICE");
			break;
		}
	}
    
    /// <summary>
    /// 获取音量最大值
    /// </summary>
    public int getMaxVolumeNumber()
    {
        int maxVolumeNumber = currentDevice.getMaxVolumeNumber();
        Debug.Log("maxVolumeNumber" + maxVolumeNumber);
        return maxVolumeNumber;
    }

    /// <summary>
    /// 获取当前音量
    /// </summary>
    public int getCurrentVolumeNumber()
    {
        int currentVolumeNumber = currentDevice.getCurrentVolumeNumber();
        Debug.Log("currentVolumeNumber" + currentVolumeNumber);
        return currentVolumeNumber;
    }

    /// <summary>
    /// 获取当前亮度
    /// </summary>
    public int getCurrentBrightness()
    {
        int currentBrightness = currentDevice.getCurrentBrightness();
        Debug.Log("currentBrightness" + currentBrightness);
        return currentBrightness;
    }

    /// <summary>
    /// 设置亮度
    /// </summary>
    public void setBrightness(int brightness)
    {
        currentDevice.setBrightness(brightness);
    }
    /// <summary>
    /// 操作falcon系统接口
    /// </summary>
    public  bool setDevicePropForUser(PicoVRConfigProfile.DeviceCommand deviceid, string value)
    {
        return currentDevice.setDevicePropForUser( deviceid, value);
    }
    /// <summary>
    /// 操作falcon系统接口
    /// </summary>
    public string getDevicePropForUser(PicoVRConfigProfile.DeviceCommand deviceid)
    {
        return currentDevice.getDevicePropForUser(deviceid);
    }
    /// <summary>
    /// 升高音量
    /// </summary>
    public void volumeUp()
    {
        currentDevice.volumeUp();
    }

    /// <summary>
    /// 降低音量
    /// </summary>
    public void volumeDown()
    {
        currentDevice.volumeDown();
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    public void setVolumeNum(int volume)
    {
        currentDevice.setVolumeNum(volume);
    }
    #endregion

    #region IOS Special
    void CheckStereoRender()
    {
        GameObject stereo = this.transform.FindChild("StereoRender").gameObject;
        if (stereo != null)
            stereo.SetActive( true );
    }

    void AddPrePostRenderStages()
    {
        var preRender = UnityEngine.Object.FindObjectOfType<PicoVRPreRender>();
        if (preRender == null)
        {
            var go = new GameObject("PreRender", typeof(PicoVRPreRender));
            go.SendMessage("Reset");
            go.transform.parent = transform;
        }

        var postRender = UnityEngine.Object.FindObjectOfType<PicoVRPostRender>();
        if (postRender == null)
        {
            var go = new GameObject("PostRender", typeof(PicoVRPostRender));
            go.SendMessage("Reset");
            go.transform.parent = transform;
        }
    }
    #endregion
    /*************************************  Unity API ****************************************/
    #region Application EVENT

    void Awake()
    {
        if (!VrUtility.IsPicoVR) return;

        if (sdk == null)
        {
            sdk = this;
        }
        if (sdk != this)
        {
            Debug.LogWarning("SDK object should be a singleton.");
            enabled = false;
            return;
        }
        string FPSname = "PicoVR/Head/FPS";
        GameObject FPS = GameObject.Find(FPSname);
        FPS.SetActive(showFPS);
        InitDevice();
        picoVRProfile = PicoVRConfigProfile.Default.Clone();
        inPicovr = false;
        newInPicovr = true;
        eyeFov = 90.0f;
        headPose = new PicoVRPose(Matrix4x4.identity);
        //CheckStereoRender();
        AddPrePostRenderStages();

    }

    void Start()
    {
        if (!VrUtility.IsPicoVR) return;

        if (currentDevice == null)
        {
            Application.Quit();
            Debug.Log("start  Device == null ");
        }
        currentDevice.Init();
        currentDevice.UpdateScreenData();
    }

    void Update()
    {
        if (!VrUtility.IsPicoVR) return;


#if UNITY_EDITOR || WIN_DEVICE
        SimulateInput();
#else
       if (Input.touchCount == 1)//一个手指触摸屏幕
        {
            if (Input.touches[0].phase == TouchPhase.Began)//开始触屏
            {
                newPicovrTriggered = true;
            }
        }
        else 
        if(Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            newPicovrTriggered = true;

        }
#endif

        upDateState = UpdateState();
        if (inPicovr != newInPicovr)
        {
            currentDevice.UpdateScreenData();
        }
        inPicovr = newInPicovr;
        picovrTriggered = newPicovrTriggered;
        newPicovrTriggered = false;
        upDated = false;
    }

    void OnDestroy()
    {
        if (!VrUtility.IsPicoVR) return;

        if (sdk == this)
        {
            sdk = null;
        } 		
        RenderTexture.active = null;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void OnApplicationQuit()
    {
        if (!VrUtility.IsPicoVR) return;

        //currentDevice.stopHidService();
        currentDevice.StopHeadTrack();
		currentDevice.CloseHMDSensor();
		currentDevice.StopBLECentral ();
        //currentDevice.requestHidSensor(0);
#if UNITY_ANDROID && !UNITY_EDITOR
/*
		try{
			Debug.Log("OnApplicationQuit  1  -------------------------");
			PVRPluginEvent.Issue( RenderEventType.ShutdownRenderThread );
		}
		catch (Exception e)
        {
            Debug.Log("ShutdownRenderThread Error" + e.Message);
        }
*/
#elif UNITY_STANDALONE_WIN
        currentDevice.Destroy();
#endif
    }

    void OnDisable()
    {
        if (!VrUtility.IsPicoVR) return;

        StopAllCoroutines();
    }

    private void OnPause()
    {
        if (!VrUtility.IsPicoVR) return;

        //currentDevice.stopHidService();
        currentDevice.StopHeadTrack();
		currentDevice.CloseHMDSensor();
		//currentDevice.requestHidSensor (0); 

        if (PicoVRManager.SDK.currentDevice.Async)
        {
            LeaveVRMode();
        }
    }       

    private void OnApplicationPause(bool pause)
    {
        if (!VrUtility.IsPicoVR) return;

        Debug.Log("OnApplicationPause-------------------------" + (pause ? "true" : "false"));
#if UNITY_ANDROID && !UNITY_EDITOR
        if (pause)
        {
            OnPause();
        }
        else
        {
            onResume = true;
            currentDevice.StartHeadTrack(); 
                GL.InvalidateState();
                StartCoroutine(OnResume()); 
        }
#endif
#if IOS_DEVICE
		if (pause)
		{
			currentDevice.StopHeadTrack();
		}
		else
		{
			currentDevice.StartHeadTrack();  
		}
#endif
    }

    private IEnumerator OnResume()
    { 
		//currentDevice.requestHidSensor (1);  
		currentDevice.OpenHMDSensor();
        //currentDevice.startHidService();
        for (int i = 0; i < 20; i++)
        {
            yield return null;
        }
        if (PicoVRManager.SDK.currentDevice.Async)
        {
            EnterVRMode();
        }
    }

    void OnApplicationFocus(bool focus)
    {
        if (!VrUtility.IsPicoVR) return;

        Debug.Log("OnApplicationFocus-------------------------" + (focus ? "true" : "false"));
		currentDevice.IsFocus(focus);
    }

    public static void EnterVRMode()
    {
        if (!VrUtility.IsPicoVR) return;

        PVRPluginEvent.Issue(RenderEventType.Resume);
    }

    public static void LeaveVRMode()
    {
        if (!VrUtility.IsPicoVR) return;

        PVRPluginEvent.Issue(RenderEventType.Pause);
    }

    #endregion   
    
	public void devicePowerStateChanged (string state){
		this.currentDevice.DevicePowerStateChanged (state);
	}

	public void deviceConnectedStateChanged (string state){
		this.currentDevice.DeviceConnectedStateChanged (state);
	}
	public void deviceFindNotification (string msg){
		this.currentDevice.DeviceFindNotification (msg);
	}
	public void acceptDeviceKeycode (string keykode){
		this.currentDevice.AcceptDeviceKeycode (keykode);
	}
}
