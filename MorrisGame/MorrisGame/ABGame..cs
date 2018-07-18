using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    public class ABGame
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
                            copyOfBoard = (char[])location.Clone();
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

        public int StaticEstimationMidEndGame(char[] board, bool isOpening)
        {
            BoardCommon b = new BoardCommon();

            int numWhitePieces = 0;
            int numBlackPieces = 0;

            List<char[]> blackMoves = new List<char[]>();

            blackMoves = GenerateBlackMove(board, isOpening); //send midgame parameter so that generatemovemidgame is called and not generate add       

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

        public List<char[]> GenerateBlackMove(char[] val, bool isOpening)
        {
            
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

            blackMove = isOpening ? GenerateAdd(copyOfBoard) : GenerateMovesMidgameEndgame(copyOfBoard);
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
            BoardCommon b = new BoardCommon();
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
    }
}
