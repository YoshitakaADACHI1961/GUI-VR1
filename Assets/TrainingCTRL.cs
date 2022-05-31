using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using System.IO;


//
// 静止画用
//
//
// 
//[System.Serializable]
// 制御用パラメータ　JSONファイル形式で指定すること
//
public class Control_Info
{
    public string ParamFile; // 制御パラメータファイル名
    public float Period; // 周期（秒）
    public float Duty; // デューティ比（<1.0）
    public float PULSE_STEP; // 定数
    public float PULSE_MAX; // 定数
    public float PULSE_INITIAL; // 定数
    public float PULSE_MIN; // 定数
    public float DUTY_INITIAL; // 定数
    public float DUTY_MIN; // 定数
    public float DUTY_MAX; // 定数
    public float DUTY_STEP; // 定数
    public float PulseStartTime; // 脈拍開始時刻（秒）
    public int PulseStartFrame; // 脈拍開始時刻（フレーム換算）
    public float PulseStopTime; // 脈拍停止時刻（秒）
    public int PulseStopFrame; // 脈拍停止時刻（フレーム換算）


    // 文字列からJSONデータを作る
    public static Control_Info CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Control_Info>(jsonString);
    }

    // JSONデータから文字列を作る
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    // 文字列を新しいデータで書き換える
    public void OverWrite(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}

//
// Raspberry Pi との情報交換に使用する
//
public class RaspControl
{
    public string RunMode; // START or STOP
    public float Period; // 周期（秒）
    public float Duty; // デューティ比（<1.0）

    // 文字列からJSONデータを作る
    public static RaspControl CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<RaspControl>(jsonString);
    }

    // JSONデータから文字列を作る
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    // 文字列を新しいデータで書き換える
    public void OverWrite(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}


//
// 
//
//[Serializable]
public class TrainingCTRL : MonoBehaviour
{
    Control_Info InitialParam = new Control_Info(); // 初期化用ファイルから入力されるパラメータ
    RaspControl ControlParam = new RaspControl(); // Raspberry Pi の制御用
    private float ProgramStartTime; // プログラムの開始時刻
    private float PulseStartTime; // 脈拍開始時刻（秒）
    private int PulseStartFrame; // 脈拍開始時刻（フレーム換算）
    private float PulseStopTime; // 脈拍停止時刻（秒）
    private int PulseStopFrame; // 脈拍停止時刻（フレーム換算）
    //
    // Start is called before the first frame update
    //
    void Start()
    {
        Debug.Log("Control Program Start!");
        Debug.Log(" Time.deltaTime : " + Time.deltaTime);
        // 初期設定用ファイルの読み込み
//        string initFile = File.ReadAllText("C:/Users/raspberry/UTfolder/InitControl.json");
        string initFile = "C:/Users/raspberry/UTfolder/InitControl.json";
//        Debug.Log("初期設定用ファイル : " + initFile);
        if (File.Exists(initFile)) // ファイルが存在する場合
        {
            string datastr = "";
            StreamReader reader;
            try
            {
                reader = new StreamReader(initFile);
                datastr = reader.ReadToEnd();
                InitialParam = JsonUtility.FromJson<Control_Info>(datastr);
  //              Debug.Log("InitialParam : " + InitialParam);
                reader.Close();
            }
            catch (System.IO.IOException ex)
            {
                Debug.Log("初期設定用ファイルを開くときにエラーになりました" + initFile + "  ERROR:" + ex);
            }
        }
        else
        {
            Debug.Log("初期設定用ファイルが存在しません : "+ initFile);
            // Unityを終了させる
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif

        }



        //        InitialParam = JsonUtility.FromJson<Control_Info>(initFile);

        // Raspberry Pi 制御パラメータの設定
        ControlParam.RunMode = "STOP";
        ControlParam.Period = InitialParam.Period;
        ControlParam.Duty = InitialParam.Duty;

        // Raspberry Pi 制御パラメータファイルの書き込み
//        Debug.Log("test : " + InitialParam.ParamFile + " : " + ControlParam.SaveToString());
        try
        {
            File.WriteAllText(InitialParam.ParamFile, ControlParam.SaveToString());
            Debug.Log("静止画用　Raspberry Pi 制御パラメータファイルの書き込み");
        }
        catch (System.IO.IOException ex)
        {
            Debug.Log("静止画用　制御パラメータファイルの書き込みでエラーになりました" + InitialParam.ParamFile + "  ERROR:" + ex);
        }

        ProgramStartTime = Time.time; // 現在の時刻
        PulseStartTime = InitialParam.PulseStartTime + ProgramStartTime; // 脈拍開始時刻（秒）
        PulseStartFrame = (int)(PulseStartTime / Time.deltaTime); // 脈拍開始時刻（フレーム換算）
        PulseStopTime = InitialParam.PulseStopTime + ProgramStartTime; // 脈拍停止時刻（秒）
        PulseStopFrame = (int)(PulseStopTime / Time.deltaTime); // 脈拍停止時刻（フレーム換算）
        Debug.Log("START time : " + PulseStartTime + " PulseStopTime" + PulseStopTime);
    }

