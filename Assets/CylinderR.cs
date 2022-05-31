using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CylinderR : MonoBehaviour
{
    // 実行モード
    int runMode = 0;
    // コントローラの位置
    ControllerPos controllerPos;

    // Start is called before the first frame update
    void Start()
    {
        //「MODE」というキーで保存されているInt値を読み込み
        runMode = PlayerPrefs.GetInt("MODE");

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
            Debug.Log("1 ファイルを開くときにエラーになりました" + ex);
        }

        controllerPos = JsonUtility.FromJson<ControllerPos>(datastr);

        if (runMode == 1) // 座位のデータを入れる
        {
            transform.position = controllerPos.sitPosR;
            Debug.Log("座位　Cylinder R = " + transform.position);
        }
        else // 立位のデータを入れる
        {
            transform.position = controllerPos.standPosR;
            Debug.Log("立位　Cylinder R = " + transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {

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
