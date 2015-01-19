using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using contestrunner.contract.host;
using contest.submission.contract;
using System.Drawing;
using System.Windows.Forms;

using Point = contest.submission.contract.Point;

namespace contest.host
{

  public class Dnp1501Host : IHost
  {
    int counter = 0;
    const int maxsteps = 40000;

    BoolArray ground = new BoolArray();
    Point startposition = new Point() { x = 0, y = 0 };
    Point endpostion = new Point() { x = 1023, y = 1023 };


    Point currentposition = new Point();

    string stopmessage = "";
    
    public void Prüfen(object beitrag, string wettbewerbspfad, string beitragsverzeichnis)
    {
      var sut = (IDnp1501Solution)beitrag;
      var steps = new List<Point>();

      sut.MakeMove += (nextposition) =>
      {
          counter++;
          if (!ground.IsMoveAllowed(currentposition, nextposition))
            stopmessage = string.Format("Unerlaubter Zug: von {0}, {1} auf {2}, {3}", currentposition.x, currentposition.y, nextposition.x, nextposition.y);

          currentposition.Clone(nextposition);
          steps.Add(new Point { x = nextposition.x, y = nextposition.y });

          if (nextposition.IsEqual(endpostion)) stopmessage = "********** Ziel erreicht *********";
        };
     
      DateTime starttime = DateTime.Now;

        ground.GenerateObstacle();
    
      
      currentposition.Clone(startposition);
      sut.Start(ground, startposition, endpostion);
   
      for (int i = 0; i < maxsteps; i++)
      {
        if (stopmessage != "") break;
        sut.NextStep();
      }
      
      var anfang = new Prüfungsanfang { Wettbewerb = Path.GetFileName(wettbewerbspfad), Beitrag = Path.GetFileName(beitragsverzeichnis) };
      Anfang(anfang);

      Status(new Prüfungsstatus() { Statusmeldung = "Anzahl der Schritte: " + counter });
      Status(new Prüfungsstatus() { Statusmeldung = stopmessage });

      Ende(new Prüfungsende(){ Dauer = DateTime.Now.Subtract(starttime) });

      var f = new Form { ClientSize = new Size(1024, 1024), FormBorderStyle = FormBorderStyle.Fixed3D };

      var b = new Bitmap(1024, 1024);

      for (int i = 0; i < 1024; i++)
      {
          for (int k = 0; k < 1024; k++)
          {
              b.SetPixel(i, k, ground.Data[i, k] ? Color.Black : Color.SteelBlue);
          }
      }

      var g = Graphics.FromImage(b);

      for (int i = 0; i < steps.Count; i++)
      {
          var p = steps[i];

          b.SetPixel(p.x, p.y, Color.White);

          if (i % 1000 == 0)
          {
              g.DrawString(i.ToString(CultureInfo.InvariantCulture), new Font("Calibri", 8), Brushes.White, p.x, p.y);
          }
      }

      f.Paint += (sender, args) => args.Graphics.DrawImage(b, 0, 0);

      f.ShowDialog();

    }

    public event Action<Prüfungsanfang> Anfang;
    public event Action<Prüfungsstatus> Status;
    public event Action<Prüfungsende> Ende;
    public event Action<Prüfungsfehler> Fehler;


    public BoolArray GenerateObstacleFromBitmap(String bitmapCopiedContentName, BoolArray ground, int scale)
    {
        var b = new Bitmap(bitmapCopiedContentName, true);

        int shiftX = (1024 - b.Width * scale) / 2;
        int shiftY = (1024 - b.Height * scale) / 2;

        for (int i = 0; i < b.Width; i++)
        {
            for (int k = 0; k < b.Height; k++)
            {
                var pixel = b.GetPixel(i, k);

                if (pixel.R < 128 && pixel.G < 128 && pixel.B < 128)
                {
                    for (int x = 0; x < scale; x++)
                    {
                        for (int y = 0; y < scale; y++)
                        {
                            ground.Data[shiftX + i * scale + x, shiftY + k * scale + y] = true;
                        }
                    }
                }
            }
        }
        return ground;
    }
  }

}