    //
    // Fixed Update 
    // Edit」→「Project Setting」→「Time」 FixedTimestep 0.5
    //
    private void FixedUpdate()
    {
        /*
            if ((Time.time > PulseStartTime)&& (Time.time < PulseStopTime) && (ControlParam.RunMode == "STOP")) // 拍動開始時刻を過ぎた場合
            {
                ControlParam.RunMode = "START";
                Debug.Log("Pulse START !  " + Time.time);
            }
            else if ( (Time.time > PulseStopTime) && (ControlParam.RunMode == "START"))
            {
                ControlParam.RunMode = "STOP";
                Debug.Log("Pulse STOP !  " + Time.time);
            }
        */
        Debug.Log("ControlParam.RunMode  :  " + ControlParam.RunMode);
        // 制御パラメータファイルの書き込み
        File.WriteAllText(InitialParam.ParamFile, ControlParam.SaveToString());

    }

    //
    // Update is called once per frame
    //
    void Update()
    {


        //
        // 脈拍の制御
        //
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.upArrowKey.wasPressedThisFrame)
            {
                ControlParam.Period -= InitialParam.PULSE_STEP;
                if (ControlParam.Period < InitialParam.PULSE_MIN)
                    ControlParam.Period = InitialParam.PULSE_MIN;
            }
            else if (keyboard.downArrowKey.wasPressedThisFrame)
            {
                ControlParam.Period += InitialParam.PULSE_STEP;
                if (ControlParam.Period > InitialParam.PULSE_MAX)
                    ControlParam.Period = InitialParam.PULSE_MAX;
            }
            else if (keyboard.rightArrowKey.wasPressedThisFrame)
            {
                ControlParam.Duty += InitialParam.DUTY_STEP;
                if (ControlParam.Duty > InitialParam.DUTY_MAX)
                    ControlParam.Duty = InitialParam.DUTY_MAX;
            }
            else if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                ControlParam.Duty -= InitialParam.DUTY_STEP;
                if (ControlParam.Duty < InitialParam.DUTY_MIN)
                    ControlParam.Duty = InitialParam.DUTY_MIN;
            }
            else if (keyboard.spaceKey.wasPressedThisFrame)
            {
                if (ControlParam.RunMode == "STOP")
                {
                    ControlParam.RunMode = "START";
                    Debug.Log("Set to START");
                }
                else
                {
                    ControlParam.RunMode = "STOP";
                    Debug.Log("Set to STOP");
                }
            }
            else if (keyboard.qKey.wasPressedThisFrame)
            {
//                WebCamTexture webCamTexture = new WebCamTexture();
//                webCamTexture.Stop(); // カメラを停止

               ControlParam.RunMode = "STOP";
                File.WriteAllText(InitialParam.ParamFile, ControlParam.SaveToString());
                // スタートメニューに切り替える
                SceneManager.LoadScene("StartHere");
            }
        }
    }
}

