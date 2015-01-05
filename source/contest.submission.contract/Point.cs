using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace contest.submission.contract
{
  public class Point
  {
    public int x { get; set; }
    public int y { get; set; }

    public bool IsEqual(Point p)
    {
      if (this.x == p.x && this.y == p.y) return true;
      return false;
    }

    public void Clone(Point p)
    {
      this.x = p.x;
      this.y = p.y;
    }
  }
}
