using contest.submission.contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace contest.submission
{
    public class PathFinder : IPathFinder
    {
        private readonly PlayGround _playGround;

        public PathFinder(BoolArray ground, Point startpoint, Point endpoint)
        {
            _playGround = new PlayGround(ground, startpoint, endpoint);
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
            Console.WriteLine("Robot uses his tricorder to find a path...");
            List<Step> pathSteps = new List<Step>();
            var stepNumber = 0;

            _playGround.MapNewStep(pathSteps, _playGround.Endpoint, stepNumber);

            do
            {
                stepNumber++;
                var lastStepNumber = stepNumber - 1;

                for (var i = 0; i < pathSteps.Count; i++)
                {
                    if (pathSteps[i].StepCount != lastStepNumber) continue;

                    //the eight adjacent cells
                    List<Point> everyDirectionsSteps = GetStepsForEveryDirections(pathSteps[i].Point);

                    //Check.. If the cell is a wall, remove it from the list
                    List<Point> possibleSteps = _playGround.FilterOutInvalidSteps(everyDirectionsSteps);

                    //Check.. If there is an element in the main list with the same coordinate
                    foreach (var step in possibleSteps.Where(step => !_playGround.IsStepUsedBefore(step)))
                    {
                        _playGround.MapNewStep(pathSteps, step, stepNumber);
                    }
                }

                if (stepNumber % 100 == 0) Console.Write("\r{0}-th step", stepNumber); 
            } while (!_playGround.IsStepUsedBefore(_playGround.Startpoint)); //Go to the next item in the list and repeat until start is reached

            return ChooseAPathLine(pathSteps);
        }

        private Point[] ChooseAPathLine(List<Step> pathSteps)
        {
            int stepCountMax = pathSteps.Max(step => step.StepCount);
            int stepCount = stepCountMax;

            Point[] choosenPath = new Point[stepCount + 1];

            choosenPath[stepCount] = _playGround.Startpoint;

            stepCount--;

            Console.WriteLine(Environment.NewLine+"Robot's tricorder found several paths with " + stepCountMax + " steps.");
            Console.WriteLine("Robot now chooses one of them... ");

            for (var i = stepCount; i > 0; i--)
            {
                List<Step> stepsWithNextStepCount = pathSteps.FindAll(step => step.StepCount == i);
                List<Point> stepsForEveryDirections = GetStepsForEveryDirections(choosenPath[i + 1]);

                choosenPath[i] = IntersectAndChooseRandom(stepsForEveryDirections, stepsWithNextStepCount);
                if (i % 100 == 0) Console.Write(".");
            }
            Console.Write(Environment.NewLine);
            choosenPath[0] = _playGround.Endpoint;
            Array.Reverse(choosenPath);
            return choosenPath;
        }

        // chooses randomely one step from from the adjacent steps that is also one in a path to the endpoint
        private static Point IntersectAndChooseRandom(List<Point> stepsForEveryDirections, List<Step> stepsWithNextStepCount)
        {
            List<Point> intersectedPoints = new List<Point>(8);
            foreach (Point point in stepsForEveryDirections)
            {
                for (var j = stepsWithNextStepCount.Count - 1; j >= 0; j--)
                {
                    if (point.IsEqual(stepsWithNextStepCount[j].Point))
                    {
                        intersectedPoints.Add(point);
                    }
                }
            }
            Random rnd = new Random();
            return intersectedPoints[rnd.Next(intersectedPoints.Count)];
        }

        static List<Point> GetStepsForEveryDirections(Point p)
        {
            List<Point> possiblePoints = new List<Point>(8)
            {
                new Point {x = p.x - 1, y = p.y - 1}, //North-West
                new Point {x = p.x    , y = p.y - 1}, //North
                new Point {x = p.x + 1, y = p.y - 1}, //North-East
                new Point {x = p.x - 1, y = p.y    }, //West
                new Point {x = p.x + 1, y = p.y    }, //East
                new Point {x = p.x - 1, y = p.y + 1}, //South-West
                new Point {x = p.x    , y = p.y + 1}, //South
                new Point {x = p.x + 1, y = p.y + 1}  //South-East
            };
            return possiblePoints;
        }
    }
}
