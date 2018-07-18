using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    public class MiniMaxGame
    {
        
        public List<char[]> GenerateMovesMidgameEndgame(char[] location)
        {
            List<char[]> piecesToMove = new List<char[]>();

            int numWhitePieces = 0;

            for (int ind = 0; ind < location.Length; ind++)
            {
                if (location[ind] == 'W')
                {
                    numWhitePieces++;
                }
            }

            if (numWhitePieces == 3)
            {
                piecesToMove = GenerateHopping(location);
                return piecesToMove;
            }
            else
            {
                piecesToMove = GenerateMove(location);
                return piecesToMove;
            }
        }

        public List<char[]> GenerateHopping(char[] location)
        {
            BoardCommon b = new BoardCommon();
            List<char[]> hoppingList = new List<char[]>();
            
            char[] copyOfBoard = new char[50];
            for (int ind = 0; ind < location.Length; ind++)
            {
                if (location[ind] == 'W')
                {
                    for (int j = 0; j < location.Length; j++)
                    {
                        if (location[j] == 'x')
                        {
                            copyOfBoard = (char[])location.Clone();
                            copyOfBoard[ind] = 'x';
                            copyOfBoard[j] = 'W';
                            if (b.CloseMill(j, copyOfBoard))
                            {
                                b.GenerateRemove(copyOfBoard, hoppingList);
                            }
                            else
                            {
                                hoppingList.Add(copyOfBoard);
                            }
                        }
                    }
                }
            }
            return hoppingList;
        }

        public List<char[]> GenerateMove(char[] location)
        {
            BoardCommon b = new BoardCommon();
            List<char[]> piecesToMove = new List<char[]>();
       
            char[] copyOfBoard = new char[50];
            int[] neighboursList;
            for (int ind = 0; ind < location.Length; ind++)
            {
                if (location[ind] == 'W')
                {
                    neighboursList = b.Neighbours(ind);
                    foreach (int neighbour in neighboursList)
                    {
                        if (location[neighbour] == 'x')
                        {
                            copyOfBoard = (char[]) location.Clone();
                            copyOfBoard[ind] = 'x';
                            copyOfBoard[neighbour] = 'W';

                            if (b.CloseMill(neighbour, copyOfBoard))
                            {
                                b.GenerateRemove(copyOfBoard, piecesToMove);
                            }
                            else
                            {
                                piecesToMove.Add(copyOfBoard);
                            }
                        }
                    }
                }
            }
            return piecesToMove;
        }

        public int StaticEstimationMidEndGame(char[] board,bool isOpening)
        {
            BoardCommon b = new BoardCommon();

            int numWhitePieces = 0;
            int numBlackPieces = 0;

            List<char[]> blackMoves = new List<char[]>();

            blackMoves = b.GenerateBlackMove(board, isOpening); //send midgame parameter so that generatemovemidgame is called and not generate add       

            int blackMovesCount = blackMoves.Count();

            for (int ind = 0; ind < board.Length; ind++)
            {
                numWhitePieces = board[ind] == 'W' ? numWhitePieces + 1 : numWhitePieces;
                numBlackPieces = board[ind] == 'B' ? numBlackPieces + 1 : numBlackPieces;
            }

            if (numBlackPieces <= 2)
            {
                return 10000;
            }
            else if (numWhitePieces <= 2)
            {
                return -10000;
            }
            else if (blackMovesCount == 0)
            {
                return 10000;
            }
            else
            {
                return ((1000 * (numWhitePieces - numBlackPieces)) - blackMovesCount);
            }
        }

    }
}
