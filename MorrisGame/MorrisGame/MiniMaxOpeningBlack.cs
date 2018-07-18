using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    public class MiniMaxOpeningBlack
    {
        private double _MiniMaxEstimate = 0;
        private int _Positions_Evaluated = 0;
        private List<char[]> GenerateAdd(char[] board)
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

        private int StaticEstimationOpening(char[] board)
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

            blackMove = GenerateAdd(copyOfBoard);
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

        private char[] MaxMin(char[] val, int depth)
        {            
            if (depth > 0)
            {
                depth--;
                List<char[]> child = new List<char[]>();
                char[] minValue;
                char[] maxValue = new char[43];
                child =GenerateAdd(val);  //so that midgame move and generate add are called by passing parameter from calling function

                double v = Double.NegativeInfinity;

                for (int ind = 0; ind < child.Count; ind++)
                {
                    minValue = MinMax(child[ind], depth);

                    double staticEstimate = StaticEstimationOpening(minValue);
                    if (v < staticEstimate)
                    {
                        v = staticEstimate;
                        _MiniMaxEstimate = v;
                        maxValue = child[ind];
                    }
                }
                return maxValue;
            }
            else if (depth == 0)
            {
                _Positions_Evaluated++;
            }
            return val;
        }

        private char[] MinMax(char[] val, int depth)
        {
            if (depth > 0)
            {
                depth--;
                List<char[]> blackChild = new List<char[]>();
                char[] minValue = new char[43];
                char[] maxValue;
                blackChild = GenerateBlackMove(val);

                double v = Double.PositiveInfinity;

                for (int ind = 0; ind < blackChild.Count; ind++)
                {
                    maxValue = MaxMin(blackChild[ind], depth);

                    double staticEstimate = StaticEstimationOpening(minValue);
                    if (v > staticEstimate)
                    {
                        v = staticEstimate;
                        _MiniMaxEstimate = v;
                        minValue = blackChild[ind];
                    }
                }
                return minValue;
            }
            else if (depth == 0)
            {
                _Positions_Evaluated++;
            }
            return val;
        }

        public char[] WhiteBlackPieceSwap(char[] position)
        {

            char[] copyOfBoard = (char[])position.Clone();

            for (int i = 0; i < copyOfBoard.Length; i++)
            {
                if (copyOfBoard[i] == 'W')
                {
                    copyOfBoard[i] = 'B';
                    continue;
                }
                if (copyOfBoard[i] == 'B')
                {
                    copyOfBoard[i] = 'W';
                }
            }
            return copyOfBoard;
        }

        public void MiniMaxOpeningForBlack()
        {
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

                    MiniMaxOpeningBlack miniMaxOpeningBlack = new MiniMaxOpeningBlack();
                    char[] swapped = miniMaxOpeningBlack.WhiteBlackPieceSwap(inputPiecePosition);
                    char[] afterMiniMax = miniMaxOpeningBlack.MaxMin(swapped, depth);
                    char[] reSwapped = miniMaxOpeningBlack.WhiteBlackPieceSwap(afterMiniMax);
                    Console.WriteLine("New Board Position " + new String(reSwapped));


                    Console.WriteLine("Position evaluated " + miniMaxOpeningBlack._Positions_Evaluated);
                    Console.WriteLine("MiniMax estimate " + miniMaxOpeningBlack._MiniMaxEstimate);


                    using (StreamWriter file =
                                      new StreamWriter(outputFile))
                    {
                        file.WriteLine("New Board Position " + new String(reSwapped));
                        file.WriteLine("Position evaluated " + miniMaxOpeningBlack._Positions_Evaluated);
                        file.WriteLine("Minimax Estimate " + miniMaxOpeningBlack._MiniMaxEstimate);
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
