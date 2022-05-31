using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using System.IO;
using System;

// 
// Oculusから見たコントローラ（左右）の位置と姿勢を計測する
// 「b」を押すと座位の計測
// 「ｍ」を押すと立位の計測
// 
public class MesurementController : MonoBehaviour
{
    // 実行モード
    int runMode = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start MesurementController !");

        //「MODE」というキーで保存されているInt値を読み込み
        runMode = PlayerPrefs.GetInt("MODE");
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger) || OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            Debug.Log("右または左の中指グリップを押した!");

            //  右手と左手　コントローラーの位置を取得
            Vector3 LocalPos_R = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 LocalPos_L = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            Quaternion LocalRotation_R = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            Quaternion LocalRotation_L = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

            //            Debug.Log(LocalPos_R+","+LocalRotation_R + "," + LocalPos_L + "," + LocalRotation_L);

            // データを入れておくファイル
            string DataFile = "C:/Users/raspberry/UTfolder/PosController.json";
            // ファイルが存在していたら書き換えをする。そうでなければ新しく作成する
            if (File.Exists(DataFile)) // ファイルが存在する場合
            {
                string datastr = "";
                StreamReader reader;
                try
                {
                    reader = new StreamReader(DataFile);
                    datastr = reader.ReadToEnd();
                    reader.Close();
                }
                catch (System.IO.IOException ex)
                {
                    Debug.Log("1 ファイルを開くときにエラーになりました" + ex);
                }

                ControllerPos controllerPos = JsonUtility.FromJson<ControllerPos>(datastr);

                if (runMode == 1) // 座位のデータを入れる
                {
                    StreamWriter writer;
                    controllerPos.sitPosR = LocalPos_R;
                    controllerPos.sitRotationR = LocalRotation_R;
                    controllerPos.sitPosL = LocalPos_L;
                    controllerPos.sitRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    Debug.Log("Json " + jsonstr);
                    try
                    {
                        writer = new StreamWriter(DataFile, false);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("2 ファイルを開くときにエラーになりました" + ex);
                    }
                }
                else // 立位のデータを入れる
                {
                    StreamWriter writer;
                    controllerPos.standPosR = LocalPos_R;
                    controllerPos.standRotationR = LocalRotation_R;
                    controllerPos.standPosL = LocalPos_L;
                    controllerPos.standRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    try
                    {
                        writer = new StreamWriter(DataFile, false);
                        Debug.Log("Json " + jsonstr);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("3 ファイルを開くときにエラーになりました" + ex);
                    }
                }

            }
            else // ファイルが存在しない場合
            {
                if (runMode == 1) // 座位のデータを入れる
                {
                    StreamWriter writer;
                    ControllerPos controllerPos = new ControllerPos();
                    controllerPos.sitPosR = LocalPos_R;
                    controllerPos.sitRotationR = LocalRotation_R;
                    controllerPos.sitPosL = LocalPos_L;
                    controllerPos.sitRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    Debug.Log("Json " + jsonstr);
                    try
                    {
                        writer = new StreamWriter(DataFile, append: true);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("4 ファイルを開くときにエラーになりました" + ex);
                    }
                }
                else // 立位のデータを入れる
                {
                    StreamWriter writer;
                    ControllerPos controllerPos = new ControllerPos();
                    controllerPos.standPosR = LocalPos_R;
                    controllerPos.standRotationR = LocalRotation_R;
                    controllerPos.standPosL = LocalPos_L;
                    controllerPos.standRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    try
                    {
                        writer = new StreamWriter(DataFile, append: true);
                        Debug.Log("Json " + jsonstr);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("5 ファイルを開くときにエラーになりました" + ex);
                    }
                }
            }
            // スタートメニューに切り替える
            SceneManager.LoadScene("StartHere");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // スタートメニューに切り替える
            SceneManager.LoadScene("StartHere");
        }
    }

    //
    // class を作成する
    //
    [System.Serializable]
    public class ControllerPos
    {
        public Vector3 sitPosR = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion sitRotationR = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 sitPosL = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion sitRotationL = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 standPosR = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion standRotationR = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 standPosL = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion standRotationL = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    }
}
