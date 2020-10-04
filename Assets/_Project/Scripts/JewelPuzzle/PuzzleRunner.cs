using System;
using System.Collections;
using System.Collections.Generic;
using AntonioHR.Common;
using UnityEngine;

namespace AntonioHR.JewelPuzzle
{
    public class PuzzleRunner : MonoBehaviour
    {
        //Inspector Variables

        //Public Properties

        //Public Functions

        //Unity Messages
        private void Start()
        {
            board = FindObjectOfType<PuzzleBoard>();
            input = FindObjectOfType<PuzzleInput>();
            input.Dragged+=OnDrag;
        }

        private void OnDrag(Piece from, Piece to)
        {
            if(board.IsBusy)
                return;

            if(from.IsAdjacentTo(to))
            {

                board.StartSwitch(from, to);
            }
        }

        #region Private Functions
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        private PuzzleBoard board;
        private PuzzleInput input;

        #endregion
    }
}