namespace IsolatedMind {
    namespace Input {
        using System;
        using UnityEngine;

        public class PlayerInput : MonoBehaviour {
            public static event Action<float, float> OnMovement = delegate { };
            public static event Action<bool> OnRunning = delegate { };
            public static event Action<float, float> OnLookAround = delegate { };
            public static event Action OnJump = delegate { };
            public static event Action<int> OnWeaponChange = delegate { };
            public static event Action OnShoot = delegate { };

            public static event Action SwitchMarkers = delegate { };

            public static bool ReportBug = false;
            public static Vector3 MousePosition;

            private void Update() {
                HandleLookAround();
                HandleMovement();
                HandleWeaponChange();
                if (Input.GetKeyDown(KeyCode.Space)) OnJump();
                if (Input.GetKey(KeyCode.Mouse0)) OnShoot();
                if (Input.GetKeyDown(KeyCode.F1)) ReportBug = !ReportBug;
                if (Input.GetKeyDown(KeyCode.F5)) SwitchMarkers();
                MousePosition = Input.mousePosition;
            }

            private void HandleMovement() {
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = Input.GetAxis("Vertical");
                if (horizontalMove != 0.0f || verticalMove != 0.0f) {
                    OnMovement(horizontalMove, verticalMove);
                }
                OnRunning(Input.GetKey(KeyCode.LeftShift));
            }

            private void HandleLookAround() {
                if(!ReportBug) OnLookAround(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            }

            private void HandleWeaponChange() {
                if (Input.GetKeyDown(KeyCode.Alpha1)) OnWeaponChange(0);
                if (Input.GetKeyDown(KeyCode.Alpha2)) OnWeaponChange(1);
                if (Input.GetKeyDown(KeyCode.Alpha3)) OnWeaponChange(2);
            }
        }
    }
}