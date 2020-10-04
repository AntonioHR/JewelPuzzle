using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AntonioHR.JewelPuzzle
{
    [RequireComponent(typeof(UIGrid))]
    public class PuzzleBoard: MonoBehaviour
    {
        //Inspector Variables
        [SerializeField] Vector2Int size = new Vector2Int(9,9);
        [SerializeField] PieceColor[] pieceColors;
        
        //Public Properties
        public Vector2Int Size =>size;
        public PieceColor[] PieceColors =>pieceColors;
        
        //Public Functions
        public void Attach(Piece piece, int x, int y, bool alsoPlaceInPosition = false)
        {
            Debug.Assert(pieces[x,y] == null, $"There's already a piece in position ({x}, {y})", pieces[x,y]);
            pieces[x,y] = piece;

            piece.transform.parent = grid.GetSlot(x, y);
            if(alsoPlaceInPosition)
                piece.transform.localPosition = Vector2.zero;
        }

        public IEnumerable<PieceColor> AvailableColorsAt(int x, int y)
        {
            var r =  pieceColors.Where(color => !HasMatchPotentialFor(color, x, y));
            return r;
        }

        public void Detach(Piece piece, int x, int y)
        {   
            Debug.Assert(pieces[x,y] != null, $"There's already no piece in position ({x}, {y})", pieces[x,y]);
            pieces[x,y] = null;
        }
        public bool CheckForMatches(out Piece[] toBreak)
        {
            throw new NotImplementedException();
        }
        
        public Piece GetPieceSafe(int x, int y)
        {
            if(x < 0 || y < 0 || x >= pieces.GetLength(0) || y >= pieces.GetLength(1))
                return null;
            return pieces[x,y];
        }
        
        //Unity Messages
        private void Awake()
        {
            pieces = new Piece[size.x, size.y];
            grid = GetComponent<UIGrid>();
        }

        #region Private Functions
        private bool HasMatchPotentialFor(PieceColor color, int x, int y)
        {
             bool hasMatchLeft = GetPieceSafe(x-1, y)?.Color == color
                                        && GetPieceSafe(x-2, y)?.Color == color;
                                        
            bool hasMatchUp = GetPieceSafe(x, y-1)?.Color == color
                                    && GetPieceSafe(x, y-2)?.Color == color;
                                    
            bool hasMatchRight = GetPieceSafe(x+1, y)?.Color == color
                                    && GetPieceSafe(x+2, y)?.Color == color;
                                    
            bool hasMatchDown = GetPieceSafe(x, y+1)?.Color == color
                                    && GetPieceSafe(x, y+2)?.Color == color;
            return hasMatchLeft || hasMatchUp || hasMatchRight || hasMatchDown;
        }
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        private Piece[,] pieces;
        private UIGrid grid;

        #endregion
    }
}