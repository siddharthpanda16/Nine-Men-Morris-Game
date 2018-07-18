using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    public class ABOpening
    {
        public double _ABMiniMaxEstimate = 0;
        public int _ABPositions_Evaluated = 0;
        public char[] MaxMin(char[] val, int depth, bool isOpening,double alpha, double beta)
        {
            BoardCommon b = new BoardCommon();
            
            ABGame mid = new ABGame();
            if (depth > 0)
            {
                depth--;
                List<char[]> child = new List<char[]>();
                char[] minValue;
                char[] maxValue = new char[50];
                child = isOpening ? GenerateAdd(val) : mid.GenerateMovesMidgameEndgame(val);  //so that midgame move and generate add are called by passing parameter from calling function
  
                double v = Double.NegativeInfinity;

                for (int ind = 0; ind < child.Count; ind++)
                {
                    minValue = MinMax(child[ind], depth, isOpening,alpha, beta);

                    double staticEstimate = isOpening ? StaticEstimationOpening(minValue) : mid.StaticEstimationMidEndGame(minValue, isOpening);
                    if (v < staticEstimate)
                    {
                        v = staticEstimate;
                        _ABMiniMaxEstimate = v;                      
                        maxValue = child[ind];
                    }

                    if (v >= beta)
                    {

                        return maxValue;
                    }
                    else
                    {
                        alpha = Math.Max(v, alpha);
                    }
                }
                return maxValue;
            }
            else if (depth == 0)
            {
                _ABPositions_Evaluated++;
             
            }
            return val;
        }

        public char[] MinMax(char[] val, int depth, bool isOpening, double alpha, double beta)
        {
            BoardCommon b = new BoardCommon();
           
            ABGame mid = new ABGame();
            if (depth > 0)
            {
                depth--;
                List<char[]> blackChild = new List<char[]>();
                char[] minValue = new char[50];
                char[] maxValue;
                blackChild = GenerateBlackMove(val, isOpening);

                double v = Double.PositiveInfinity;

                for (int ind = 0; ind < blackChild.Count; ind++)
                {
                    maxValue = MaxMin(blackChild[ind], depth, isOpening, alpha, beta);

                    double staticEstimate = isOpening ? StaticEstimationOpening(minValue) : mid.StaticEstimationMidEndGame(minValue, isOpening);
                   
                    if (v > staticEstimate)
                    {
                        v = staticEstimate;
                        _ABMiniMaxEstimate = v;
                        minValue = blackChild[ind];
                        
                    }

                    if (v <= alpha)
                    {
                        return minValue;
                    }
                    else
                    {
                        beta = Math.Min(v, beta);
                    }
                }
                return minValue;
            }
            else if (depth == 0)
            {
                _ABPositions_Evaluated++;
            }
            return val;
        }

        public int StaticEstimationOpening(char[] board)
        {
            int numWhitePieces = 0;
            int numBlackPieces = 0;

            char[] copyOfBoard = (char[])board.Clone();

            for (int ind = 0; ind < board.Length; ind++)
            {
                numWhitePieces = copyOfBoard[ind] == 'W' ? numWhitePieces + 1 : numWhitePieces;
                numBlackPieces = copyOfBoard[ind] == 'B' ? numBlackPieces + 1 : numBlackPieces;
            }
            return numWhitePieces - numBlackPieces;
        }

        public List<char[]> GenerateAdd(char[] board)
        {
            BoardCommon b = new BoardCommon();
            List<char[]> piecesListAdd = new List<char[]>();

            char[] copyOfBoard;

            for (int ind = 0; ind < board.Length; ind++)
            {
                if (board[ind] == 'x')
                {
                    copyOfBoard = (char[])board.Clone();
                    copyOfBoard[ind] = 'W';

                    if (b.CloseMill(ind, copyOfBoard))
                    {
                        piecesListAdd = GenerateRemove(copyOfBoard, piecesListAdd);
                    }
                    else
                    {
                        piecesListAdd.Add(copyOfBoard);
                    }
                }
            }
            return piecesListAdd;
        }

        public List<char[]> GenerateRemove(char[] board, List<char[]> l)
        {
            BoardCommon b= new BoardCommon();
            List<char[]> piecesListRemove = l.Select(item => (char[])item.Clone()).ToList();

            for (int ind = 0; ind < board.Length; ind++)
            {
                if (board[ind] == 'B')
                {
                    if (!(b.CloseMill(ind, board)))
                    {
                        char[] copyOfBoard = (char[])board.Clone();                        
                        copyOfBoard[ind] = 'x';             //Removed Black piece from board
                        piecesListRemove.Add(copyOfBoard);
                    }
                    else
                    {
                        char[] copyOfBoard = (char[])board.Clone();
                        piecesListRemove.Add(copyOfBoard);
                    }
                }
            }
            return piecesListRemove;
        }

        public List<char[]> GenerateBlackMove(char[] val, bool isOpening)
        {
            ABGame mid = new ABGame();
            char[] copyOfBoard = (char[])val.Clone();

            for (int ind = 0; ind < copyOfBoard.Length; ind++)
            {
                if (copyOfBoard[ind] == 'W')
                {
                    copyOfBoard[ind] = 'B';
                    continue;
                }
                if (copyOfBoard[ind] == 'B')
                {
                    copyOfBoard[ind] = 'W';
                }
            }

            List<char[]> blackMove = new List<char[]>();
            List<char[]> blackMoveReplace = new List<char[]>();

            blackMove = isOpening ? GenerateAdd(copyOfBoard) : mid.GenerateMovesMidgameEndgame(copyOfBoard);
            foreach (char[] b in blackMove)
            {
                char[] board = b;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 'W')
                    {
                        board[i] = 'B';
                        continue;
                    }
                    if (board[i] == 'B')
                    {
                        board[i] = 'W';
                    }
                }
                blackMoveReplace.Add(b);
            }
            return blackMoveReplace;
        }
    }
}
