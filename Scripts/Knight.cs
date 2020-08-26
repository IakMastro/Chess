using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];

        // Up left
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);
        
        // Up right
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);
        
        // Right up
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);
        
        // Right right
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);
        
        // Down left
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);
        
        // Down right
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);
        
        // Left up
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);
        
        // Left down
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);
        
        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        Chessman c;
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.Chessmen[x, y];
            if (c == null || isWhite != c.isWhite)
                r[x, y] = true;
        }
    }
}