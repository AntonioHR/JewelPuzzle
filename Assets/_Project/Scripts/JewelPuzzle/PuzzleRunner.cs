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
        [SerializeField] private float pieceMoveTime = .25f;
        [SerializeField] private float pieceBreakTime = .25f;

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
            if(isBusy)
                return;

            if(from.IsAdjacentTo(to))
            {
                StartCoroutine(PerformSwitch(from, to));
            }
        }

        private IEnumerator PerformSwitch(Piece from, Piece to)
        {
            isBusy = true;
            board.Switch(from, to);

            StartCoroutine(MovePieceTo(from, from.Position.Value, pieceMoveTime));
            StartCoroutine(MovePieceTo(to, to.Position.Value, pieceMoveTime));

            yield return new WaitUntil(()=>movingPieces == 0);
            
            Piece[] toBreak;

            if(board.CheckForMatches(out toBreak))
            {
                //Run Turn
                StartCoroutine(BreakAndFall(toBreak));
            } else
            {
                board.Switch(from, to);
                StartCoroutine(MovePieceTo(from, from.Position.Value, pieceMoveTime));
                StartCoroutine(MovePieceTo(to, to.Position.Value, pieceMoveTime));
                isBusy = false;
            }
        }

        private IEnumerator BreakAndFall(Piece[] toBreak)
        {
            foreach (var piece in toBreak)
            {
                board.Detach(piece);

                StartCoroutine(piece.transform.PerformScale(Vector3.zero, pieceBreakTime));
            }
            yield return new WaitForSeconds(pieceBreakTime);
            isBusy = false;
        }

        //This would be much easier with tweening, but... oh well
        private IEnumerator MovePieceTo(Piece piece, Vector2Int boardPos, float time)
        {
            movingPieces++;
            Vector3 worldEnd = board.Grid.GetSlot(boardPos).position;
            yield return piece.transform.PerformMove(worldEnd, time);
            movingPieces--;
        }

        #region Private Functions
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        private bool isBusy = false;
        private PuzzleBoard board;
        private PuzzleInput input;
        private int movingPieces;

        #endregion
    }
}