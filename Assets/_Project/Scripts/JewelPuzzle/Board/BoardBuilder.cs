using System;
using System.Collections;
using System.Collections.Generic;
using AntonioHR.Common;
using UnityEngine;

namespace AntonioHR.JewelPuzzle
{
    public class BoardBuilder: MonoBehaviour
    {
        //Inspector Variables
        [SerializeField] PuzzleBoard board;
        [SerializeField] Piece piecePrefab;
        
        //Public Properties
        
        //Public Functions

        public void BuildBoard()
        {
            Vector2Int size = board.Size;
            Canvas canvas = board.GetComponentInParent<Canvas>();
            for (int row = 0; row < size.y; row++)
            {
                for (int col = 0; col < size.x; col++)
                {
                    //We'll check right and down even though it must have no pieces yet, as this code is easier to understand than that would be
                    var options = board.AvailableColorsAt(col, row);
                    PieceColor color =  options.RandomItem();

                    //spawn inside the board canvas so there's no problem with canvas scaling
                    Piece piece = Instantiate(piecePrefab, canvas.transform);
                    piece.Initialize(color);

                    board.Attach(piece, col, row, alsoPlaceInPosition:true);
                }
            }
        }
        
        //Unity Messages
        private void Start()
        {
            BuildBoard();
        }

        #region Private Functions
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        #endregion
    }
}