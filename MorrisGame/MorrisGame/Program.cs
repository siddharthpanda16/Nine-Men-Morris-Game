using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Do you prefer Improved - I / Unimproved - U static estimation : (I/U)");
                string isImproved = Console.ReadLine();
                if (isImproved.ToUpper() == "U")
                {
                    Console.WriteLine("Enter MiniMaxGame/MiniMaxGameBlack: (W/B)");
                    string gameType = Console.ReadLine();
                    if (gameType == "W")
                    {
                        Console.WriteLine("Enter the input file location : ");
                        string inputFile = Console.ReadLine();
                        Console.WriteLine("Enter the output file location : ");
                        string outputFile = Console.ReadLine();
                        Console.WriteLine("Enter depth : ");
                        int depth = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Is your game Opening or MidGame : (O/M)  ");
                        string isOpeningInput = Console.ReadLine();

                        Console.WriteLine("MiniMax / AlphaBeta : (M/A)  ");
                        string miniMaxOrAlphaBeta = Console.ReadLine();

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

                                BoardCommon b = new BoardCommon();
                                MiniMaxOpening mOpen = new MiniMaxOpening();
                                ABOpening aBOpening = new ABOpening();

                                Console.WriteLine("Input Board Position " + new String(inputPiecePosition));

                                bool isOpening = isOpeningInput.ToUpper() == "O" ? true : false;
                                bool isMiniMax = miniMaxOrAlphaBeta.ToUpper() == "M" ? true : false;

                                char[] afterMiniMax = isMiniMax ? b.MaxMin(inputPiecePosition, depth, isOpening) : aBOpening.MaxMin(inputPiecePosition, depth, isOpening, Double.NegativeInfinity, Double.PositiveInfinity);
                                Console.WriteLine("New Board Position " + new String(afterMiniMax));

                               
                                if (isMiniMax)
                                {
                                    Console.WriteLine("Position evaluated " + b._Positions_Evaluated);
                                    Console.WriteLine("Minimax Estimate " + b._MiniMaxEstimate);

                                    using (StreamWriter file =
                                       new StreamWriter(outputFile))
                                    {
                                        file.WriteLine("New Board Position " + new String(afterMiniMax));
                                        file.WriteLine("Position evaluated " + b._Positions_Evaluated);
                                        file.WriteLine("Minimax Estimate " + b._MiniMaxEstimate);
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Position evaluated " + aBOpening._ABPositions_Evaluated);
                                    Console.WriteLine("Minimax Estimate " + aBOpening._ABMiniMaxEstimate);

                                    using (StreamWriter file =
                                      new StreamWriter(outputFile))
                                    {
                                        file.WriteLine("New Board Position " + new String(afterMiniMax));
                                        file.WriteLine("Position evaluated " + aBOpening._ABPositions_Evaluated);
                                        file.WriteLine("Minimax Estimate " + aBOpening._ABMiniMaxEstimate);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception Occured in Part 1");
                        }
                        Console.ReadLine();
                    }
                    else
                    {
                        try
                        {
                            Console.WriteLine("Is your game Opening or MidGame : (O/M)  ");
                            string isOpeningInput = Console.ReadLine();
                            bool isOpening = isOpeningInput.ToUpper() == "O" ? true : false;

                            MiniMaxGameBlack miniMaxGameBlack = new MiniMaxGameBlack();
                            MiniMaxOpeningBlack miniMaxOpeningBlack = new MiniMaxOpeningBlack();
                            if (isOpening)
                                miniMaxOpeningBlack.MiniMaxOpeningForBlack();
                            else
                                miniMaxGameBlack.MiniMaxGameForBlack();
                        }
                        catch(Exception ee)
                        {
                            Console.WriteLine("Exception Occured in MiniMax Black");
                        }
                    }
                }
                else
                {
                    try
                    {
                        Console.WriteLine("Is your game Opening or MidGame : (O/M)");
                        string isOpeningInput = Console.ReadLine();

                        if (isOpeningInput.ToUpper() == "O")
                        {
                            MiniMaxOpeningImproved miniMaxOpeningImproved = new MiniMaxOpeningImproved();
                            miniMaxOpeningImproved.MiniMaxOpeningForImproved();
                        }
                        else
                        {
                            MiniMaxGameImproved miniMaxGameImproved = new MiniMaxGameImproved();
                            miniMaxGameImproved.MiniMaxGameForImproved();
                        }
                    }
                    catch(Exception ee)
                    {
                        Console.WriteLine("Exception Occured in MiniMax Improveds");
                    }
                }
            }
        }
    }
}


/*
           *foreach (char locs in boardValue)
               Console.Write(locs + "  ");
           Console.WriteLine();

           Board b = new Board();
           while (true)
           {
               Console.Write("Enter location: ");
               int loc = Convert.ToInt32(Console.ReadLine());
               int[] arr = b.Neighbours(loc);
               foreach (int locs in arr)
                   Console.Write(locs + "  ");
               Console.ReadLine();

           }
*/