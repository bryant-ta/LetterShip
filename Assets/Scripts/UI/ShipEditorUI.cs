using TMPro;
using UnityEngine;

public class ShipEditorUI : MonoBehaviour {
    [SerializeField] TMP_InputField shipNameField;

    void Awake() {
        Ship ship = Factory.Instance.CreateBaseShip(Vector3.zero);
        Ref.Player.SetShip(ship);

        Ref.Player.Input.InputCancel += Trash;

        // ship.transform.Rotate(new Vector3(0,0,45));
    }

    public void SpawnFrame(int frameID) {
        if (Ref.Player.IsHolding) return;

        Frame frame = Factory.Instance.CreateFrame(frameID, Vector3.zero);
        GameObject salvageObj = Factory.Instance.CreateSalvage(Vector3.zero);
        frame.transform.parent = salvageObj.transform;

        ClickInputArgs clickInputArgs = new ClickInputArgs {
            CursorPos = Input.mousePosition,
            TargetCol = frame.BodyCol,
            TargetObj = frame.gameObject
        };

        Ref.Player.GrabBit(clickInputArgs);
    }

    public void Trash() {
        if (!Ref.Player.IsHolding) return;
        Ref.Player.TrashBit();
    }

    public void SaveShip() { Factory.Instance.SaveShip(shipNameField.text); }

    public void LoadShip() { Factory.Instance.LoadShip(shipNameField.text); }

    public void ResetShip() { Ref.Player.Ship.transform.position = Vector3.zero; }
}