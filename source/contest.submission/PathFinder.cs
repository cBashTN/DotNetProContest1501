﻿using contest.submission.contract;
using System.Collections.Generic;

namespace contest.submission
{
    public class Pathfinder
    {
        private readonly PlayGround _playGround;
        private readonly Point _startPoint;
        private readonly Point _endPoint;

        private readonly int[] _nextPossibleX = new int[8];
        private readonly int[] _nextPossibleY = new int[8];

        
        // Every step number has its own list of points. These lists are stored in an array (_steps) that is inxed by the step number
        // 530000 exceeds the maximum possible path in a 1024x1024 playground
        private readonly List<Point>[] _steps = new List<Point>[530000];

        public Pathfinder(BoolArray ground, Point startpoint, Point endpoint)
        {
            _playGround = new PlayGround(ground);
            _startPoint = startpoint;
            _endPoint = endpoint;
        }

        /** Algorithm:
         * 
         * It's more or less the same as described here: http://en.wikipedia.org/wiki/Pathfinding#Sample_algorithm
         *
         * - Create a list of the eight adjacent cells, with a counter variable of the current element's counter variable + 1
         * 
         * - Check all cells in each list for the following two conditions:
         *      * If the cell is a wall, remove it from the list
         *      * If there is an element in the main list with the same coordinate and an equal or higher counter
         * 
         * - Add all remaining cells in the list to the end of the main list
         * 
         * - Go to the next item in the list and repeat until start is reached 
         * 
        **/
        public Point[] FindAPath()
        {
            var stepNumber = 0;
            AddAndMarkNewStep(null, _endPoint, stepNumber);

            do
            {
                stepNumber++; // the current element's counter variable + 1
                var lastStepNumber = stepNumber - 1;

                foreach (var lastStep in _steps[lastStepNumber])
                {
                    foreach (var newStep in NextPossibleSteps(lastStep))
                    {
                        AddAndMarkNewStep(lastStep, newStep, stepNumber); //Add all remaining cells in the list to the end of the main list
                    }
                }
            } while (_playGround.IsNewStep(_startPoint.x,_startPoint.y)); //Go to the next item in the list and repeat until start is reached 

            return ChooseAPathLine(stepNumber);
        }

        private IEnumerable<Point> NextPossibleSteps(Point p)
        {
            SetTheEightAdjacentSteps(p); //Create a list of the eight adjacent cells, with a counter variable of the current element's counter variable + 1

            for (int i = 0; i < 8; i++)
            {
                //Check all cells in each list for the following two conditions:
                // - If the cell is a wall, remove it from the list
                // - If there is an element in the main list with the same coordinate and an equal or higher counter
                if (_playGround.IsValid(i, _nextPossibleX[i], _nextPossibleY[i]))
                {
                    yield return new Point { x = _nextPossibleX[i], y = _nextPossibleY[i] };
                }
            }
        }

        // Goes back from the startpoint (0) to the endpoint (stepCountMax) by using the last steps which were stored for each used step
        private Point[] ChooseAPathLine(int stepCountMax)
        {
            var choosenPath = new Point[stepCountMax + 1];
            choosenPath[0] = _startPoint;

            for (var stepCount = 0; stepCount < stepCountMax-1; stepCount++)
            {
                var nextX = choosenPath[stepCount].x;
                var nextY = choosenPath[stepCount].y;

                choosenPath[stepCount+1] = new Point()
                {
                    x = _playGround.LastStepX[nextX, nextY],
                    y = _playGround.LastStepY[nextX, nextY]
                };
            }

            choosenPath[stepCountMax] = _endPoint;
            return choosenPath;
        }

        // Every step field will be stored in a list. Every stepnumber has its own list.
        private void AddAndMarkNewStep(Point lastStep, Point step, int stepNumber)
        {
            AddNewStep(step, stepNumber);
            _playGround.MarkNewStep(lastStep, step, stepNumber);
        }

        private void AddNewStep(Point step, int stepNumber)
        {
            if (IsANewStepNumber(stepNumber))
                    {
                _steps[stepNumber] = new List<Point>();
                }
            _steps[stepNumber].Add(step);
            }

        private bool IsANewStepNumber(int stepNumber)
        {
            return _steps[stepNumber] == null;
        }

        private void SetTheEightAdjacentSteps(Point p)
        {
            _nextPossibleX[0] = p.x - 1; _nextPossibleY[0] = p.y - 1; //North-West
            _nextPossibleX[1] = p.x    ; _nextPossibleY[1] = p.y - 1; //North
            _nextPossibleX[2] = p.x + 1; _nextPossibleY[2] = p.y - 1; //North-East
            _nextPossibleX[3] = p.x - 1; _nextPossibleY[3] = p.y;     //West
            _nextPossibleX[4] = p.x + 1; _nextPossibleY[4] = p.y;     //East
            _nextPossibleX[5] = p.x - 1; _nextPossibleY[5] = p.y + 1; //South-West
            _nextPossibleX[6] = p.x    ; _nextPossibleY[6] = p.y + 1; //South
            _nextPossibleX[7] = p.x + 1; _nextPossibleY[7] = p.y + 1; //South-East
        }
    }
}
