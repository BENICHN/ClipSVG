using System;
using System.Text.RegularExpressions;
using System.Globalization;
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

                            if (MessageBox.Show(Sentences.INFO_Success, "ClipSVG", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes) Process.Start(sfd.FileName);
                        }
                    }
                }
            }
        }
    }

    public static class Sentences //We don't use .resx files because it adds .dll files to the builds.
    {
        public static string INFO_Success
        {
            get
            {
                switch (CultureInfo.DefaultThreadCurrentCulture.ToString())
                {
                    case "fr-FR":
                        return "Fichier enregistré." + Environment.NewLine + "Voulez-vous l'ouvrir ?";
                    case "es-ES":
                        return "Archivo guardado." + Environment.NewLine + "¿Quiere abrirlo?";
                    default:
                        return "File saved." + Environment.NewLine + "Do you want to open it ?";
                }
            }
        }
    }
}
