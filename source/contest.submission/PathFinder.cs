using contest.submission.contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace contest.submission
{
    public class PathFinder
    {
        private readonly PlayGround _playGround;
        private readonly Point _startPoint;
        private readonly Point _endPoint;

        public PathFinder(BoolArray ground, Point startpoint, Point endpoint)
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
            var steps = new List<Point>[530000];  //530000 exceeds the maximum possible path in a 1024x1024 playground

            var stepNumber = 0;

            _playGround.MarkNewStep(steps, _endPoint, stepNumber);

            do
            {
                stepNumber++; // the current element's counter variable + 1
                var lastStepNumber = stepNumber - 1;

                foreach (var stp in steps[lastStepNumber])
                {
                    //the eight adjacent cells
                    IEnumerable<Point> everyDirectionsSteps = GetStepsForEveryDirections(stp);

                    //Check.. If the cell is a wall, remove it from the list
                    IEnumerable<Point> possibleSteps = _playGround.FilterOutInvalidSteps(everyDirectionsSteps);

                    //Check.. If there is an element in the main list with the same coordinate
                    //        If it's a new coordinate then mark this element
                    foreach (var step in possibleSteps.Where(step => !_playGround.IsStepMarkedBefore(step)))
                    {
                        _playGround.MarkNewStep(steps, step, stepNumber);
                    }
                }
            //Go to the next item in the list and repeat until start is reached
            } while (!_playGround.IsStepMarkedBefore(_startPoint));

            return ChooseAPathLine(steps, stepNumber);
        }

        private Point[] ChooseAPathLine(List<Point>[] pathSteps, int stepnumber)
        {
            int stepCountMax = stepnumber;
            int stepCount = stepCountMax;

            Point[] choosenPath = new Point[stepCount + 1];

            choosenPath[stepCount] = _startPoint;

            stepCount--;

            for (var i = stepCount; i > 0; i--)
            {
                List<Point> stepsWithNextStepCount = pathSteps[i];
                IEnumerable<Point> stepsForEveryDirections = GetStepsForEveryDirections(choosenPath[i + 1]);

                choosenPath[i] = IntersectAndChooseRandom(stepsForEveryDirections, stepsWithNextStepCount);
            }
            choosenPath[0] = _endPoint;
            Array.Reverse(choosenPath);
            return choosenPath;
        }

        // chooses randomly one step from from the adjacent steps that is also one in a path to the endpoint
        private static Point IntersectAndChooseRandom(IEnumerable<Point> stepsForEveryDirections, List<Point> stepsWithNextStepCount)
        {
            List<Point> intersectedPoints = new List<Point>(8);
            foreach (Point point in stepsForEveryDirections)
            {
                for (var j = stepsWithNextStepCount.Count - 1; j >= 0; j--)
                {
                    if (point.IsEqual(stepsWithNextStepCount[j]))
                    {
                        intersectedPoints.Add(point);
                    }
                }
            }
            var rnd = new Random();
            return intersectedPoints[rnd.Next(intersectedPoints.Count)];
        }

        static IEnumerable<Point> GetStepsForEveryDirections(Point p)
        {
            yield return new Point {x = p.x - 1, y = p.y - 1}; //North-West
            yield return new Point {x = p.x    , y = p.y - 1}; //North
            yield return new Point {x = p.x + 1, y = p.y - 1}; //North-East
            yield return new Point {x = p.x - 1, y = p.y    }; //West
            yield return new Point {x = p.x + 1, y = p.y    }; //East
            yield return new Point {x = p.x - 1, y = p.y + 1}; //South-West
            yield return new Point {x = p.x    , y = p.y + 1}; //South
            yield return new Point {x = p.x + 1, y = p.y + 1}; //South-East
        }
    }
}
