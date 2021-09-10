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
    public int posx;
    public int posz;
    public int lights;
    public int reference;
}

public class AgentControllerNEW : MonoBehaviour
{
    public float timeToUpdate = 1.5f;
    private float timer;
    public float dt;

    void Start() {
        #if UNITY_EDITOR
        //string call = "WAAAAASSSSSAAAAAAAAAAP?";
        Vector3 fakePos = new Vector3(3.44f, 0, -15.707f);
        string json = EditorJsonUtility.ToJson(fakePos);
        //StartCoroutine(SendData(call));
        StartCoroutine(SendData(json));
        timer = timeToUpdate;

#endif
        }

    //Los agentes los ponemos manualmente en la simulacion

    public GameObject cars;
    

    // public List<Vector3> posC1;
    // public List<Vector3> posC2;
    // public List<Vector3> posC3;
    // public List<Vector3> posC4;

    private List<List<Vector3>> positions;

    private SortedDictionary<int, GameObject> activeCars =
    new SortedDictionary<int, GameObject>();


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
                    //Debug.Log(item.posx);
                    // if(item.reference == 4 || item.reference >= 10){
                    //     Vector3 pos= new Vector3(item.posx,1.58f,item.posz);
                    //     List<Vector3> posVec = new List<Vector3>();
                    //     posVec.Add(pos);
                    //     positions.Insert(item.reference, posVec);
                    //     //Debug.Log("1: "+pos);
                    //     posVec.Clear();
                    // }
                    Vector3 pos = new Vector3(item.posx*10,1.58f,item.posz*10);
                    Debug.Log(pos);
                    if(activeCars.ContainsKey(item.reference)){
                        activeCars[item.reference].GetComponent<CarController>().newPosition = pos;

                    } else {
                        activeCars.Add(item.reference,Instantiate(cars,pos,Quaternion.Euler(0, 0, 0)));
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
        /*
         *    5 -------- 100
         *    timer ----  ?
         */
        timer -= Time.deltaTime;
        dt = 1.0f - (timer / timeToUpdate);

        if(timer < 0)
        {
#if UNITY_EDITOR
            timer = timeToUpdate; // reset the timer
            Vector3 fakePos = new Vector3(3.44f, 1.5f, -15.707f);
            string json = EditorJsonUtility.ToJson(fakePos);
            StartCoroutine(SendData(json));
#endif
        }

    //HACER EL FOR PARA lightS1 y todos esos y cambiar las luces

    }
}
