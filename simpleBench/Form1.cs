using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using ProgressBarSample;

namespace simpleBench
{ 
    public partial class Form1 : Form
    {
        public int PrN = 0;
        public long result = 0;

        private List<string> log = new List<string>();

        TextProgressBar textProgressBar1 = new TextProgressBar();
        TextProgressBar textProgressBar2 = new TextProgressBar();

        TextProgressBar textProgressBar3 = new TextProgressBar();
        TextProgressBar textProgressBar4 = new TextProgressBar();

        private static int limit = 20_000_000;
        private List<int> numbers = Enumerable.Range(0, limit).ToList();

        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            radioButton5.Checked = true;
            

            ExtractResource("7zr", "bench7Z.exe");

            button2.Enabled = false;
            
            // text*ProgressBar bench prime
            textProgressBar1.Location = new System.Drawing.Point(0, 0);
            textProgressBar1.CustomText = "";
            textProgressBar1.VisualMode = ProgressBarDisplayMode.CustomText;
            textProgressBar1.Size = new System.Drawing.Size(512, 28);
            textProgressBar1.ForeColor = Color.WhiteSmoke;
            textProgressBar1.BackColor = Color.White;
            textProgressBar1.ProgressColor = Color.LightCoral;
            textProgressBar1.Value = 32;
            textProgressBar1.Maximum = 90_000;

            // text*ProgressBar2 bench prime
            textProgressBar2.Location = new System.Drawing.Point(0, 0);
            textProgressBar2.CustomText = "";
            textProgressBar2.VisualMode = ProgressBarDisplayMode.CustomText;
            textProgressBar2.Size = new System.Drawing.Size(512, 28);
            textProgressBar2.ForeColor = Color.WhiteSmoke;
            textProgressBar2.BackColor = Color.White;
            textProgressBar2.ProgressColor = Color.LightCoral;
            textProgressBar2.Value = 52;
            textProgressBar2.Maximum = 90_000;

            panel2.Controls.Add(textProgressBar1);
            panel3.Controls.Add(textProgressBar2);

            // text*ProgressBar bench 7z
            textProgressBar3.Location = new System.Drawing.Point(0, 0);
            textProgressBar3.CustomText = "";
            textProgressBar3.VisualMode = ProgressBarDisplayMode.CustomText;
            textProgressBar3.Size = new System.Drawing.Size(512, 28);
            textProgressBar3.TextFont = new System.Drawing.Font("Microsoft YaHei", 9F,
                                        System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point,
                                        ((byte)(0)));
            textProgressBar3.ForeColor = Color.WhiteSmoke;
            textProgressBar3.BackColor = Color.White;
            textProgressBar3.ProgressColor = Color.BlueViolet;
            textProgressBar3.Value = 32;
            textProgressBar3.Maximum = 90_000;

            // text*ProgressBar2 bench 7z
            textProgressBar4.Location = new System.Drawing.Point(0, 0);
            textProgressBar4.CustomText = "";
            textProgressBar4.VisualMode = ProgressBarDisplayMode.CustomText;
            textProgressBar4.Size = new System.Drawing.Size(512, 28);
            textProgressBar4.TextFont = new System.Drawing.Font("Microsoft YaHei", 9F,
                                        System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point,
                                        ((byte)(0)));
            textProgressBar4.ForeColor = Color.WhiteSmoke;
            textProgressBar4.BackColor = Color.White;
            textProgressBar4.ProgressColor = Color.BlueViolet;
            textProgressBar4.Value = 52;
            textProgressBar4.Maximum = 90_000;

