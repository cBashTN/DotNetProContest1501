using System;
using System.IO;
using contestrunner.contract.host;
using contest.submission.contract;
using System.Collections.Generic;

namespace contest.host
{

  public class Dnp1501Host : IHost
  {
    int counter = 0;
    const int maxsteps = 10000;

    BoolArray ground = new BoolArray();
    Point startposition = new Point() { x = 0, y = 0 };
    Point endpostion = new Point() { x = 1023, y = 1023 };

    Point currentposition = new Point();

    string stopmessage = "";
    
    public void Prüfen(object beitrag, string wettbewerbspfad, string beitragsverzeichnis)
    {
      var sut = (IDnp1501Solution)beitrag;

      sut.MakeMove += (nextposition) =>
        {
          if (!ground.IsMoveAllowed(currentposition, nextposition))
            stopmessage = string.Format("Unerlaubter Zug: von {0}, {1} auf {2}, {3}", currentposition.x, currentposition.y, nextposition.x, nextposition.y);

          currentposition.Clone(nextposition);

          if (nextposition.IsEqual(endpostion)) stopmessage = "********** Ziel erreicht *********";
        };
     
      DateTime starttime = DateTime.Now;

      ground.GenerateObstacle();
      
      currentposition.Clone(startposition);
      sut.Start(ground, startposition, endpostion);

      for (int i = 0; i < maxsteps; i++)
      {
        counter++;
        if (stopmessage != "") break;
        sut.NextStep();
      }
      
      var anfang = new Prüfungsanfang { Wettbewerb = Path.GetFileName(wettbewerbspfad), Beitrag = Path.GetFileName(beitragsverzeichnis) };
      Anfang(anfang);

      Status(new Prüfungsstatus() { Statusmeldung = "Anzahl der Schritte: " + counter });
      Status(new Prüfungsstatus() { Statusmeldung = stopmessage });

      Ende(new Prüfungsende(){ Dauer = DateTime.Now.Subtract(starttime) });
    }

    public event Action<Prüfungsanfang> Anfang;
    public event Action<Prüfungsstatus> Status;
    public event Action<Prüfungsende> Ende;
    public event Action<Prüfungsfehler> Fehler;
  }

}
