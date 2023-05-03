using Shawn.ProjectFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadUI : BasePanel
{
    private Slider progressSlider;
    private Text text;
    private int sceneIndex;

    public override void Show()
    {
        base.Show();

        // 获取进度条组件
        progressSlider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<Text>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(sceneIndex + 1);
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // 更新
            progressSlider.value = operation.progress;

            text.text = operation.progress * 100 + "%";

            // 加载完成，隐藏
            if (operation.progress >= 0.9f)
            {
                progressSlider.value = 1f;

                text.text = "Press AnyKey to Continue";
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                    PanelManager.Instance.HidePanel("LoadUI");
                    PanelManager.Instance.ShowPanel<UGUI_MainUIPanel>("UGUI_MainUIPanel");
                }
            }
            yield return null;
        }
    }
}
