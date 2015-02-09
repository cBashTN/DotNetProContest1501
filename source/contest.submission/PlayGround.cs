using contest.submission.contract;

namespace contest.submission
{
    internal class PlayGround
    {
        private const int PlayGroundDimMaxX = 1024;
        private const int PlayGroundDimMaxY = 1024;
        private const int PlayGroundDimArrayMaxX = PlayGroundDimMaxX - 1;
        private const int PlayGroundDimArrayMaxY = PlayGroundDimMaxY - 1;

        private readonly int[,] _lastStepX = new int[PlayGroundDimMaxX, PlayGroundDimMaxY]; //stores the x-coordiante of the last step
        private readonly int[,] _lastStepY = new int[PlayGroundDimMaxX, PlayGroundDimMaxY]; //stores the y-coordiante of the last step 

        private readonly int[,] _stepData = new int[PlayGroundDimMaxX, PlayGroundDimMaxY]; //stores the stepnumber per stepped point


        private readonly BoolArray _ground;

        internal PlayGround(BoolArray ground)
        {
            _ground = ground;
        }

        internal int[,] LastStepX
        {
            get { return _lastStepX; }
        }

        internal int[,] LastStepY
        {
            get { return _lastStepY; }
        }

        internal bool IsNewStep(int x, int y)
        {
            return _stepData[x, y] == 0;
        }

        internal void MarkNewStep(Point lastStep, Point newStep, int stepNumber)
        {
            SetStepNumberToPoint(stepNumber, newStep);
            SetOriginStepNumberToPoint(lastStep, newStep);
        }

        internal void SetStepNumberToPoint(int stepCount, Point p)
        {
            _stepData[p.x, p.y] = stepCount;
        }

        internal void SetOriginStepNumberToPoint(Point lastStep, Point newStep)
        {
            if (lastStep == null) return;
            _lastStepX[newStep.x, newStep.y] = lastStep.x;
            _lastStepY[newStep.x, newStep.y] = lastStep.y;
        }

        internal int GetStepNumberFromPoint(Point p)
        {
            return _stepData[p.x, p.y];
        }

        internal bool IsOutsideOfTheGround(int x, int y)
        {
            return ((x < 0) || (x > PlayGroundDimArrayMaxX) ||
                    (y < 0) || (y > PlayGroundDimArrayMaxY));
        }

        internal bool IsPointAWall(int x, int y)
        {
            return _ground.Data[x, y];
        }

        internal bool IsValid(int i, int x, int y)
        {
            return    !IsOutsideOfTheGround(x, y)
                   && IsNewStep(x, y) //If there is an element in the main list with the same coordinate and an equal or higher counter
                   && !IsPointAWall(x, y) //If the cell is a wall, remove it from the list
                ;
        }
    }
}