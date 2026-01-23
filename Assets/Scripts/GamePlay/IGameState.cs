using UnityEditor;
using UnityEngine;

public interface IGameState
{
    GameState State { get; }
}