using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ClipSVG
{
    class Program
    {
        static Regex m_svg = new Regex(@"(<svg)(\s|\S)+(</svg>)");

        [STAThread]
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InstalledUICulture;

            string cliptext = Clipboard.GetText();
            string[] svgs = Regex.Split(cliptext, @"(?=<svg)");

            if (svgs.Length > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Scalable Vector Graphics (*.svg)|*.svg" })
                {
                    foreach (string s in svgs)
                    {
                        if (m_svg.IsMatch(s) && sfd.ShowDialog() == DialogResult.OK)
                        {
                            string svgtext = m_svg.Match(s).Value;

                            File.WriteAllText(sfd.FileName, svgtext);

                            if (MessageBox.Show(Resources.Resources.INFO_Success, "ClipSVG", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes) Process.Start(sfd.FileName);
                        }
                    }
                }
            }
        }
    }
}
