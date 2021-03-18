using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Events;
public class PopUpManager : MonoBehaviour
{

    UIViewPop uiPrefab;
    UIViewPop ui;
    protected Stack<GameObject> currentPopups = new Stack<GameObject>();
    protected Stack<GameObject> currentPanels = new Stack<GameObject>();

    public static PopUpManager main;
    Action<UIViewPop> _onClose;
    public GameObject panel;
    void Init()
    {

    }

    void Awake()
    {
        main = this;
    }
    public void ShowPannl(bool show)
    {
        if (panel == null)
        {
            return;
        }
        panel.gameObject.SetActive(show);
    }
    public void Show<T>(string pathPrefab, Action<T> onOpened = null, Action<UIViewPop> onClose = null, bool darkenBackground = true) where T : UIViewPop
    {
        StartCoroutine(OpenPopupAsync(pathPrefab, onOpened, onClose, darkenBackground));
    }


    /// <summary>
    /// Utility coroutine to open a popup asynchronously.
    /// </summary>
    /// <param name="popupName">The name of the popup prefab located in the Resources folder.</param>
    /// <param name="onOpened">The callback to invoke when the popup has finished loading.</param>
    /// <param name="darkenBackground">True if the popup should have a dark background; false otherwise.</param>
    /// <typeparam name="T">The type of the popup.</typeparam>
    /// <returns>The coroutine.</returns>
    protected IEnumerator OpenPopupAsync<T>(string popupName, Action<T> onOpened, Action<UIViewPop> onClose, bool darkenBackground) where T : UIViewPop
    {
        // var request = Resources.LoadAsync<GameObject>(popupName);
        // while (!request.isDone)
        // {
        //     yield return null;
        // }
        yield return null;
        GameObject objPrefab = PrefabCache.main.Load(popupName);
        if (objPrefab == null)
        {
            objPrefab = PrefabCache.main.LoadByKey(popupName);
        }
        Canvas canvas = AppSceneBase.main.canvasMain;
        panel = new GameObject("Panel");
        var panelImage = panel.AddComponent<Image>();
        var color = Color.black;
        color.a = 0.5f;
        panelImage.color = color;
        var panelTransform = panel.GetComponent<RectTransform>();
        panelTransform.anchorMin = new Vector2(0, 0);
        panelTransform.anchorMax = new Vector2(1, 1);
        panelTransform.pivot = new Vector2(0.5f, 0.5f);
        panel.transform.SetParent(canvas.transform, false);
        currentPanels.Push(panel);
        StartCoroutine(FadeIn(panel.GetComponent<Image>(), 0.2f));

        //var popup = Instantiate(request.asset) as GameObject;
        var popup = Instantiate(objPrefab) as GameObject;
        Assert.IsNotNull((popup));
        popup.transform.SetParent(canvas.transform, false);

        //popup.GetComponent<Popup>().parentScene = this;

        AppSceneBase.main.listPopup.Add(popup.GetComponent<UIViewPop>());


        if (onOpened != null)
        {
            onOpened(popup.GetComponent<T>());
        }
        _onClose = onClose;
        currentPopups.Push(popup);
    }


    public void OnClose()
    {

    }


    public void OnCloseAll()
    {
        foreach (GameObject ui in currentPanels)
        {
            Destroy(ui);
        }
        currentPanels.Clear();

    }

    /// <summary>
    /// Closes the topmost popup.
    /// </summary>
    public void ClosePopup()
    {
        var topmostPopup = currentPopups.Pop();
        if (topmostPopup == null)
        {
            return;
        }
        int len = AppSceneBase.main.listPopup.Count;
        if (len > 0)
        {
            AppSceneBase.main.listPopup.RemoveAt(len - 1);
        }

        var topmostPanel = currentPanels.Pop();
        if (topmostPanel != null)
        {
            StartCoroutine(FadeOut(topmostPanel.GetComponent<Image>(), 0.2f, () => Destroy(topmostPanel)));
        }

        if (_onClose != null)
        {
            _onClose(topmostPopup.GetComponent<UIViewPop>());
        }
    }



    /// <summary>
    /// Utility coroutine to fade in the specified image.
    /// </summary>
    /// <param name="image">The image to fade.</param>
    /// <param name="time">The duration of the fade in seconds.</param>
    /// <returns>The coroutine.</returns>
    protected IEnumerator FadeIn(Image image, float time)
    {
        var alpha = image.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            var color = image.color;
            color.a = Mathf.Lerp(alpha, 220 / 256.0f, t);
            image.color = color;
            yield return null;
        }
    }

    /// <summary>
    /// Utility coroutine to fade out the specified image.
    /// </summary>
    /// <param name="image">The image to fade.</param>
    /// <param name="time">The duration of the fade in seconds.</param>
    /// <param name="onComplete">The callback to invoke when the fade has finished.</param>
    /// <returns>The coroutine.</returns>
    protected IEnumerator FadeOut(Image image, float time, Action onComplete)
    {
        var alpha = image.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            var color = image.color;
            color.a = Mathf.Lerp(alpha, 0, t);
            image.color = color;
            yield return null;
        }
        if (onComplete != null)
        {
            onComplete();
        }
    }
}
