using contest.submission.contract;

namespace contest.submission
{
    internal class PlayGround
    {
        private const int PlayGroundDimMaxX = 1024;
        private const int PlayGroundDimMaxY = 1024;
        private const int PlayGroundDimArrayMaxX = PlayGroundDimMaxX-1;
        private const int PlayGroundDimArrayMaxY = PlayGroundDimMaxY-1;

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

        public bool IsNewStep(int x, int y)
        {
            return _stepData[x, y] == 0;
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

        public int GetStepNumberFromPoint(Point p)
        {
            return _stepData[p.x, p.y];
        }

        public bool IsOutsideOfTheGround(int x, int y)
        {
            return ((x < 0) || (x > PlayGroundDimArrayMaxX) ||
                    (y < 0) || (y > PlayGroundDimArrayMaxY));
        }

        public bool IsPointAWall(int x, int y)
        {
            return Ground.Data[x, y];
        }
    }
}