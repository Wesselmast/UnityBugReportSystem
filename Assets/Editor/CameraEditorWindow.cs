using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class CameraEditorWindow : EditorWindow {

    private string cameraPostion = string.Empty;
    private string cameraRotation = string.Empty;

    private int positionCount;
    private const int guiOffset = 25;

    private GameObject camera = null;

    [MenuItem("Window/Custom/EasyDebug")]
    private static void OpenWindow() {
        CameraEditorWindow window = GetWindow<CameraEditorWindow>();
        window.titleContent = new GUIContent("EasyDebug");
        window.Show();
    }

    private void OnGUI() {
        positionCount = 1;
        cameraPostion = EditorGUI.TextField(new Rect(3, guiOffset * positionCount++, position.width - 6, 20), "Camera Position: ", cameraPostion);
        cameraRotation = EditorGUI.TextField(new Rect(3, guiOffset * positionCount++, position.width - 6, 20), "Camera Rotation: ", cameraRotation);

        if (GUI.Button(new Rect(3, guiOffset * positionCount++, position.width - 6, 20), "Create Camera")) {

            if (camera != null) {
                if (!EditorUtility.DisplayDialog("Are you sure?",
                    "A debug camera already exists. Do you want to overwrite it?",
                    "Yes",
                    "No")) return;
            }
            else camera = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load("Prefabs/DebugCamera", typeof(GameObject)));
            camera.transform.position = ParseVector(cameraPostion);
            camera.transform.eulerAngles = ParseVector(cameraRotation);
        }
    }

    private Vector3 ParseVector(string input) {
        List<float> parsed = new List<float>();
        foreach (var s in Regex.Split(input, "([-+]?[0-9]*\\.?[0-9]+)")) {
            string thing = s.Replace("(", string.Empty).
                             Replace(",", string.Empty).
                             Replace(")", string.Empty).
                             Replace(" ", string.Empty);
            if (thing == string.Empty) continue;
            if(float.TryParse(thing, out float result)) parsed.Add(float.Parse(thing));
        }
        try {
            return new Vector3(parsed[0], parsed[1], parsed[2]);
        }
        catch {
            throw new System.Exception("You didn't specify a camera postion or rotation!");
        }
    }
}
