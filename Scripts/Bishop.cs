using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chessman
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];

        Chessman c;
        int i, j;

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