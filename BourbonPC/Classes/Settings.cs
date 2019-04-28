using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Drawing;

namespace Bourbon
{
    class Settings
    {

        public static bool SetupNeeded {
            get {
                return GamePath == "" || !File.Exists(Path.Combine(GamePath, "Bourbon.exe"));
            }
        }

        public static string GamePath
        {
            get
            {
                FixRegistryPolution();
                return BourbonRegistry.GetValue("CoHPath", "").ToString();
            }
            set
            {
                BourbonRegistry.SetValue("CoHPath", value);
            }
        }

        public static bool QuitOnLaunch
        {
            get
            {

                return BourbonRegistry.GetValue("QuitOnLaunch", "FALSE").ToString().ToUpper() == "TRUE";
            }
            set
            {
                if(value) 
                    BourbonRegistry.SetValue("QuitOnLaunch", "TRUE");
                else
                    BourbonRegistry.SetValue("QuitOnLaunch", "FALSE");
            }
        }

        public static string GameParams
        {
            get
            {
                return BourbonRegistry.GetValue("Parameters", "").ToString();
            }
            set
            {
                BourbonRegistry.SetValue("Parameters", value);
            }
        }

        public static Color BGColor
        {
            get
            {
                int color;
                bool success = int.TryParse(BourbonRegistry.GetValue("BGColor", -13017488).ToString(), out color);
                if (success) return Color.FromArgb(color);
                else return Color.Black;
            }
            set
            {
                BourbonRegistry.SetValue("BGColor", value.ToArgb());
            }
        }

        public static Color TextColor
        {
            get
            {
                int color;
                bool success = int.TryParse(BourbonRegistry.GetValue("TextColor", -1).ToString(), out color);
                if (success) return Color.FromArgb(color);
                else return Color.Black;
            }
            set
            {
                BourbonRegistry.SetValue("TextColor", value.ToArgb());
            }
        }

        public static List<string> Manifests
        {
            get
            {
                char[] splitChars = {'\n'};
                return BourbonRegistry.GetValue("Manifests", "").ToString().Split(splitChars, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }
            set
            {
                string strManifests = "";
                foreach (string Manifest in value)
                {
                    strManifests += Manifest.Trim() + "\n";
                }

                if (strManifests.EndsWith("\n")) strManifests = strManifests.Substring(0, strManifests.Length - 1);
                BourbonRegistry.SetValue("Manifests", strManifests);
            }
        }

        public static string LastManifest
        {
            get
            {
                return BourbonRegistry.GetValue("LastManifest", "").ToString();
            }
            set
            {
                BourbonRegistry.SetValue("LastManifest", value);
            }
        }

        public static void Reset() {
            BourbonRegistry.DeleteValue("CoHPath");
        }

        private static RegistryKey BourbonRegistry {
            get
            {
                RegistryKey r = Registry.CurrentUser.OpenSubKey(@"Software\Bourbon\Settings", true);

                if (r == null)
                {
                    r = Registry.CurrentUser.CreateSubKey(@"Software\Bourbon\Settings");
                }

                return r;
            }
        }

        private static void FixRegistryPolution()
        {
            try
            {
                string s = Registry.CurrentUser.GetValue("CoHPath", "").ToString();
                if (s != "")
                {
                    GamePath = s;
                    Registry.CurrentUser.DeleteValue("CoHPath");
                }
            }
            catch (Exception ex) { }
        }



    }
}
