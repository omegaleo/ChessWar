using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board board;

    private void Start()
    {
        board.Create();
        PieceManager.instance.Setup(board);
    }
}
