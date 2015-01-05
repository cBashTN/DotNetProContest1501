using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace contest.submission.contract
{
  [TestClass]
  public class Testclass
  {
    [TestMethod]
    public void Zweimal_das_gleiche_ist_gleich()
    {
      BoolArray a = new BoolArray();
      BoolArray b = new BoolArray();
      a.GenerateObstacle();
      b.Clone(a);
      Assert.AreEqual(true, b.IsEqualTo(a));
    }

    [TestMethod]
    public void Das_gleiche_per_Switch_verändert_ist_nicht_gleich()
    {
      BoolArray a = new BoolArray();
      BoolArray b = new BoolArray();
      a.GenerateObstacle();
      b.Clone(a);
      b.Switch(new Point() { x = 1, y = 1 });
      Assert.AreEqual(false, b.IsEqualTo(a));
    }


    [TestMethod]
    public void Das_gleiche_per_Switch_zweimal_verändert_ist_gleich()
    {
      BoolArray a = new BoolArray();
      BoolArray b = new BoolArray();
      a.GenerateObstacle();
      b.Clone(a);
      b.Switch(new Point() { x = 1, y = 1 });
      b.Switch(new Point() { x = 1, y = 1 });
      Assert.AreEqual(true, b.IsEqualTo(a));
    }

    [TestMethod]
    public void Anzahl_von_Anfangs_true_ist_100()
    {
      BoolArray a = new BoolArray();
      a.GenerateObstacle();

      Assert.AreEqual(100, a.CountAllTrue());
    }

    [TestMethod]
    public void An_Stelle_x_y_befindet_sich_ein_true()
    {
      BoolArray a = new BoolArray();
      a.Switch(new Point() { x = 12, y = 1 });

      Assert.AreEqual(true, a.IsTrue(new Point() { x = 12, y = 1 }));
      Assert.AreEqual(false, a.IsTrue(new Point() { x = 13, y = 1 }));
    }

    [TestMethod]
    public void Darf_nur_einen_Schritt_gehen()
    {
      BoolArray a = new BoolArray();
      a.GenerateObstacle();
      // Ja klappt
      Assert.IsTrue(a.IsMoveAllowed(new Point() { x = 10, y = 10 }, new Point() { x = 11, y = 10 }));
      // Mehr als ein Schritt gegangen
      Assert.IsFalse(a.IsMoveAllowed(new Point() { x = 20, y = 30 }, new Point() { x = 22, y = 30 }));
      Assert.IsFalse(a.IsMoveAllowed(new Point() { x = 20, y = 30 }, new Point() { x = 20, y = 32 }));
    }

    [TestMethod]
    public void Darf_an_die_Position_gesetzt_werden()
    {
      BoolArray a = new BoolArray();
      a.GenerateObstacle();
      // Mehr als ein Schritt gegangen
      Assert.IsTrue(a.IsMoveAllowed(new Point() { x = 20, y = 30 }, new Point() { x = 21, y = 30 }));
      // Hindernis
      Assert.IsFalse(a.IsMoveAllowed(new Point() { x = 899, y = 899 }, new Point() { x = 900, y = 900 }));
    }

    [TestMethod]
    public void Muss_innerhalb_von_ground_bleiben()
    {
      BoolArray a = new BoolArray();
      a.GenerateObstacle();
      Assert.IsFalse(a.IsMoveAllowed(new Point() { x = 0, y = 0 }, new Point() { x = -1, y = 0 }));
      Assert.IsFalse(a.IsMoveAllowed(new Point() { x = 0, y = 0 }, new Point() { x = 0, y = -1 }));
      Assert.IsFalse(a.IsMoveAllowed(new Point() { x = 1031, y = 1031 }, new Point() { x = 1032, y = 1032 }));
      Assert.IsFalse(a.IsMoveAllowed(new Point() { x = 1031, y = 1031 }, new Point() { x = 1031, y = 1032 }));
    }
  }
}
