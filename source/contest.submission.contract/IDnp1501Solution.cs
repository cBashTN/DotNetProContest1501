using System;

namespace contest.submission.contract
{
    public interface IDnp1501Solution
    {
      void Start(BoolArray ground, Point startpoint, Point endpoint);

      void NextStep();
      
      event Action<Point> MakeMove;
    }
}
