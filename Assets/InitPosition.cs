using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using System.IO;


//
// Sphere100 のコントロール
//

public class InitPosition : MonoBehaviour
{
    // 現在の位置
    Vector3 currentPos;

    // 現在のコントローラ L の位置・姿勢
    Vector3 controllerPos;
    Quaternion controllerOri;
    // ターゲットコントローラ L の位置・姿勢
    Vector3 targetPos;
    Quaternion targetOri;
    // 移動量
    Vector3 offsetPos;
    // 手動の移動量
    Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        //「MODE」というキーで保存されているInt値を読み込み
        int runMode = PlayerPrefs.GetInt("MODE");
        // 読み込み
        string DataFile = "C:/Users/raspberry/UTfolder/PosController.json";
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
            Debug.Log("ファイルを開くときにエラーになりました" + ex);
        }
        ControllerPos initialControllerPos = JsonUtility.FromJson<ControllerPos>(datastr);
        if (runMode == 1) // 座位のデータを入れる
        {

            // Oculusから見たコントローラの相対位置
            targetPos = initialControllerPos.sitPosL;
            targetOri = initialControllerPos.sitRotationL;
        }
        else // 立位のデータを入れる
        {
            // Oculusから見たコントローラの相対位置
            targetPos = initialControllerPos.standPosL;
            targetOri = initialControllerPos.standRotationL;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //
        // 球をOculusの追従させて移動させる
        //
        // Oculusから見たコントローラの相対位置
        controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        controllerOri = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        // 移動量の計算　元の位置に戻るためのベクトル量
        offsetPos = controllerPos - targetPos;
        // 今の位置
        currentPos = this.transform.position;  //

        //
        // 球を手動で移動させる
        //
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            //            Debug.Log("キーボード");
            if (keyboard.upArrowKey.wasPressedThisFrame)
            {
                offset.z = offset.z + 0.01f;
//                Debug.Log("↑" + $" Offset: {offset:0.000}");
            }
            else if (keyboard.rightArrowKey.wasPressedThisFrame)
            {
                offset.x = offset.x + 0.01f;
//                Debug.Log("→" + $" Offset: {offset:0.000}");
            }
            else if (keyboard.downArrowKey.wasPressedThisFrame)
            {
                offset.z = offset.z - 0.01f;
//                Debug.Log("↓" + $" Offset: {offset:0.000}");
            }
            else if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                offset.x = offset.x - 0.01f;
//                Debug.Log("←" + $" Offset: {offset:0.000}");
            }
            else if (keyboard.rightBracketKey.wasPressedThisFrame)
            {
                offset.y = offset.y - 0.01f;
//                Debug.Log("  ]" + $" Offset: {offset:0.000}");
            }
            else if (keyboard.leftBracketKey.wasPressedThisFrame)
            {
                offset.y = offset.y + 0.01f;
//                Debug.Log("  [" + $" Offset: {offset:0.000}");
            }
            else if (keyboard.backspaceKey.wasPressedThisFrame)
            {
                offset = new Vector3(0.0f, 0.0f, 0.0f);
 //               Debug.Log("Reset Position" + $" =  Offset:{offset:0.000}");
            }
            else if (keyboard.qKey.wasPressedThisFrame)
            {
                // シーン間でオフセットデータの共有
                string offsetStr = offset.ToString("F5");
                PlayerPrefs.SetString("OFFSET", offsetStr);
                PlayerPrefs.Save();

                Debug.Log("OffsetString:" + offsetStr);

                // スタートメニューに切り替える
                SceneManager.LoadScene("StartHere");
            }
        }
        // 移動
        this.transform.position = offsetPos + offset;   // 座標を設定


        Debug.Log($"Now: {currentPos:0.000}  Offset:{offsetPos:0.000} Next:{this.transform.position:0.000}  Scale:{this.transform.localScale:0.0}");
//        Debug.Log("Now: " + currentPos + " Offset: " + offsetPos + " Next: " + this.transform.position + " Scale: " + this.transform.localScale);

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
