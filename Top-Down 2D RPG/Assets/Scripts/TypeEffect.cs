using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    string _targetMsg;
    public int _charPerSec;
    int index;
    public bool isAniamtion;

    public GameObject EndCursor;

    TextMeshProUGUI msgText;
    AudioSource audioSource;

    private void Awake()
    {
        msgText = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg)
    {
        if (isAniamtion)
        {
            CancelInvoke();
            msgText.text = _targetMsg;
            EffectEnd();
        }
        else
        {
            _targetMsg = msg;
            EffectStart();
        }
    }

    private void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);
        isAniamtion = true;

        Invoke("Effecting", 1.0f / _charPerSec);
    }

    void Effecting()
    {
        if (msgText.text.Equals(_targetMsg))
        {
            EffectEnd();
            return;
        }

        msgText.text += _targetMsg[index];

        // Sound
        if((_targetMsg[index] != '.') && (_targetMsg[index] != ' '))
            audioSource.Play();

        index++;
        
        Invoke("Effecting", 1.0f / _charPerSec);
    }

    void EffectEnd()
    {
        isAniamtion = false;
        EndCursor.SetActive(true);
    }
}
