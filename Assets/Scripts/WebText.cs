using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebText : MonoBehaviour
{
    public Text MyTextUI;
    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        // Get : �ּ�?Ű1=��1&Ű2=��2..
        string url = "https://openapi.naver.com/v1/search/news.json?query=�̽���&display=30";

        var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("X-Naver-Client-Id", "erQUFRB6PXsPpu030Aeo");
        www.SetRequestHeader("X-Naver-Client-Secret", "N840ZV00EL");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            MyTextUI.text = www.downloadHandler.text;
        }
    }
}
