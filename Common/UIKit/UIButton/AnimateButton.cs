// Copyright (C) 2017-2018 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


/// <summary>
/// All the buttons in the game play an animation when they are pressed. This class, modeled after Unity's Button,
/// enables that behavior.
/// </summary>
public class AnimateButton : UIBehaviour, IPointerClickHandler
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent
    {
    }

    public bool interactable = true;
    public bool enableAnimate = true;
    public bool isPlaySound = true;

    [SerializeField]
    private ButtonClickedEvent m_onClick = new ButtonClickedEvent();

    private Animator animator;

    private bool blockInput;

    public ButtonClickedEvent onClick
    {
        get { return m_onClick; }
        set { m_onClick = value; }
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called when there is a click/touch over the button.
    /// </summary>
    /// <param name="eventData">The data associated to the pointer event.</param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("AnimateButton OnPointerClick blockInput=" + blockInput);
        if (eventData.button != PointerEventData.InputButton.Left || !interactable)
        {
            Debug.Log("AnimateButton OnPointerClick fail");
            return;
        }

        if (!blockInput)
        {
            blockInput = true;
            Press();
            StartCoroutine(BlockInputTemporarily());
        }
    }

    /// <summary>
    /// Sets this button as pressed.
    /// </summary>
    private void Press()
    {
        Debug.Log("AnimateButton Press");
        if (!IsActive())
        {
            return;
        }
        if (isPlaySound)
        {
            AudioPlay.main.PlayBtnSound();
        }
        animator.SetTrigger("Pressed");
        StartCoroutine(InvokeOnClickAction());
    }

    /// <summary>
    /// Invokes the custom, user-defined event associated to the button press.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator InvokeOnClickAction()
    {
        yield return new WaitForSeconds(0.2f);
        blockInput = false;
        m_onClick.Invoke();
    }

    /// <summary>
    /// Blocks the input temporarily to prevent spamming.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator BlockInputTemporarily()
    {
        yield return new WaitForSeconds(0.5f);
        blockInput = false;
    }
}


