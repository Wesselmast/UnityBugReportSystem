using IsolatedMind.Input;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ScreenCapture : PlayerBehaviour {

    private string problemTitle = string.Empty;
    private string problemDescription = string.Empty;
    private int bugID;

    private const string requiredText = "Required";
    private GUIStyle backgroundStyle = new GUIStyle();

    private Vector2 windowSize = new Vector2(450, 300);
    private Vector2 popupSize = new Vector2(400, 250);
    private Camera cam = default;

    private bool popup = false;

    private Marker marker = null;

    protected override void Wake() {
        Texture2D background = new Texture2D(1, 1);
        background.SetPixel(1, 1, new Color32(0, 0, 0, 175));
        background.Apply();
        backgroundStyle.normal.background = background;
        cam = Camera.main;
    }

    private void OnEnable() {
        bugID = PlayerPrefs.GetInt("BugID");
    }

    private void OnDisable() {
        PlayerPrefs.SetInt("BugID", bugID);
    }

    private void OnGUI() {
        if (!PlayerInput.ReportBug) {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;

        if (popup) {
            GUI.Window(0, new Rect(Screen.width / 2 - popupSize.x / 2, Screen.height / 2 - popupSize.y / 2, popupSize.x, popupSize.y),
                (int windowID) => {
                    GUILayout.Label("Thank you for reporting a bug, this will be sent to our database!");
                    if (GUI.Button(new Rect(popupSize.x / 2 - 60, popupSize.y - 60, 120, 20), "No Problem!")) {
                        popup = false;
                        PlayerInput.ReportBug = false;
                    }
                }, "Thank you!");
            return;
        }

        GUILayout.BeginArea(new Rect(Screen.width / 2 - windowSize.x / 2, Screen.height / 2 - windowSize.y / 2, windowSize.x, windowSize.y), backgroundStyle);

        int textAreaSize = 250;
        GUI.Label(new Rect(20, 40, 100, 20), "Problem: ");
        problemTitle = GUI.TextField(new Rect(windowSize.x - (textAreaSize + 20), 40, textAreaSize, 20), problemTitle, 15);
        GUI.Label(new Rect(20, 80, 100, 20), "Description: ");
        problemDescription = GUI.TextArea(new Rect(windowSize.x - (textAreaSize + 20), 80, textAreaSize, 80), problemDescription, 150);

        if (GUI.Button(new Rect(windowSize.x / 2 - 80, 200, 160, 20), "Send Bugreport")) {
            bool correct = true;
            if (CheckString(problemTitle)) {
                problemTitle = requiredText;
                correct = false;
            }
            if (CheckString(problemDescription)) {
                problemDescription = requiredText;
                correct = false;
            }
            if (!correct) return;
            StartCoroutine(SendData());
            popup = true;
        }
        GUILayout.EndArea();
    }

    private bool CheckString(string text) {
        return text.Replace(" ", string.Empty) == string.Empty || text.Replace(" ", string.Empty) == requiredText;
    }

    private IEnumerator SendData() {
        WWWForm form = new WWWForm();
        Bug bug = new Bug(++bugID, problemTitle, problemDescription);

        if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit)) {
            marker = (Marker)PrefabUtility.InstantiatePrefab((Marker)Resources.Load("Prefabs/Marker", typeof(Marker)));
            marker.transform.position = hit.point;
            marker.MakeLookAtPlayer();
            marker.Bug = bug;
        }
        form.AddField("entry.546547628", bug.ID);
        form.AddField("entry.2139304620", bug.Title);
        form.AddField("entry.666100114", bug.Description);
        form.AddField("entry.491665277", cam.transform.position.ToString());
        form.AddField("entry.1609531456", cam.transform.eulerAngles.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("https://docs.google.com/forms/d/e/1FAIpQLSf-k0B7KlFdunCNMRgzjpl3xqZJtkxKVNR6KG3bVL5FEFvIjw/formResponse", form)) {
            yield return www.SendWebRequest();
            problemTitle = string.Empty;
            problemDescription = string.Empty;

            if (www.isNetworkError || www.isHttpError) {
                throw new System.Exception(www.error);
            }
        }
    }
}
public struct Bug {
    public int ID;
    public string Title;
    public string Description;

    public Bug(int ID, string title, string description) {
        this.ID = ID;
        this.Title = title;
        this.Description = description;
    }
}