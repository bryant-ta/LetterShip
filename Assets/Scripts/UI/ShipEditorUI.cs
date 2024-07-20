using UnityEngine;

public class ShipEditorUI : MonoBehaviour {
    [SerializeField] GameObject starterShip;

    void Start() {
        Ship ship = Instantiate(starterShip, Vector3.zero, Quaternion.identity).GetComponent<Ship>();
    }

    public void SpawnFrame(int frameID) {
        Frame frame = Factory.Instance.CreateFrame(0, Vector3.zero);

        ClickInputArgs clickInputArgs = new ClickInputArgs {
            CursorPos = Input.mousePosition,
            TargetCol = frame.BodyCol,
            TargetObj = frame.gameObject
        };

        Ref.Player.GrabBit(clickInputArgs);
    }
}
