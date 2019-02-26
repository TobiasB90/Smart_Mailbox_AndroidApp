using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;

public class NetworkManager : MonoBehaviour
{
    [Header("Connection Settings")]
    public int ArduinoID;
    public string ServerIP;
    public string ServerPHP;

    [Header("Misc")]
    [SerializeField] private Image LatestMailPic;
    [SerializeField] private GameObject NewMailObj;
    [SerializeField] private GameObject NoMailObj;
    [SerializeField] private TextMeshProUGUI DateText;
    [SerializeField] private TextMeshProUGUI TimeText;

    [SerializeField] private GameObject HistoryObj_Prefab;
    [SerializeField] private GameObject HistoryObj_Parent;

    [SerializeField] private GameObject LoadingCircle;


    public void GetImage()
    {
        LoadingCircle.SetActive(true);
        NewMailObj.SetActive(false);
        if(ServerIP != null && ServerPHP != null)
        {
            string ServerURL = ServerIP.ToString() + "/" + ServerPHP.ToString();
            Debug.Log(ServerURL.ToString());
            StartCoroutine(GetImageURL(ServerURL));
        }
    }

    IEnumerator GetImageURL(string ServerURL)
    {
        WWWForm form = new WWWForm();
        form.AddField("action", "getimage");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(ServerURL, form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = ServerURL.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                Debug.Log(webRequest.downloadHandler.text);
                string imageURL = webRequest.downloadHandler.text.Replace("C:/Spiele/Xampp/htdocs", ServerIP);
                Debug.Log(imageURL);
                StartCoroutine(GetImageData(imageURL));
            }
        }
    }

    IEnumerator GetImageData(string imageURL)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                LoadingCircle.SetActive(false);
                Debug.Log(uwr.error);
                NoMailObj.SetActive(true);
            }
            else
            {
                LoadingCircle.SetActive(false);
                var texture = DownloadHandlerTexture.GetContent(uwr);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                if (LatestMailPic != null)
                {
                    LatestMailPic.sprite = sprite;
                    NoMailObj.SetActive(false);
                    NewMailObj.SetActive(true);
                    DateText.text = DateTime.Now.ToString("dd.MM.yyyy");
                    TimeText.text = DateTime.Now.ToString("hh:mm tt");

                    if(HistoryObj_Parent.transform.childCount <= 3)
                    {
                        var obj = GameObject.Instantiate(HistoryObj_Prefab, HistoryObj_Parent.transform);
                        HistoryObj_Data obj_data = obj.GetComponent<HistoryObj_Data>();
                        obj_data.MailImage.sprite = sprite;
                        obj_data.TimeText.text = DateTime.Now.ToString("hh:mm tt");
                        obj_data.DateText.text = DateTime.Now.ToString("dd.MM.yyyy");
                    }                    
                }
            }
        }
    }

    // Check for the same picture - could use later
    public bool SamePicture(Texture2D[] imgs)
    {
        int x, y;
        bool nomatch = false;

        y = imgs[0].height;
        x = imgs[0].width;

        if (x != imgs[1].width || y != imgs[1].height)
        {
            nomatch = true;
        }
        else
        {
            while (x > 0)
            {
                x--;
                y = imgs[0].height;
                while (y > 0)
                {
                    y--;
                    if (imgs[0].GetPixel(x, y) != imgs[1].GetPixel(x, y))
                    {
                        nomatch = true;
                    }
                }
            }
        }

        if (nomatch)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
