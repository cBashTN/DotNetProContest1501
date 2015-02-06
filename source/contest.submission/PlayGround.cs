using System.Collections.Generic;
using System.Linq;
using contest.submission.contract;

namespace contest.submission
{
    internal class PlayGround
    {
        private const int PlayGroundDimMaxX = 1024;
        private const int PlayGroundDimMaxY = 1024;
        private const int PlayGroundDimArrayMaxX = 1023;
        private const int PlayGroundDimArrayMaxY = 1023;

        private readonly int[,] _lastStepX = new int[PlayGroundDimMaxX, PlayGroundDimMaxY];
        private readonly int[,] _lastStepY = new int[PlayGroundDimMaxX, PlayGroundDimMaxY];
        private readonly int[,] _stepData  = new int[PlayGroundDimMaxX, PlayGroundDimMaxY];
        
        public BoolArray Ground;

        public PlayGround(BoolArray ground)
        {
            Ground = ground;
        }

        public int[,] LastStepX
        {
            get { return _lastStepX; }
        }

        public int[,] LastStepY
        {
            get { return _lastStepY; }
        }

        public bool IsNewStep(Point p)
        {
            return _stepData[p.x, p.y] == 0;
        }

        public int GetStepNumberFromPoint(Point p)
        {
            return _stepData[p.x, p.y];
        }

        public IEnumerable<Point> FilterOutInvalidSteps(IEnumerable<Point> possibleSteps)
        {
            var filteredSteps = new List<Point>(8);
            filteredSteps.AddRange(possibleSteps.
                Where(p => !IsOutsideOfTheGround(p)). //this check must be before..
                Where(p => !IsPointAWall(p))); //..checking the actual array
            return filteredSteps;
        }

        private static bool IsOutsideOfTheGround(Point p)
        {
            return ((p.x < 0) || (p.x > PlayGroundDimArrayMaxX) ||
                    (p.y < 0) || (p.y > PlayGroundDimArrayMaxY));
        }

        private bool IsPointAWall(Point p)
        {
            // return _ground.IsTrue(p.x, p.y);  // Gives an error others also found (http://www.dotnetpro.de/newsgroups/newsgroupthread.aspx?id=8779)
            return Ground.Data[p.x, p.y];
        }

        public void SetStepNumberToPoint(int stepCount, Point p)
        {
            _stepData[p.x, p.y] = stepCount;
        }

        public void SetOriginStepNumberToPoint(Point lastPoint, Point newPoint)
        {
            _lastStepX[newPoint.x, newPoint.y] = lastPoint.x;
            _lastStepY[newPoint.x, newPoint.y] = lastPoint.y;
        }
    }
}