using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    public class MiniMaxGameImproved
    {
        private double _MiniMaxEstimate = 0;
        private int _Positions_Evaluated = 0;
        private List<char[]> GenerateMovesMidgameEndgame(char[] location)
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

        private List<char[]> GenerateHopping(char[] location)
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
                                GenerateRemove(copyOfBoard, hoppingList);
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

        private List<char[]> GenerateMove(char[] location)
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
                                GenerateRemove(copyOfBoard, piecesToMove);
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

        private List<char[]> GenerateRemove(char[] board, List<char[]> l)
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
                        //Console.WriteLine("Remove black piece at postion " + ind);
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

        private List<char[]> GenerateBlackMove(char[] val)
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

            blackMove = GenerateMovesMidgameEndgame(copyOfBoard);
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

        private int StaticEstimationMidEndGame(char[] board)
        {


            int numWhitePieces = 0;
            int numBlackPieces = 0;

            List<char[]> blackMoves = new List<char[]>();

            blackMoves = GenerateBlackMove(board); //send midgame parameter so that generatemovemidgame is called and not generate add       

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
                return blackMovesCount;
            }
        }      

        private char[] MaxMin(char[] val, int depth)
        {
            if (depth > 0)
            {
                depth--;
                List<char[]> child = new List<char[]>();
                char[] minValue;
                char[] maxValue = new char[43];
                child = GenerateMovesMidgameEndgame(val);  //so that midgame move and generate add are called by passing parameter from calling function

                /*
                foreach (char[] c in child)
                {
                    Console.WriteLine("number of possible moves for white are " + new String( c ));
                }
                */

                double v = Double.NegativeInfinity;

                for (int ind = 0; ind < child.Count; ind++)
                {
                    minValue = MinMax(child[ind], depth);

                    double staticEstimate = StaticEstimationMidEndGame(minValue);
                    if (v < staticEstimate)
                    {
                        v = staticEstimate;
                        _MiniMaxEstimate = v;
                        maxValue = child[ind];
                        // Console.WriteLine("ascsc" +_MiniMaxEstimate);
                    }
                }
                return maxValue;
            }
            else if (depth == 0)
            {
                _Positions_Evaluated++;
                //Console.WriteLine("common " + _Positions_Evaluated);
            }
            return val;
        }

        private char[] MinMax(char[] val, int depth)
        {
            // m = new MiniMaxOpening();
            //MiniMaxGame mid = new MiniMaxGame();
            if (depth > 0)
            {
                depth--;
                List<char[]> blackChild = new List<char[]>();
                char[] minValue = new char[43];
                char[] maxValue;
                blackChild = GenerateBlackMove(val);

                /*
                foreach (char[] c in blackChild)
                {
                    Console.WriteLine("number of possible moves for black are " + new string( c));
                }
                */

                double v = Double.PositiveInfinity;

                for (int ind = 0; ind < blackChild.Count; ind++)
                {
                    maxValue = MaxMin(blackChild[ind], depth);

                    double staticEstimate = StaticEstimationMidEndGame(minValue);
                    if (v > staticEstimate)
                    {
                        v = staticEstimate;
                        _MiniMaxEstimate = v;
                        minValue = blackChild[ind];
                        //Console.WriteLine("common _MiniMaxEstimate " + _MiniMaxEstimate);
                    }
                }
                return minValue;
            }
            else if (depth == 0)
            {
                //Console.WriteLine("common _Positions_Evaluated " + _Positions_Evaluated);
                _Positions_Evaluated++;
            }
            return val;
        }

        public void MiniMaxGameForImproved()
        {
            Console.WriteLine("You have entered Improved MiniMax Static Estimation");
            Console.WriteLine("Enter the input file location : ");
            string inputFile = Console.ReadLine();
            Console.WriteLine("Enter the output file location : ");
            string outputFile = Console.ReadLine();
            Console.WriteLine("Enter depth : ");
            int depth = Convert.ToInt32(Console.ReadLine());

            try
            {
                using (StreamReader sr = new StreamReader(inputFile))
                {
                    char[] inputPiecePosition = new char[50];
                    string wholeInput;
                    // read lines from the file                    
                    while ((wholeInput = sr.ReadLine()) != null)
                    {
                        inputPiecePosition = wholeInput.ToCharArray();
                    }

                    Console.WriteLine("Input Board Position " + new String(inputPiecePosition));

                    MiniMaxGameImproved miniMaxGameImproved = new MiniMaxGameImproved();
                    char[] afterMiniMax = miniMaxGameImproved.MaxMin(inputPiecePosition, depth);
                    Console.WriteLine("New Board Position " + new String(afterMiniMax));

                    Console.WriteLine("Position evaluated " + miniMaxGameImproved._Positions_Evaluated);
                    Console.WriteLine("MiniMax estimate " + miniMaxGameImproved._MiniMaxEstimate);

                    using (StreamWriter file =
                                      new StreamWriter(outputFile))
                    {
                        file.WriteLine("New Board Position " + new String(afterMiniMax));
                        file.WriteLine("Position evaluated " + miniMaxGameImproved._Positions_Evaluated);
                        file.WriteLine("Minimax Estimate " + miniMaxGameImproved._MiniMaxEstimate);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in MiniMaxGame Black");
            }
        }
    }
}
