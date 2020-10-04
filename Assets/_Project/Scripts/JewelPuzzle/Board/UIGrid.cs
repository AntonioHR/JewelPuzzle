using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AntonioHR.JewelPuzzle
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class UIGrid: MonoBehaviour
    {
        //Inspector Variables
        public Vector2Int size = new Vector2Int(9,9);
        
        //Public Properties
        
        //Public Functions
        public RectTransform GetSlot(int x, int y)
        {
            return slots[x, y];
        }
        public RectTransform GetSlot(Vector2Int pos)
        {
            return slots[pos.x, pos.y];
        }
        
        //Unity Messages
        private void Awake()
        {
            SetupLayout();

            SetupSlots();
        }

        #region Private Functions
        private void SetupLayout()
        {
            gridLayout = GetComponent<GridLayoutGroup>();
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = size.x;
        }
        private void SetupSlots()
        {
            slots = new RectTransform[size.x, size.y];
            
            for (int row = 0; row < size.y; row++)
            {
                for (int col = 0; col < size.x; col++)
                {
                    RectTransform slot = new GameObject($"Slot [{col},{row}]", typeof(RectTransform)).GetComponent<RectTransform>();
                    slot.parent = transform;
                    slots[col, row] = slot;
                }
            }
        }
        #endregion

        #region Private Properties
        #endregion

        #region Private Variables
        private RectTransform[,] slots;
        private GridLayoutGroup gridLayout;
        #endregion
    }
}