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
        [SerializeField] Transform cursor;

        //Public Properties

        //Public Functions

        //Unity Messages
        private void Start()
        {
            board = FindObjectOfType<PuzzleBoard>();
            input = FindObjectOfType<PuzzleInput>();
            input.Dragged+=OnDrag;
            input.ClickedOnPiece += ClickedOnPiece;
            input.ClickedEmpty += Deselect;
            Deselect();
        }

        private void Deselect()
        {
            cursor.gameObject.SetActive(false);
            selected = null;
        }

        private void ClickedOnPiece(Piece piece)
        {
            if(board.IsBusy)
                return;

            if(selected == null)
            {
                cursor.gameObject.SetActive(true);
                cursor.gameObject.transform.position = piece.transform.position;
                selected = piece;
            } else 
            {
                if(selected == piece)
                {
                    Deselect();
                } else
                {
                    OnDrag(selected, piece);
                }
            }

        }

        private void OnDrag(Piece from, Piece to)
        {
            if(board.IsBusy)
                return;

            if(from.IsAdjacentTo(to))
            {

                board.StartSwitch(from, to);
            }
            Deselect();
        }

        #region Private Functions
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        private PuzzleBoard board;
        private PuzzleInput input;
        private Piece selected;

        #endregion
    }
}