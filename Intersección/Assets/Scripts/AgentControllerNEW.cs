// TC2008B. Sistemas Multiagentes y Gráficas Computacionales
// C# client to interact with Python
// Sergio. Julio 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class jsonDataClass
{
    public List<dataList> data;
}

[Serializable]
public class dataList
{
    public string tag;
    public int x;
    public int z;
    public int luces;
    public int reference;
}

public class AgentControllerNEW : MonoBehaviour
{
    public float spawnTime = 1.5f;
    private float timing;
    public float otherTime;

    void Start() {
        #if UNITY_EDITOR
        Vector3 fakePos = new Vector3(3.44f, 1.03f, -15.707f);
        string json = EditorJsonUtility.ToJson(fakePos);
        StartCoroutine(SendData(json));
        timing = spawnTime;

#endif
        }

    public GameObject[] cars;
    private List<List<Vector3>> positions;

    private SortedDictionary<int, GameObject> carsSpwnd =
    new SortedDictionary<int, GameObject>();
    int RandomOption;


    // IEnumerator - yield return
    IEnumerator SendData(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585";
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("Content-Type", "text/html");
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();          // Talk to Python
            if ((www.result == UnityWebRequest.Result.ConnectionError) ||
                (www.result == UnityWebRequest.Result.ProtocolError))
            {
                Debug.Log(www.error);
                www.Dispose();
                www.disposeUploadHandlerOnDispose = true;
                www.disposeDownloadHandlerOnDispose = true;
            }
            else
            {
                Debug.Log(www.downloadHandler.text);    // Answer from Python
                jsonDataClass js= JsonUtility.FromJson<jsonDataClass>(www.downloadHandler.text);
                foreach (dataList item in js.data){
                    //Debug.Log(item.x);
                    Vector3 pos = new Vector3(item.x*10,1.58f,item.z*10);
                    RandomOption = UnityEngine.Random.Range(0, cars.Length);
                    Debug.Log(pos);
                    if(carsSpwnd.ContainsKey(item.reference)){
                        carsSpwnd[item.reference].GetComponent<CarController>().newPosition = pos;

                    } else {
                        GameObject carro = cars[RandomOption];
                        carsSpwnd.Add(item.reference,Instantiate(carro,pos,Quaternion.Euler(0, 0, 0)));
                    }

                    www.Dispose();
                    www.disposeUploadHandlerOnDispose = true;
                    www.disposeDownloadHandlerOnDispose = true;
                }
                www.Dispose();
                www.disposeUploadHandlerOnDispose = true;
                www.disposeDownloadHandlerOnDispose = true;
            }
        }

    }

    void Update()
    {
        timing -= Time.deltaTime;
        otherTime = 1.0f - (timing / spawnTime);

        if(timing < 0)
        {
#if UNITY_EDITOR
            timing = spawnTime; // reset the timing
            Vector3 fakePos = new Vector3(3.44f, 1.03f, -15.707f);
            string json = EditorJsonUtility.ToJson(fakePos);
            StartCoroutine(SendData(json));
#endif
        }
    }
}
