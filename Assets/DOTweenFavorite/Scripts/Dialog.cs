using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using static DG.Tweening.Ease;

public class Dialog : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] RectTransform window;
    [SerializeField] Text message;
    [SerializeField] Text buttonText;
    Graphic[] graphics;
    Action onClose;

    public bool IsShowing    { get; private set; }
    public string Message    => message.text;
    public string ButtonText => buttonText.text;

    public static Dialog Create()
    {
        var d      = Instantiate(Resources.Load<Dialog>(nameof(Dialog)));
        d.graphics = d.GetComponentsInChildren<Graphic>();
        return d;
    }

    public Dialog SetMessage(string message)
    {
        this.message.text = message;
        return this;
    }

    public Dialog SetButtonText(string buttonText)
    {
        this.buttonText.text = buttonText;
        return this;
    }

    public Dialog Open(Action onClose = null)
    {
        this.onClose = onClose;
        IsShowing    = true;
        PlayAnimation();
        return this;
    }

    public Dialog Close()
    {
        IsShowing = false;
        PlayAnimation();
        return this;
    }

    public void OnButtonClick() => Close();

    void PlayAnimation()
    {
        if(IsShowing)
        {
            DOTween.Sequence()
                   .Append(window.DOScale(1, 0.3f).SetEase(OutBack))
                   .Append(background.DOFade(0.5f, 0.1f).SetEase(Linear));
        }
        else
        {
            background.DOFade(0, 0.3f).SetEase(OutCubic);
        }

        var start    = IsShowing ? 0f : 1f;
        var end      = IsShowing ? 1f : 0f;
        var duration = 0.3f;
        var ease     = IsShowing ? InCubic : OutCubic;
        var tweener  = DOTween.To(a =>
        {
            foreach(var g in graphics)
            {
                if(g == background) continue;

                var color = g.color;
                color.a   = a;
                g.color   = color;
            }
        }, 
        start, end, duration).SetEase(ease);

        if(!IsShowing)
        {
            tweener.OnComplete(() =>
            {
                onClose?.Invoke();
                Destroy(gameObject);
            });
        }
    }
}