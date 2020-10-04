using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AntonioHR.JewelPuzzle
{
    public class Piece: MonoBehaviour
    {

        //Inspector Variables
        [SerializeField] Image img;
        //Public Properties
        public PieceColor Color { get; private set; }

        //Public Functions

        //Unity Messages
        private void Start()
        {
        
        }

        public void Initialize(PieceColor color)
        {
            this.Color = color;
            img.sprite = color.sprite;
        }

        #region Private Functions
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        #endregion
    }
}