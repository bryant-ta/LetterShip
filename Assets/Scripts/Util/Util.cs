using UnityEngine;

public class Util {
    public static Vector2 IntToDir(int v) {
        switch (v) {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.right;
            case 2:
                return Vector2.down;
            case 3:
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }
    
    public static Quaternion LookRotation2D(Vector2 dir) {
        Vector3 direction3D = new Vector3(dir.x, dir.y, 0);
        return Quaternion.LookRotation(Vector3.forward, direction3D);
    }
}

public struct ClickInputArgs {
    public Vector2 CursorPos;
    public Collider2D TargetCol;
    public GameObject TargetObj;
}
