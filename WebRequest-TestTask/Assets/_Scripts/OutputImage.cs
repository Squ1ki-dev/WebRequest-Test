using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

namespace Tools
{
    public class OutputImage : MonoBehaviour
    {
        [SerializeField] private TMP_Text _verificationText;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool createCash = true;
        private bool needValidateCertificate = true;

        private string url;

        public void PrintImage() 
        {
            url = _inputField.text;

            WebRequest.GetPicture(url, (Sprite getSpriteEvent) => 
            { 
                // Successfully contacted URL
                _verificationText.SetText("Success!");
                Sprite sprite = getSpriteEvent;
                spriteRenderer.sprite = sprite;
            }, (string error) =>
            {
                Debug.LogError("Failed!" + error);
                _verificationText.SetText("Failed!" + error);
            }, createCash, needValidateCertificate);
        }
    }
}

