using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ref : Singleton<Ref> {
    [SerializeField] Player player;
    public static Player Player => _player;
    static Player _player;

    void Awake() { _player = player; }
}