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

    public void SpawnFrame(int id) {
        if (Ref.Player.IsHolding) return;

        Frame frame = Factory.Instance.CreateFrame(id, Vector3.zero);
        GameObject salvageObj = Factory.Instance.CreateSalvage(Vector3.zero);
        frame.transform.parent = salvageObj.transform;

        ClickInputArgs clickInputArgs = new ClickInputArgs {
            CursorPos = Input.mousePosition,
            TargetCol = frame.BodyCol,
            TargetObj = frame.gameObject
        };

        Ref.Player.GrabBit(clickInputArgs);
    }
    
    public void SpawnWeapon(int id) {
        if (Ref.Player.IsHolding) return;

        Weapon weapon = Factory.Instance.CreateWeapon(id, Vector3.zero);
        GameObject salvageObj = Factory.Instance.CreateSalvage(Vector3.zero);
        weapon.transform.parent = salvageObj.transform;

        ClickInputArgs clickInputArgs = new ClickInputArgs {
            CursorPos = Input.mousePosition,
            TargetCol = weapon.BodyCol,
            TargetObj = weapon.gameObject
        };

        Ref.Player.GrabBit(clickInputArgs);
    }
    
    public void SpawnThruster(int id) {
        if (Ref.Player.IsHolding) return;

        Thruster thruster = Factory.Instance.CreateThruster(id, Vector3.zero);
        GameObject salvageObj = Factory.Instance.CreateSalvage(Vector3.zero);
        thruster.transform.parent = salvageObj.transform;

        ClickInputArgs clickInputArgs = new ClickInputArgs {
            CursorPos = Input.mousePosition,
            TargetCol = thruster.BodyCol,
            TargetObj = thruster.gameObject
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