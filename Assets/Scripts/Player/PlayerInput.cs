using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public LayerMask PointLayer;

    public event Action<char> InputKeyDown;
    public event Action<char> InputKeyUp;

    public event Action<ClickInputArgs> InputPrimaryDown;
    public event Action<ClickInputArgs> InputPrimaryUp;
    public event Action<ClickInputArgs> InputSecondaryDown;
    public event Action<ClickInputArgs> InputSecondaryUp;
    public event Action<ClickInputArgs> InputPoint;

    Camera mainCam;

    void Awake() { mainCam = Camera.main; }

    void Update() {
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) {
            if (IsAlphabetKey(keyCode)) {
                if (Input.GetKeyDown(keyCode)) {
                    InputKeyDown?.Invoke(KeyCodeToChar(keyCode));
                } else if (Input.GetKeyUp(keyCode)) {
                    InputKeyUp?.Invoke(KeyCodeToChar(keyCode));
                }
            }
        }

        // Purely for dragging bits and rotating, etc.
        ClickInputArgs clickInputArgs = ClickInputArgsRaycast(Input.mousePosition);
        InputPoint?.Invoke(clickInputArgs);

        if (clickInputArgs.TargetObj == null) {
            return;
        }
        
        if (Input.GetButtonDown("Fire1")) {
            InputPrimaryDown?.Invoke(clickInputArgs);
        } else if (Input.GetButtonUp("Fire1")) {
            InputPrimaryUp?.Invoke(clickInputArgs);
        }

        if (Input.GetButtonDown("Fire2")) {
            InputSecondaryDown?.Invoke(clickInputArgs);
        } else if (Input.GetButtonUp("Fire2")) {
            InputSecondaryUp?.Invoke(clickInputArgs);
        }

    }

    bool IsAlphabetKey(KeyCode keyCode) { return keyCode >= KeyCode.A && keyCode <= KeyCode.Z; }

    char KeyCodeToChar(KeyCode keyCode) {
        if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z) {
            return (char) ('a' + (keyCode - KeyCode.A)); // lowercase
        }
        return '\0';
    }

    public ClickInputArgs ClickInputArgsRaycast(Vector2 cursorPos) {
        ClickInputArgs clickInputArgs = new();
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, 0));
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, PointLayer);
        if (hit.collider != null) {
            clickInputArgs.TargetCol = hit.collider;
            clickInputArgs.TargetObj = hit.collider.gameObject;
        }
        
        clickInputArgs.CursorPos = worldPos;

        return clickInputArgs;
    }
    // bool IsPointerOverUIElement(Vector2 cursorPos) {
    //     PointerEventData pointerEventData = new PointerEventData(eventSystem);
    //     pointerEventData.position = cursorPos;
    //
    //     List<RaycastResult> raycastResults = new List<RaycastResult>();
    //     graphicRaycaster.Raycast(pointerEventData, raycastResults);
    //
    //     return raycastResults.Count > 0;
    // }
}