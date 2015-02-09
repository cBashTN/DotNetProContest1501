using contest.submission.contract;
using System;

namespace contest.submission
{
    [Serializable]
    public class Solution : IDnp1501Solution
    {
        private int _stepNumber;
        [NonSerializedAttribute]
        private Point[] _path;

        public void Start(BoolArray ground, Point startpoint, Point endpoint)
        {
            Pathfinder pathFinder = new Pathfinder(ground, startpoint, endpoint);
            _path = pathFinder.FindAPath();
            
            _stepNumber = 1; // 0 would be the startpoint

            NextStep();
        }

        public void NextStep()
        {
            MakeMove(_path[_stepNumber++]);
        }

        public event Action<Point> MakeMove;
    }
}
