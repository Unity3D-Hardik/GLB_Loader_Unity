using UnityEngine;
using Siccity.GLTFUtility;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Testing script that help to load single or multiple GLB model in Unity3D Editor as well as Android device
///
///
/// Model position default 0,0,0 scale 0,0,0
///
/// for demo purpose nothing chnage as of not staticaly value updated for demo buttons
/// 
/// </summary>
///

[System.Serializable]
public class Pose {

    public Vector3 position ;
    public Vector3 scale;
    public Quaternion rotation;

}


public class ImportGLTF : MonoBehaviour
{


    [SerializeField]
    InputField urlpath;
    [SerializeField]
    Button loadButton;


    public string[] predefineModes;
    public GameObject loader;

    int loadmodel = -1;

    public Pose[] modelPose;

    private void Start()
    {
        loadButton.onClick.AddListener(LoadModel);
    }


    public void downloadPredefineModels(int index) {

        loadmodel = index;
        string filename = "Model_" + index + ".glb";
        StartCoroutine(DownloadFile(predefineModes[index], filename));
    }


    public void LoadModel() {
        loadmodel = -1;
        if (!string.IsNullOrWhiteSpace(urlpath.text)) {
            StartCoroutine(DownloadFile(urlpath.text, "model.glb"));
        }

    }


    IEnumerator DownloadFile(string filePath,string filename)
    {
        loader.SetActive(true);
        var uwr = new UnityWebRequest(filePath, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.persistentDataPath, filename);
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success) {

            //Todo: Dispaly Error dialog box here.
            Debug.LogError(uwr.error);
        }
        else {
               Debug.Log("File successfully downloaded and saved to " + path);
            //Todo: For different model type error handler or popup set 
               ImportGLTFModel(path);
        }
    }


    public GameObject obj;
    void ImportGLTFModel(string filepath)
    {
        if (obj != null) {
            Destroy(obj);
        }

        obj = Importer.LoadFromFile(filepath);



        //Todo : For testing Obj scale set staticaly


        if (loadmodel < 0)
        {
            obj.transform.SetPositionAndRotation(modelPose[4].position, modelPose[4].rotation);
            obj.transform.localScale = modelPose[4].scale;
        }
        else {
            obj.transform.SetPositionAndRotation(modelPose[loadmodel].position, modelPose[loadmodel].rotation);
            obj.transform.localScale = modelPose[loadmodel].scale;
        }

        loader.SetActive(false);
    }


    //Todo: Multiple model load multithread option
    void ImportGLTFAsync(string filepath)
    {
        Importer.ImportGLTFAsync(filepath, new ImportSettings(), OnFinishAsync);
    }

    private void OnFinishAsync(GameObject arg1, AnimationClip[] arg2)
    {
        Debug.Log("Finished importing " + arg1.name);
    }

    
}
