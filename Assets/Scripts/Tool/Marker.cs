using UnityEngine;
using IsolatedMind.Input;
using System.Linq;

public class Marker : MonoBehaviour {
    public Bug Bug { get; set; }

    private Player player = null;
    private Behaviour[] behaviours = null;
    private SpriteRenderer spriteRenderer = null;

    private bool disabled = false;

    private void Awake() {
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        behaviours = GetComponents<Behaviour>().Where(c => c != this).ToArray();
    }

    private void OnEnable() {
        PlayerInput.OnMovement += LookAt;
        PlayerInput.SwitchMarkers += SwitchMarkers;
    }

    private void OnDisable() {
        PlayerInput.OnMovement -= LookAt;
        PlayerInput.SwitchMarkers -= SwitchMarkers;
    }

    private void SwitchMarkers() {
        if (!disabled) {
            behaviours.All(c => c.enabled = false);
            spriteRenderer.enabled = false;
            disabled = true;
            return;
        }
        behaviours.All(c => c.enabled = true);
        spriteRenderer.enabled = true;
        disabled = false;
    }

    private void LookAt(float x, float y) {
        transform.LookAt(player.transform);
    }

    public void MakeLookAtPlayer() {
        LookAt(1, 1);
    }
}
