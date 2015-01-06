using System;
using contest.submission.contract;

namespace contest.submission
{

    [Serializable]
    public class Solution : IDnp1501Solution
    {
        private int _stepNumber;
        private Point[] Path { get; set; }

        public void Start(BoolArray ground, Point startpoint, Point endpoint)
        {
            IPathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Path = pathFinder.FindAPath();
            
            _stepNumber = 1; // 0 would be the startpoint

            NextStep();
        }

        public void NextStep()
        {

            MakeMove(Path[_stepNumber++]);
        }

        public event Action<Point> MakeMove;
    }
}
