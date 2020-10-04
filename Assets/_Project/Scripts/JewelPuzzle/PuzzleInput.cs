using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AntonioHR.JewelPuzzle
{
    public delegate void PieceClickHandler(Piece piece);
    public delegate void PieceDragHandler(Piece from, Piece to);
    public class PuzzleInput: MonoBehaviour
    {
        public event PieceClickHandler ClickedOnPiece;
        public event Action ClickedEmpty;
        public event PieceDragHandler Dragged;
        //Inspector Variables

        //Public Properties

        //Public Functions

        //Unity Messages
        private void Start()
        {
            raycaster = FindObjectOfType<GraphicRaycaster>();
            eventSystem = FindObjectOfType<EventSystem>();
        }
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                touchStartPiece = GetHoveredPiece();

                if (touchStartPiece == null)
                    ClickedEmpty?.Invoke();
            }
            else if(touchStartPiece != null)
            {
                Piece hovered = GetHoveredPiece();
                
                //If is on the same piece
                if(hovered == touchStartPiece)
                {
                    if(!Input.GetMouseButton(0))
                    {
                        //clicked
                        ClickedOnPiece?.Invoke(touchStartPiece);
                        touchStartPiece = null;
                    }
                } 
                //If Dragged to another piece
                else if(hovered != null)
                {
                    Dragged?.Invoke(touchStartPiece, hovered);
                    touchStartPiece = null;
                }
            }
        }

        #region Private Functions
        private Piece GetHoveredPiece()
        {
            var pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            return results.Select(r=>r.gameObject.GetComponent<Piece>())
                        .Where(p=>p!=null)
                        .FirstOrDefault();
        }

        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        private GraphicRaycaster raycaster;
        private EventSystem eventSystem;
        private Piece touchStartPiece;
        #endregion
    }
}