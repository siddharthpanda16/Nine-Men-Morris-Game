using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    public class MiniMaxOpening
    {       
        public List<char[]> GenerateAdd(char[] board)
        {
            BoardCommon b = new BoardCommon();
            List<char[]> piecesListAdd = new List<char[]>();

            char[] copyOfBoard;
            
            for(int ind=0;ind<board.Length;ind++)
            {
                if(board[ind]=='x')
                {
                    copyOfBoard = (char[])board.Clone();
                    copyOfBoard[ind] = 'W';

                    if(b.CloseMill(ind,copyOfBoard))
                    {
                        piecesListAdd = b.GenerateRemove(copyOfBoard, piecesListAdd);
                    }
                    else
                    {
                        piecesListAdd.Add(copyOfBoard);
                    }
                }
            }
            return piecesListAdd;
        }

      
        public int StaticEstimationOpening(char[] board)
        {
            int numWhitePieces = 0;
            int numBlackPieces = 0;

            char[] copyOfBoard = (char[])board.Clone();

            for(int ind=0;ind<board.Length;ind++)
            {
                numWhitePieces= copyOfBoard[ind] == 'W' ? numWhitePieces + 1: numWhitePieces;
                numBlackPieces = copyOfBoard[ind] == 'B' ? numBlackPieces + 1 : numBlackPieces;
            }
            return numWhitePieces - numBlackPieces;
        }
        
    }
}
