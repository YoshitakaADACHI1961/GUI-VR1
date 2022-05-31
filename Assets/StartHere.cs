using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class StartHere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.bKey.wasPressedThisFrame)
            {
                // 座位での初期位置を計測して位置姿勢データを保存する
                // 
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MeasurementScene");

            }
            else if (keyboard.mKey.wasPressedThisFrame)
            {
                // 立位での初期位置を計測して位置姿勢データを保存する
                // 
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MeasurementScene");
            }
            else if (keyboard.cKey.wasPressedThisFrame)
            {
                // 座位のキャリブレーション
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("InitPosition");
            }
            else if (keyboard.zKey.wasPressedThisFrame)
            {
                // 立位のキャリブレーション
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("InitPosition");
            }
            else if (keyboard.rKey.wasPressedThisFrame)
            {
                // 座位の実行
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("TrainingScene");
            }
            else if (keyboard.yKey.wasPressedThisFrame)
            {
                // 立位の実行
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("TrainingScene");
            }
            else if (keyboard.oKey.wasPressedThisFrame)
            {
                // 座位の実行
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MovieScene");
            }
            else if (keyboard.uKey.wasPressedThisFrame)
            {
                // 立位の実行
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MovieScene");
            }
        }
    }
}
