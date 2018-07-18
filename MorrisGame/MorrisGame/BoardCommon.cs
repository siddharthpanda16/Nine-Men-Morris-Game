using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisGame
{
    public class BoardCommon
    {
        public double _MiniMaxEstimate = 0;
        public int _Positions_Evaluated = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public int[] Neighbours(int location)
        {
            List<int[]> nbour = new List<int[]>();
            nbour.Add(new int[] { 0,1,2,6 });
            nbour.Add(new int[] { 1,0,11});
            nbour.Add(new int[] { 2,0,3,4,7 });
            nbour.Add(new int[] { 3,2,10});
            nbour.Add(new int[] { 4,5,2,8});
            nbour.Add(new int[] { 5,4,9});
            nbour.Add(new int[] { 6,0,18,7});
            nbour.Add(new int[] { 7,6,15,2,8});
            nbour.Add(new int[] { 8,7,12,4});
            nbour.Add(new int[] { 9,5,14,10});
            nbour.Add(new int[] { 10,9,17,3,11});
            nbour.Add(new int[] { 11,10,20,1});
            nbour.Add(new int[] { 12,8,13});
            nbour.Add(new int[] { 13,12,14,16});
            nbour.Add(new int[] { 14,13,10});
            nbour.Add(new int[] { 15,7,16});
            nbour.Add(new int[] { 16,19,13,15,17});
            nbour.Add(new int[] { 17,16,10});
            nbour.Add(new int[] { 18,6,19});
            nbour.Add(new int[] { 19,18,16,20});
            nbour.Add(new int[] { 20,19,11});         

            foreach(int[] arry in nbour)
            {
                if (arry[0] == location)
                    return arry;
            }
            return new int[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool CloseMill(int location, char[] board)
        {
            char pieceAtLoc = board[location];
            if (pieceAtLoc == 'x')
                return false;
            switch(location)
            {
                case 0:
                    if ((pieceAtLoc == board[6] && pieceAtLoc == board[18]) || (pieceAtLoc == board[2] && pieceAtLoc == board[4]))
                        return true;
                    break;
                case 1:
                    if (pieceAtLoc == board[11] && pieceAtLoc == board[20])
                        return true;
                    break;
                case 2:
                    if (pieceAtLoc == board[0] && pieceAtLoc == board[4])
                        return true;
                    break;
                case 3:
                    if (pieceAtLoc == board[10] && pieceAtLoc == board[17])
                        return true;
                    break;
                case 4:
                    if ((pieceAtLoc == board[2] && pieceAtLoc == board[0]) || (pieceAtLoc == board[8] && pieceAtLoc == board[12]))
                        return true;
                    break;
                case 5:
                    if (pieceAtLoc == board[9] && pieceAtLoc == board[14])
                        return true;
                    break;
                case 6:
                    if ((pieceAtLoc == board[0] && pieceAtLoc == board[18]) || (pieceAtLoc == board[7] && pieceAtLoc == board[8]))
                        return true;
                    break;
                case 7:
                    if ((pieceAtLoc == board[6] && pieceAtLoc == board[8]) || (pieceAtLoc == board[2] && pieceAtLoc == board[15]))
                        return true;
                    break;
                case 8:
                    if ((pieceAtLoc == board[6] && pieceAtLoc == board[7]) || (pieceAtLoc == board[4] && pieceAtLoc == board[12]))
                        return true;
                    break;
                case 9:
                    if ((pieceAtLoc == board[10] && pieceAtLoc == board[11]) || (pieceAtLoc == board[5] && pieceAtLoc == board[14]))
                        return true;
                    break;
                case 10:
                    if ((pieceAtLoc == board[9] && pieceAtLoc == board[11]) || (pieceAtLoc == board[3] && pieceAtLoc == board[17]))
                        return true;
                    break;
                case 11:
                    if ((pieceAtLoc == board[9] && pieceAtLoc == board[10]) || (pieceAtLoc == board[1] && pieceAtLoc == board[20]))
                        return true;
                    break;
                case 12:
                    if ((pieceAtLoc == board[13] && pieceAtLoc == board[14]) || (pieceAtLoc == board[8] && pieceAtLoc == board[4]))
                        return true;
                    break;
                case 13:
                    if ((pieceAtLoc == board[12] && pieceAtLoc == board[14]) || (pieceAtLoc == board[16] && pieceAtLoc == board[19]))
                        return true;
                    break;
                case 14:
                    if ((pieceAtLoc == board[12] && pieceAtLoc == board[13]) || (pieceAtLoc == board[5] && pieceAtLoc == board[9]))
                        return true;
                    break;
                case 15:
                    if ((pieceAtLoc == board[7] && pieceAtLoc == board[2]) || (pieceAtLoc == board[16] && pieceAtLoc == board[17]))
                        return true;
                    break;
                case 16:
                    if ((pieceAtLoc == board[15] && pieceAtLoc == board[17]) || (pieceAtLoc == board[3] && pieceAtLoc == board[19]))
                        return true;
                    break;
                case 17:
                    if ((pieceAtLoc == board[16] && pieceAtLoc == board[15]) || (pieceAtLoc == board[10] && pieceAtLoc == board[3]))
                        return true;
                    break;
                case 18:
                    if ((pieceAtLoc == board[6] && pieceAtLoc == board[0]) || (pieceAtLoc == board[19] && pieceAtLoc == board[20]))
                        return true;
                    break;
                case 19:
                    if ((pieceAtLoc == board[18] && pieceAtLoc == board[20]) || (pieceAtLoc == board[16] && pieceAtLoc == board[13]))
                        return true;
                    break;
                case 20:
                    if ((pieceAtLoc == board[18] && pieceAtLoc == board[19]) || (pieceAtLoc == board[11] && pieceAtLoc == board[1]))
                        return true;
                    break;
            }
            return false;
        }

        public List<char[]> GenerateRemove(char[] board, List<char[]> l)
        {
            List<char[]> piecesListRemove = l.Select(item => (char[])item.Clone()).ToList();

            for (int ind = 0; ind < board.Length; ind++)
            {
                if (board[ind] == 'B')
                {
                    if (!(CloseMill(ind, board)))
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

        public char[] MaxMin(char[] val, int depth,bool isOpening)
        {
            MiniMaxOpening m = new MiniMaxOpening();
            MiniMaxGame mid = new MiniMaxGame();
            if (depth > 0)
            {
                depth--;
                List<char[]> child = new List<char[]>();
                char[] minValue;
                char[] maxValue = new char[43];
                child = isOpening ? m.GenerateAdd(val) : mid.GenerateMovesMidgameEndgame(val);  //so that midgame move and generate add are called by passing parameter from calling function
                double v = Double.NegativeInfinity;

                for (int ind = 0; ind < child.Count; ind++)
                {
                    minValue = MinMax(child[ind], depth, isOpening);

                    double staticEstimate = isOpening ? m.StaticEstimationOpening(minValue) : mid.StaticEstimationMidEndGame(minValue, isOpening);
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

        public char[] MinMax(char[] val, int depth, bool isOpening)
        {
            MiniMaxOpening m = new MiniMaxOpening();
            MiniMaxGame mid = new MiniMaxGame();
            if (depth > 0)
            {
                depth--;
                List<char[]> blackChild = new List<char[]>();
                char[] minValue = new char[43];
                char[] maxValue;
                blackChild = GenerateBlackMove(val, isOpening);

                double v = Double.PositiveInfinity;

                for (int ind = 0; ind < blackChild.Count; ind++)
                {
                    maxValue = MaxMin(blackChild[ind], depth, isOpening);

                    double staticEstimate = isOpening ? m.StaticEstimationOpening(minValue) : mid.StaticEstimationMidEndGame(minValue, isOpening);
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


        public List<char[]> GenerateBlackMove(char[] val, bool isOpening)
        {
            MiniMaxOpening m = new MiniMaxOpening();
            MiniMaxGame mid = new MiniMaxGame();
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

            blackMove = isOpening ?  m.GenerateAdd(copyOfBoard) : mid.GenerateMovesMidgameEndgame(copyOfBoard) ;
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
