using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using contest.submission.contract;

namespace contest.submission
{
    public class PathFinder : IPathFinder
    {
        private PlayGround _playGround;


        public PathFinder(BoolArray ground, Point startpoint, Point endpoint)
        {
           _playGround = new PlayGround(ground, startpoint, endpoint);
 
        }

        /** Algorithm:
         * 
         * Its mor or less the same as described here: http://en.wikipedia.org/wiki/Pathfinding#Sample_algorithm
         *
         * - Create a list of the eight adjacent cells, with a counter variable of the current element's counter variable + 1
         * 
         * - Check all cells in each list for the following two conditions:
         *      * If the cell is a wall, remove it from the list
         *      * If there is an element in the main list with the same coordinate and an equal or higher counter
         * 
         * - Add all remaining cells in the list to the end of the main list
         * 
         * - Go to the next item in the list and repeat until start p is reached 
         * 
        **/
        public Point[] FindAPath()
    {
        Console.WriteLine("Robot uses his tricorder to find a path...");
        List<Step> pathSteps = new List<Step>();
        var stepNumber = 0;

        MapNewStep(pathSteps, _playGround.Endpoint, stepNumber);

        do
        {
            stepNumber++;
            var lastStepNumber = stepNumber - 1;
            foreach (var lastStep in pathSteps.ToArray().Where(step => step.StepCount == lastStepNumber))
            {
                List<Point> everyDirectionsSteps = GetStepsForEveryDirections(lastStep.Point); //the eight adjacent cells
                List<Point> possibleSteps = FilterOutInvalidSteps(everyDirectionsSteps); //Check.. If the cell is a wall, remove it from the list

                //Check.. If there is an element in the main list with the same coordinate
                foreach (var step in possibleSteps.Where(step => !IsStepUsedBefore(step)))
                {
                    MapNewStep(pathSteps, step, stepNumber);
                }
            }
            if (stepNumber % 100 == 0) Console.WriteLine(stepNumber + "-th step");
        } while (!IsStepUsedBefore(_playGround.Startpoint)); //Go to the next item in the list and repeat until start p is reached

        return ChooseAPathLine(pathSteps);
    }
        private void MapNewStep(List<Step> pathSteps, Point step, int stepNumber)
        {
            pathSteps.Add(new Step { Point = step, StepCount = stepNumber });
            _playGround.SetPoint(step, stepNumber);
        }

        private Point[] ChooseAPathLine(List<Step> pathSteps)
        {

            int stepCountMax = pathSteps.Max(step => step.StepCount);
            int stepCount = stepCountMax;

            Point[] choosenPath = new Point[stepCount + 1];

            choosenPath[stepCount] = _playGround.Startpoint;

            stepCount--;

            Console.WriteLine("Robot's tricorder found several paths with " + stepCountMax + " steps.");
            Console.WriteLine("Robot now chooses one of them... ");

            for (var i = stepCount; i > 0; i--)
            {
                List<Step> stepsWithNextStepCount = pathSteps.FindAll(step => step.StepCount == i);
                List<Point> stepsForEveryDirections = GetStepsForEveryDirections(choosenPath[i + 1]);

                choosenPath[i] = IntersectAndChooseRandom(stepsForEveryDirections, stepsWithNextStepCount);
            }
            choosenPath[0] = _playGround.Endpoint;
            Array.Reverse(choosenPath);
            return choosenPath;
        }

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

        private bool IsStepUsedBefore(Point newStep)
        {
            //this is too slow
            //return pathSteps.Any(storedSteps => storedSteps.Point.IsEqual(newStep));

            //broke down to O(1) instead of O(x^2)
            return _playGround.IsPointUsed(newStep);
        }

        private List<Point> FilterOutInvalidSteps(List<Point> possibleSteps)
        {
            var filteredSteps = new List<Point>(8);
            filteredSteps.AddRange(possibleSteps.
                Where(point => !IsOutsideOfTheGround(point)).
                Where(point => !IsPointAWall(point))
                );
            return filteredSteps;
        }

        private static bool IsOutsideOfTheGround(Point p)
        {
            return ((p.x < 0) || (p.x > 1023) || 
                    (p.y < 0) || (p.y > 1023));
        }

        bool IsPointAWall(Point p)
        {
            // return _ground.IsTrue(p.x, p.y);  // Gives an error others also found (http://www.dotnetpro.de/newsgroups/newsgroupthread.aspx?id=8779)
            return _playGround.Ground.Data[p.x, p.y];
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