            panel6.Controls.Add(textProgressBar3);
            panel5.Controls.Add(textProgressBar4);

        }
        // install 7z
        private void ExtractResource(string resName, string fName)
        {
            // recup du exe
            object ob = Properties.Resources.ResourceManager.GetObject(resName,
                             System.Globalization.CultureInfo.InvariantCulture);
            byte[] myResBytes = (byte[])ob;
            using (FileStream fsExe = new FileStream(fName, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = myResBytes;
                fsExe.Write(bytes, 0, bytes.Length);
                fsExe.Close();
                fsExe.Dispose();
            }
        }

        /// <summary>
        /// GetPrimeList returns Prime numbers by using sequential ForEach
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private static IList<int> GetPrimeList(IList<int> numbers) => 
            numbers.Where(IsPrime).ToList();

        /// <summary>
        /// GetPrimeListWithParallel returns Prime numbers by using Parallel.ForEach
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private static IList<int> GetPrimeListWithParallel(IList<int> numbers)
        {
            var primeNumbers = new ConcurrentBag<int>();

            Parallel.ForEach(numbers, number =>
            {
                if (IsPrime(number))
                {
                    primeNumbers.Add(number);
                }
            });

            return primeNumbers.ToList();
        }

        /// <summary>
        /// IsPrime returns true if number is Prime, else false.
        /// <param name="number"></param>
        /// <returns></returns>
        private static bool IsPrime(int number)
        {
            if (number < 2)
            {
                return false;
            }

            for (var divisor = 2; divisor <= Math.Sqrt(number); divisor++)
            {
                if (number % divisor == 0)
                {
                    return false;
                }
            }
            return true;
        }
    

    private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            label6.Visible = true;

            var limit = 52_000_000;
            var numbers = Enumerable.Range(0, limit).ToList();

            // to reset the form
            resetTab();

            //make log
            makeLog("");

            if (radioButton1.Checked)
            {
                // PrN = 550_000; // testing
                monoCoeur();
                label2.Visible = false;
                panel2.Visible = true;
                while (textProgressBar1.Maximum < (PrN / 5))
                    textProgressBar1.Maximum += 5000;
                textProgressBar1.Value = PrN/5;
                textProgressBar1.CustomText = (PrN / 5).ToString();
                textProgressBar1.Visible = true;
                button2.Font = new System.Drawing.Font("Microsoft YaHei UI Light",
                                           9.75F, System.Drawing.FontStyle.Bold,
                                           System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            }

            else if (radioButton2.Checked)
            {
                multiCoeurs();
                label2.Visible = false;
                panel3.Visible = true;
                while (textProgressBar2.Maximum < (result / 5))
                    textProgressBar2.Maximum += 5000;
                textProgressBar2.Value = (int)result/5;
                textProgressBar2.CustomText = (result/5).ToString();
                textProgressBar2.Visible = true;
                button2.Font = new System.Drawing.Font("Microsoft YaHei UI Light",
                                          9.75F, System.Drawing.FontStyle.Bold,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            else
            {
                monoCoeur();
                multiCoeurs();
                label2.Visible = false;
                panel2.Visible = true;
                panel3.Visible = true;
                while (textProgressBar2.Maximum < (result / 5))
                    textProgressBar2.Maximum += 5000;
                while (textProgressBar1.Maximum < (PrN / 5))
                    textProgressBar1.Maximum += 5000;
                textProgressBar1.Value = PrN/5;
                textProgressBar1.CustomText = (PrN/5).ToString();
                textProgressBar1.Visible = true;
                textProgressBar2.Value = (int)result/5;
                textProgressBar2.CustomText = (result / 5).ToString();
                textProgressBar2.Visible = true;
                button2.Font = new System.Drawing.Font("Microsoft YaHei UI Light",
                                          9.75F, System.Drawing.FontStyle.Bold,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            label6.Visible = false;
            button1.Enabled = true; button2.Enabled = true;
        }

        private void  makeLog( string bench)
        {
            string cpuName = string.Empty;

            log.Clear();

            ManagementObjectSearcher mos =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                cpuName = mo["Name"].ToString();
            }

            log.Add("#----------------------------------------------------#");
            log.Add("#     simpleBench is a Monarc's program ( 2022 )     #");
            log.Add("#____________________________________________________#");
            log.Add("");

            log.Add("Votre configuration:");
            log.Add("--------------------");
            log.Add("Marque carte mère: " + MotherboardInfo.Manufacturer);
            log.Add("Modèle:  " + MotherboardInfo.Product);
            log.Add("Processeur: " + cpuName);

            log.Add("____________________");
            log.Add("--------------------");

            log.Add(" ");
            log.Add($"{(result != 0 && PrN != 0 ? "Les scores" : "Le score")} de bench {(!string.IsNullOrEmpty(bench)? bench+" " : "")}{(result != 0 && PrN != 0 ? "sont" : "est")} de: ");
            log.Add("-------------------");
            log.Add(" ");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private string bench7Z(int cores)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            // cmd.StartInfo.WorkingDirectory = "C:\\Program Files\\7-Zip";
            cmd.StartInfo.FileName = "bench7Z.exe";
            cmd.StartInfo.Arguments = " b -mmt"+ cores;
            cmd.Start();

            //cmd.StandardInput.WriteLine("\7z b -mmt1");
            //cmd.StandardInput.Flush();
            //cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(); 
            
            return cmd.StandardOutput.ReadToEnd();
        }

        private string bench7ZArranger(string res)
        {
            int offset = 1;

            char[] tableau = res.ToCharArray();
            // Array.Reverse(tableau);
            for (int i = tableau.Length - 1; i >= 0; i--)
            {
                // reverse += tableau[i];
                if (tableau[i] == ' ')
                    break;
                else
                    offset = i;
            }

            string result = res.Substring(offset).Replace("\n", "").Replace("\r", ""); 
            // Console.WriteLine(result);
            return result;
        }
        private void monoCoeur()
        {
            var watch = Stopwatch.StartNew();
            var primeNumbersFromForeach = GetPrimeList(numbers);
            watch.Stop();

            PrN = (int)primeNumbersFromForeach.Count / ((int)watch.ElapsedMilliseconds / 100);
            log.Add($"Single core: {PrN}");
        }
        private void multiCoeurs()
        {
            var watchForParallel = Stopwatch.StartNew();
            var primeNumbersFromParallelForeach = GetPrimeListWithParallel(numbers);
            watchForParallel.Stop();

            result = primeNumbersFromParallelForeach.Count / ((int)watchForParallel.ElapsedMilliseconds / 100);

            log.Add($"Multi cores: {result}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WriteAllLines.Write(log, string.Empty);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void resetTab()
        {
            panel2.Visible = false;
            panel3.Visible = false;

            panel6.Visible = false;
            panel5.Visible = false;

            label3.Visible = true;
            label2.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label5.Visible = true;
            resetTab();
            button3.Enabled = false;

            int coreL = Environment.ProcessorCount;
            string res = string.Empty;
            string res2 = string.Empty;

            //make log
            makeLog("7zip");

            if (radioButton5.Checked)
            {
                res = bench7ZArranger(bench7Z(1));
                label3.Visible = false;
                panel6.Visible = true;
                textProgressBar3.Value = Convert.ToInt32(res);
                textProgressBar3.CustomText = $"{res} MIPS";
                log.Add($"Single core: {res} MIPS");
                Console.WriteLine($"Single core: {res} MIPS");
            }
            else if (radioButton4.Checked)
            {
                res = bench7ZArranger(bench7Z(coreL));
                label3.Visible = false;
                panel5.Visible = true;
                textProgressBar4.Value = Convert.ToInt32(res);
                textProgressBar4.CustomText = $"{res} MIPS";
                log.Add($"{coreL} cores: {res} MIPS");

            }
            else
            {
                res = bench7ZArranger(bench7Z(1));
                res2 = bench7ZArranger(bench7Z(coreL));

                log.Add($"Single core: {res} MIPS");
                log.Add($"{coreL} cores: {res2} MIPS");


                panel6.Visible = true;
                textProgressBar3.Value = Convert.ToInt32(res);
                textProgressBar3.CustomText = $"{res} MIPS";

                label3.Visible = false;

                panel5.Visible = true;
                textProgressBar4.Value = Convert.ToInt32(res2);
                textProgressBar4.CustomText = $"{res2} MIPS";
            }

            label5.Visible = false;
            button3.Enabled = true;
            button4.Font = new System.Drawing.Font("Microsoft YaHei UI Light",
                                          9.75F, System.Drawing.FontStyle.Bold,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WriteAllLines.Write(log, "7z");
        }
    }

    public static class WriteAllLines
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

    static public class MotherboardInfo
    {
        private static ManagementObjectSearcher baseboardSearcher = 
            new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private static ManagementObjectSearcher motherboardSearcher = 
            new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");

        static public string Availability
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return GetAvailability(int.Parse(queryObj["Availability"].ToString()));
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: "+ e.Message;
                }
            }
        }

        static public bool HostingBoard
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        using (queryObj)
                        {
                            if (queryObj["HostingBoard"].ToString() == "True")
                                return true;
                            else
                                return false;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        static public string InstallDate
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return ConvertToDateTime(queryObj["InstallDate"].ToString());
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string Manufacturer
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Manufacturer"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string Model
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Model"].ToString();
                    }
                    return "Nc";
                }
                catch (Exception e)
                {
                    return "Nc or error:"+ e.Message;
                }
            }
        }

        static public string PartNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["PartNumber"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string PNPDeviceID
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["PNPDeviceID"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string PrimaryBusType
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["PrimaryBusType"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string Product
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Product"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public bool Removable
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        if (queryObj["Removable"].ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch 
                {
                    return false;
                }
            }
        }

        static public bool Replaceable
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        if (queryObj["Replaceable"].ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch 
                {
                    return false;
                }
            }
        }

        static public string RevisionNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["RevisionNumber"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string SecondaryBusType
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["SecondaryBusType"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string SerialNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["SerialNumber"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string Status
        {
            get
            {
                try
                {
                    foreach (ManagementObject querObj in baseboardSearcher.Get())
                    {
                        return querObj["Status"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string SystemName
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["SystemName"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        static public string Version
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Version"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "erreur: " + e.Message;
                }
            }
        }

        private static string GetAvailability(int availability)
        {
            switch (availability)
            {
                case 1: return "Other";
                case 2: return "Unknown";
                case 3: return "Running or Full Power";
                case 4: return "Warning";
                case 5: return "In Test";
                case 6: return "Not Applicable";
                case 7: return "Power Off";
                case 8: return "Off Line";
                case 9: return "Off Duty";
                case 10: return "Degraded";
                case 11: return "Not Installed";
                case 12: return "Install Error";
                case 13: return "Power Save - Unknown";
                case 14: return "Power Save - Low Power Mode";
                case 15: return "Power Save - Standby";
                case 16: return "Power Cycle";
                case 17: return "Power Save - Warning";
                default: return "Unknown";
            }
        }

        private static string ConvertToDateTime(string unconvertedTime)
        {
            string convertedTime = "";
            int year = int.Parse(unconvertedTime.Substring(0, 4));
            int month = int.Parse(unconvertedTime.Substring(4, 2));
            int date = int.Parse(unconvertedTime.Substring(6, 2));
            int hours = int.Parse(unconvertedTime.Substring(8, 2));
            int minutes = int.Parse(unconvertedTime.Substring(10, 2));
            int seconds = int.Parse(unconvertedTime.Substring(12, 2));
            string meridian = "AM";
            if (hours > 12)
            {
                hours -= 12;
                meridian = "PM";
            }
            convertedTime = date.ToString() + "/" + month.ToString() + "/" + year.ToString() + " " +
            hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString() + " " + meridian;
            return convertedTime;
        }
    }
}
