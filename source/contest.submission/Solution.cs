using System;
using contest.submission.contract;

namespace contest.submission
{
 
  [Serializable]
  public class Solution : IDnp1501Solution
  {
    Point startpoint, endpoint;
    static Point currentposition;
    public void Start(BoolArray ground, Point startpoint, Point endpoint)
    {
      this.startpoint = startpoint;
      this.endpoint = endpoint;
      currentposition = startpoint;
      NextStep();
    }

    public void NextStep()
    {
      Point nextposition = new Point();
      int difx = currentposition.x - endpoint.x;
      int dify = currentposition.y - endpoint.y;

      if (difx < 0) currentposition.x++;
      if (difx > 0) currentposition.x--;

      if (dify < 0) currentposition.y++;
      if (dify > 0) currentposition.y--;
      
      MakeMove(currentposition);

    }

    public event Action<Point> MakeMove;
  }
}
