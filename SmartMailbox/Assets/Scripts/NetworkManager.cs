using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        GetImage();
    }

    public void GetImage()
    {
        if(ServerIP != null && ServerPHP != null)
        {
            string ServerURL = ServerIP + "/" + ServerPHP;
            StartCoroutine(GetImageURL(ServerURL));
        }        
    }

    IEnumerator GetImageURL(string ServerURL)
    {
        WWWForm form = new WWWForm();
        form.AddField("ArduinoID", ArduinoID);
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
                StartCoroutine(GetImageData(webRequest.downloadHandler.text));
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
                Debug.Log(uwr.error);
                NoMailObj.SetActive(true);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                if(LatestMailPic != null)
                {
                    LatestMailPic.sprite = sprite;
                    NoMailObj.SetActive(false);
                    NewMailObj.SetActive(true);
                }
            }
        }
    }
}
