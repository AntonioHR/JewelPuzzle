using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AntonioHR.Common;
using UnityEngine;
using UnityEngine.UI;

namespace AntonioHR.JewelPuzzle
{
    [RequireComponent(typeof(UIGrid))]
    public class PuzzleBoard: MonoBehaviour
    {
        //Inspector Variables
        [SerializeField] private float pieceMoveTime = .25f;
        [SerializeField] private float pieceBreakTime = .25f;
        [SerializeField] Vector2Int size = new Vector2Int(9,9);
        
        [SerializeField] Piece piecePrefab;
        [SerializeField] PieceColor[] pieceColors;
        
        //Public Properties
        public Vector2Int Size =>size;
        public PieceColor[] PieceColors =>pieceColors;

        public UIGrid Grid => grid;

        public bool IsBusy { get; private set; }


        //Public Functions
        
        public void StartSwitch(Piece from, Piece to)
        {
            StartCoroutine(SwitchCoroutine(from, to));
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
            BuildStartingBoard();
        }

        //Coroutines



        private IEnumerator SwitchCoroutine(Piece from, Piece to)
        {
            IsBusy = true;
            Switch(from, to);

            StartCoroutine(MovePieceTo(from, from.Position.Value, pieceMoveTime));
            StartCoroutine(MovePieceTo(to, to.Position.Value, pieceMoveTime));

            yield return new WaitUntil(()=>movingPieces == 0);

            if(CheckForMatches())
            {
                //Run Turn
                var toBreak = currentMatches.ToArray();
                currentMatches.Clear();
                StartCoroutine(BreakAndFallCoroutine(toBreak));
            } else
            {
                Switch(from, to);
                StartCoroutine(MovePieceTo(from, from.Position.Value, pieceMoveTime));
                StartCoroutine(MovePieceTo(to, to.Position.Value, pieceMoveTime));
                IsBusy = false;
            }
        }

        private IEnumerator BreakAndFallCoroutine(Piece[] toBreak)
        {
            foreach (var piece in toBreak)
            {
                Detach(piece);

                StartCoroutine(piece.transform.PerformScale(Vector3.zero, pieceBreakTime));
            }

            yield return new WaitForSeconds(pieceBreakTime);

            // PerformFallAndSpawn();
            IsBusy = false;
        }

        //This would be much easier with tweening, but... oh well
        private IEnumerator MovePieceTo(Piece piece, Vector2Int boardPos, float time)
        {
            movingPieces++;
            Vector3 worldEnd = grid.GetSlot(boardPos).position;
            yield return piece.transform.PerformMove(worldEnd, time);
            movingPieces--;
        }


        #region Private Functions

        #region Basic Operations
        private void Attach(Piece piece, int x, int y, bool alsoPlaceInPosition = false)
        {
            Debug.Assert(pieces[x,y] == null, $"There's already a piece in position ({x}, {y})", pieces[x,y]);
            pieces[x,y] = piece;

            piece.transform.parent = grid.GetSlot(x, y);
            piece.Position = new Vector2Int(x,y);
            if(alsoPlaceInPosition)
                piece.transform.localPosition = Vector2.zero;
        }
        private void Switch(Piece from, Piece to)
        {
            Vector2Int fromPos = from.Position.Value;
            Vector2Int toPos = to.Position.Value;

            Detach(from);
            Detach(to);

            Attach(from, toPos.x, toPos.y, false);
            Attach(to, fromPos.x, fromPos.y, false);
        }
        private void Detach(Piece piece)
        {   
            Debug.Assert(piece.Position != null);
            Vector2Int pos  = piece.Position.Value;
            Debug.Assert(pieces[pos.x,pos.y] == piece);

            DetachAt(pos.x, pos.y);
        }
        private void DetachAt(int x, int y)
        {
            Debug.Assert(pieces[x,y] != null, $"There's already no piece in position ({x}, {y})");
            pieces[x,y].Position = null;
            pieces[x,y] = null;
        }


        #endregion
        private void BuildStartingBoard()
        {
            Vector2Int size = Size;
            Canvas canvas = GetComponentInParent<Canvas>();
            for (int row = 0; row < size.y; row++)
            {
                for (int col = 0; col < size.x; col++)
                {
                    //We'll check right and down even though it must have no pieces yet, as this code is easier to understand than that would be
                    var options = AvailableColorsAt(col, row);
                    PieceColor color =  options.RandomItem();

                    //spawn inside the board canvas so there's no problem with canvas scaling
                    Piece piece = Instantiate(piecePrefab, canvas.transform);
                    piece.Initialize(color);

                    Attach(piece, col, row, alsoPlaceInPosition:true);
                }
            }
        }
        private IEnumerable<PieceColor> AvailableColorsAt(int x, int y)
        {
            var r =  pieceColors.Where(color => !HasMatchPotentialFor(color, x, y));
            return r;
        }
        private bool CheckForMatches()
        {
            List<Piece> result = new List<Piece>();
            currentMatches.Clear();
            for (int y = 0; y < pieces.GetLength(1); y++)
            {
                for (int x = 0; x < pieces.GetLength(0); x++)
                {
                    CheckForMatchesAt(x,y);
                }
            }
            return currentMatches.Any();
        }
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
        private void CheckForMatchesAt(int x, int y )
        {
            PieceColor color = pieces[x, y].Color;

            if(GetPieceSafe(x-1, y)?.Color == color
            && GetPieceSafe(x-2, y)?.Color == color)
            {
                currentMatches.Add(GetPieceSafe(x, y));
                currentMatches.Add(GetPieceSafe(x-1, y));
                currentMatches.Add(GetPieceSafe(x-2, y));
            }
            if(GetPieceSafe(x+1, y)?.Color == color
            && GetPieceSafe(x+2, y)?.Color == color)
            {
                currentMatches.Add(GetPieceSafe(x, y));
                currentMatches.Add(GetPieceSafe(x+1, y));
                currentMatches.Add(GetPieceSafe(x+2, y));
            }
            if(GetPieceSafe(x, y-1)?.Color == color
            && GetPieceSafe(x, y-2)?.Color == color)
            {
                currentMatches.Add(GetPieceSafe(x, y));
                currentMatches.Add(GetPieceSafe(x, y-1));
                currentMatches.Add(GetPieceSafe(x, y-2));
            }
            if(GetPieceSafe(x, y+1)?.Color == color
            && GetPieceSafe(x, y+2)?.Color == color)
            {
                currentMatches.Add(GetPieceSafe(x, y));
                currentMatches.Add(GetPieceSafe(x, y+1));
                currentMatches.Add(GetPieceSafe(x, y+2));
            }
        }
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        private Piece[,] pieces;
        private UIGrid grid;

        private HashSet<Piece> currentMatches = new HashSet<Piece>();
        private int movingPieces;

        #endregion
    }
}