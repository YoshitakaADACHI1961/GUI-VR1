using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


//
// Sphere100 のコントロール
//


public class TrainigSphere : MonoBehaviour
{
    // transform
    //    Transform myTransform;

    // 現在の位置
    Vector3 currentPos;

    // 修正用
    Vector3 cPos = new Vector3(1.0f, 1.0f, 1.0f);

    // 現在のコントローラ L の位置・姿勢
    Vector3 controllerPos;
    Quaternion controllerOri;
    // ターゲットコントローラ L の位置・姿勢
    Vector3 targetPos;
    Quaternion targetOri;
    // 移動量
    Vector3 offsetPos;

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
        // 球を移動させる
        // Oculusから見たコントローラの相対位置
        controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        controllerOri = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        // 移動量の計算　元の位置に戻るためのベクトル量
        offsetPos = controllerPos - targetPos;

        // 今の位置
        currentPos = this.transform.position;  //

        string offset2 = PlayerPrefs.GetString("OFFSET").Trim('(', ')');
        string[] offsetStr = offset2.Split(',');

        // store as a Vector3
        Vector3 offset = new Vector3(
            float.Parse(offsetStr[0]),
            float.Parse(offsetStr[1]),
            float.Parse(offsetStr[2]));


        // 移動
        this.transform.position = offsetPos +offset;  // 座標を設定

/*
        var keyboard = Keyboard.current;
        if (keyboard.qKey.wasPressedThisFrame)
        {
            // スタートメニューに切り替える
            SceneManager.LoadScene("StartHere");
        }

        Debug.Log("Now: " + currentPos + " Offset: " + offsetPos + " Next: " + this.transform.position + " Scale: " + this.transform.localScale);
*/

    }


    //
    // class を作成する
    //
    [System.Serializable]
class ControllerPos
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
