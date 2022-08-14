using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simpleBench
{
    public static class WriteLog
    {
        public static void Write(List<string> journal, string typ)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string[] lines = journal.ToArray();


            // + jour + heure + minuteSeconds;
            DateTime maint = DateTime.Now;
            string jour = string.Empty;
            if (!string.IsNullOrEmpty(typ))
                jour += @"\7z";
            else
                jour += @"\";
            jour += string.Format("Log-SimpleBench-{0:MM-dd-yy_H}h", maint);
            string heure = DateTime.Now.ToString("mm");
            string minute = "m" + DateTime.Now.ToString("ss") + "s.txt";

            // + jour + heure + minuteSeconds;
            StringBuilder dateFile = new StringBuilder();
            dateFile.Append(desktopPath);
            dateFile.Append(jour);
            dateFile.Append(heure);
            dateFile.Append(minute);

            File.WriteAllLines(dateFile.ToString(), lines);

            MessageBox.Show($"Le fichier log\n{Path.GetFileName(dateFile.ToString())}\nà bien été créé sur le bureau",
                            "Bench by Monarc's log",
                             MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
