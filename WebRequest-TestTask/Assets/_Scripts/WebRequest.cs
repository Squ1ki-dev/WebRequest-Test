using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Tools.PlayerPrefs;

namespace Tools
{
    public static class WebRequest 
    {
        private class WebRequestsMonoBehaviour : MonoBehaviour { }

        private static WebRequestsMonoBehaviour _webRequestsMonoBehaviour;

        private static void Init() 
        {
            if (_webRequestsMonoBehaviour == null) 
            {
                GameObject gameObject = new GameObject("WebRequest");
                _webRequestsMonoBehaviour = gameObject.AddComponent<WebRequestsMonoBehaviour>();
            }
        }

        public static void GetPicture(string url, Action<Sprite> getSpriteEvent, Action<string> error, bool createCash = true, bool needValidateCertificate = true) 
        {
            Init();
            _webRequestsMonoBehaviour.StartCoroutine(LoadSprite(url, getSpriteEvent, error, createCash, needValidateCertificate));
        }

        public static IEnumerator LoadTexture2D(string url, Action<Texture2D> getTextureEvent, Action<string> error, bool createCash = true, bool needValidateCertificate = true)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                if (createCash && PlayerPrefsPro.HasKey(url))
                {
                    getTextureEvent.Invoke(PlayerPrefsPro.GetTexture(url));
                    yield break;
                }
                //Need set headers
                if (!needValidateCertificate) request.certificateHandler = new BypassCertificate();
                yield return request.SendWebRequest();
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                getTextureEvent.Invoke(texture);
                if (createCash) PlayerPrefsPro.SetTexture(url, texture);
            }
        }

        public static IEnumerator LoadSprite(string url, Action<Sprite> getSpriteEvent, Action<string> error, bool createCash = true, bool needValidateCertificate = true)
        {
            yield return LoadTexture2D(url, texture =>
            {
                getSpriteEvent.Invoke(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 1f), 150));
            }, error, createCash, needValidateCertificate);
        }
    }
}