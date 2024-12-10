using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequests {

    private class WebRequestsMonoBehaviour : MonoBehaviour { }

    private static WebRequestsMonoBehaviour webRequestsMonoBehaviour;

    private static void Init() {
        if (webRequestsMonoBehaviour == null) {
            GameObject gameObject = new GameObject("WebRequests");
            webRequestsMonoBehaviour = gameObject.AddComponent<WebRequestsMonoBehaviour>();
        }
        
        UnityWebRequest.ClearCookieCache();
    }

    public static void Get(string url, Action<string> onError, Action<string> onSuccess, Action<float> OnUpdateLoading = null) {
        Init();
        webRequestsMonoBehaviour.StartCoroutine(GetCoroutine(url, onError, onSuccess, OnUpdateLoading));
    }

    private static IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSuccess, Action<float> OnUpdateLoading = null) {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url)) {
            unityWebRequest.SendWebRequest();

            while (!unityWebRequest.isDone)
            {
                // Cập nhật thanh loading bằng cách sử dụng downloadProgress
                OnUpdateLoading?.Invoke(unityWebRequest.downloadProgress);
                yield return null;
            }

            OnUpdateLoading?.Invoke(1.0f);

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError) {
                onError(unityWebRequest.error);
            } else {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
            
            unityWebRequest.Dispose();
        }
    }

    public static void Post(string url, Dictionary<string, string> formFields, Action<string> onError, Action<string> onSuccess) {
        Init();
        webRequestsMonoBehaviour.StartCoroutine(GetCoroutinePost(url, formFields, onError, onSuccess));
    }

    public static void Post(string url, string postData, Action<string> onError, Action<string> onSuccess) {
        Init();
        webRequestsMonoBehaviour.StartCoroutine(GetCoroutinePost(url, postData, onError, onSuccess));
    }

    private static IEnumerator GetCoroutinePost(string url, Dictionary<string, string> formFields, Action<string> onError, Action<string> onSuccess) {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, formFields)) {
            
            yield return unityWebRequest.SendWebRequest();
            

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError) {
                onError(unityWebRequest.error);
            } else {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
            
            unityWebRequest.Dispose();
        }
    }

    private static IEnumerator GetCoroutinePost(string url, string postData, Action<string> onError, Action<string> onSuccess) {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.PostWwwForm(url, postData)) {
            
            yield return unityWebRequest.SendWebRequest();
            

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError) {
                onError(unityWebRequest.error);
            } else {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
            
            unityWebRequest.Dispose();
        }
    }

    public static void Put(string url, string bodyData, Action<string> onError, Action<string> onSuccess) {
        Init();
        webRequestsMonoBehaviour.StartCoroutine(GetCoroutinePut(url, bodyData, onError, onSuccess));
    }

    private static IEnumerator GetCoroutinePut(string url, string bodyData, Action<string> onError, Action<string> onSuccess) {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Put(url, bodyData)) {
            
            yield return unityWebRequest.SendWebRequest();
            

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError) {
                onError(unityWebRequest.error);
            } else {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
            
            unityWebRequest.Dispose();
        }
    }

    public static void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess) {
        Init();
        webRequestsMonoBehaviour.StartCoroutine(GetTextureCoroutine(url, onError, onSuccess));
    }

    private static IEnumerator GetTextureCoroutine(string url, Action<string> onError, Action<Texture2D> onSuccess) {
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url)) {
            
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError) {
                onError(unityWebRequest.error);
            } else {
                DownloadHandlerTexture downloadHandlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                onSuccess(downloadHandlerTexture.texture);
            }
            
            unityWebRequest.Dispose();
        }
    }

}
