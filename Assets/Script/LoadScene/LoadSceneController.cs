using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneController : MonoBehaviour
{
    static string nextScene;
    private float zPosition = 90f;
    private float newPosition;
    [SerializeField] private GameObject startText;
    [SerializeField] private Image gageImage;
    
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); //비동기 방식의 불러오기
        op.allowSceneActivation = false; //씬을 90퍼까지만 로딩하고 멈춤 완전히 안넘김
        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                // progressbar.fillamount = op.progress;
                newPosition = (float)(zPosition - (180 * op.progress));
                gageImage.transform.eulerAngles = new Vector3(0,0, newPosition);
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                // 0.9에서 1까지는 자연스럽게 채우는 수치 표시가 있으니 이게 계산값에 들어가야한다고 본다.
                newPosition = zPosition - (180 * Mathf.Lerp(0.9f, 1f, timer));
                gageImage.transform.eulerAngles = new Vector3(0, 0,newPosition);
                Debug.Log(newPosition);
                if (newPosition <= -90f)
                {
                    Debug.Log("z값 180이 되었거나 그 이상임.");
                    startText.SetActive(true);
                    while (ButtonController.CheckNext == false)
                    {
                        
                        yield return new WaitForSeconds(0.5f);
                        if (startText.activeSelf == true)
                        {
                            startText.SetActive(false);
                        }
                        yield return new WaitForSeconds(0.5f);
                        if (startText.activeSelf == false)
                        {
                            startText.SetActive(true);
                        }
                    }
                    op.allowSceneActivation = true;
                }
            }

        }

    }
}
