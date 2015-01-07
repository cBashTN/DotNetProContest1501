using contest.submission.contract;
using System.Collections.Generic;
using System.Linq;

namespace contest.submission
{
    class PlayGround
    {
        public BoolArray Ground;
        public Point Startpoint;
        public Point Endpoint;

        const int PlayGroundDimMaxX = 1024;
        const int PlayGroundDimMaxY = 1024;

        private readonly int[,] _stepData = new int[PlayGroundDimMaxX, PlayGroundDimMaxY];

        public PlayGround(BoolArray ground, Point startpoint, Point endpoint)
        {
            this.Ground     = ground;
            this.Startpoint = startpoint;
            this.Endpoint   = endpoint;

            InitializeWithZero();
        }

        public bool IsStepUsedBefore(Point newStep)
        {
            //this is too slow:
            //return pathSteps.Any(storedSteps => storedSteps.Point.IsEqual(newStep));

            //broke down to O(1) instead of O(x^2) by using a "hash-array"
            return IsPointUsed(newStep);
        }

        public void MapNewStep(List<Step> pathSteps, Point step, int stepNumber)
        {
            pathSteps.Add(new Step { Point = step, StepCount = stepNumber });
            SetStepNumberToPoint(stepNumber, step);
        }

        public List<Point> FilterOutInvalidSteps(List<Point> possibleSteps)
        {
            var filteredSteps = new List<Point>(8);
            filteredSteps.AddRange(possibleSteps.
                Where(point => !IsOutsideOfTheGround(point)).   //this check must be before..
                Where(point => !IsPointAWall(point)));          //..checking the actual array
            return filteredSteps;
        }

        private static bool IsOutsideOfTheGround(Point p)
        {
            return ((p.x < 0) || (p.x > PlayGroundDimMaxX-1) ||
                    (p.y < 0) || (p.y > PlayGroundDimMaxY-1));
        }

        private bool IsPointAWall(Point p)
        {
            // return _ground.IsTrue(p.x, p.y);  // Gives an error others also found (http://www.dotnetpro.de/newsgroups/newsgroupthread.aspx?id=8779)
            return Ground.Data[p.x, p.y];
        }

        private bool IsPointUsed(Point p)
        {
            return _stepData[p.x, p.y] > 0;
        }

        private void SetStepNumberToPoint(int stepCount, Point p)
        {
            _stepData[p.x, p.y] = stepCount;
        }

        private void InitializeWithZero()
        {
            for (var i = 0; i < PlayGroundDimMaxX; i++)
            {
                for (var j = 0; j < PlayGroundDimMaxY; j++)
                {
                    _stepData[i, j] = 0;
                }
            }
        }
    }
}
