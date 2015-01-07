using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using contest.submission.contract;

namespace contest.submission
{
    class PlayGround
    {
        public BoolArray Ground;
        public Point Startpoint;
        public Point Endpoint;

        private BoolArray b;
        private readonly int[,] _stepData = new int[PlayGroundDimMaxX, PlayGroundDimMaxY];

        const int PlayGroundDimMaxX = 1024;
        const int PlayGroundDimMaxY = 1024;

        public PlayGround(BoolArray ground, Point startpoint, Point endpoint)
        {
            this.Ground = ground;
            this.Startpoint = startpoint;
            this.Endpoint = endpoint;

            InitializeWithZero();
        }

        public bool IsPointUsed(Point p)
        {
            return _stepData[p.x, p.y] > 0;
        }

        public void SetPoint(Point p, int stepCount)
        {
            _stepData[p.x, p.y] = stepCount;
        }

        public int GetStepCount(Point p)
        {
            return _stepData[p.x, p.y];
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
