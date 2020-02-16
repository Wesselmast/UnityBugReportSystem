using UnityEngine;

public class DebugCamera : MonoBehaviour {
    private void OnEnable() {
        if (!Application.isEditor || Application.isPlaying) {
            Destroy(gameObject); 
        }
    }
}