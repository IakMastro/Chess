﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];

        Chessman c;
        int i, j;
        
        // Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = BoardManager.Instance.Chessmen[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }
        
        // Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i <= 0)
                break;

            c = BoardManager.Instance.Chessmen[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }
        
        // Up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = BoardManager.Instance.Chessmen[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;

                break;
            }
        }
        
        // Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i <= 0)
                break;

            c = BoardManager.Instance.Chessmen[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;

                break;
            }
        }
        
        // Top left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
                break;

            c = BoardManager.Instance.Chessmen[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;

                break;
            }
        }
        
        // Top right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8)
                break;

            c = BoardManager.Instance.Chessmen[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;

                break;
            }
        }
        
        // Down left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;

            c = BoardManager.Instance.Chessmen[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;

                break;
            }
        }
        
        // Down right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0)
                break;

            c = BoardManager.Instance.Chessmen[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;

                break;
            }
        }
        
        return r;
    }
}
