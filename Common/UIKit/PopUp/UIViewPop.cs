using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIViewPop : UIView
{


    public UnityEvent onOpen;
    public UnityEvent onClose;

    private Animator animator;

    public void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    public void Start()
    {
        base.Start();
        AudioPlay.main.PlayFile(AppRes.Audio_PopupOpen);
        if (onOpen != null)
        {
            onOpen.Invoke();
        }
    }

    /// <summary>
    /// Closes the popup.
    /// </summary>
    public void Close()
    {
        AudioPlay.main.PlayFile(AppRes.Audio_PopupClose);
        if (onClose != null)
        {
            onClose.Invoke();
        }
        // if (parentScene != null)
        // {
        //     parentScene.ClosePopup();
        // }
        PopUpManager.main.ClosePopup();
        if (animator != null)
        {
            animator.Play("Close");
            StartCoroutine(DestroyPopup());
        }
        else
        {
            DoClose();
        }
    }


    public void DoClose()
    {
        PopUpManager.main.OnClose();
        DestroyImmediate(gameObject);
    }

    /// <summary>
    /// Utility coroutine to automatically destroy the popup after its closing animation has finished.
    /// </summary>
    /// <returns>The coroutine.</returns>
    protected virtual IEnumerator DestroyPopup()
    {
        yield return new WaitForSeconds(0.5f);
        DoClose();
    }


}
