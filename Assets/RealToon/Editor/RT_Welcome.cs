//MJQStudioWorks
//2020

using UnityEditor;
using System.IO;

namespace RealToon.Editor.Welcome
{

    [InitializeOnLoad]
    class RT_Welcome
    {

        #region RT_Welcome
        static readonly string rt_welcome_path = "Assets/RealToon/Editor/RT_Welcome.cs";
        static readonly string rt_welcome_settings = "Assets/RealToon/Editor/RTW.sett";

        static RT_Welcome()
        {
            if (File.ReadAllText(rt_welcome_settings) == "0")
            {
                if (AssetDatabase.LoadAssetAtPath(rt_welcome_path, typeof(object)) && AssetDatabase.LoadAssetAtPath(rt_welcome_settings, typeof(object)))
                {
                    EditorApplication.delayCall += Run_Welcome;
                }
                else
                {
                    EditorUtility.DisplayDialog("The script and settings are not exist", "Cant run 'RT_Welcome.cs' and can't find 'RTW.settings'", "Ok");
                }
            }
        }

        static void Run_Welcome()
        {

            if (EditorUtility.DisplayDialog(

               "Thank you for purchasing and using RealToon Shader",

               "*The default imported RealToon Shader, Effects and Example are for Built-In RP.\n\n" +

               "*If you are an unity 2019 user and beyond and SRP user, read the 'For Unity 2019 - Beyond and SRP's users.txt' text file. \n\n" +

               "*If you are a VRoid user, read the 'For VRoid users please read.txt' text file.\n\n" +

               "*For video tutorials and user guide, see the bottom part of RealToon Inspector panel.\n\n" +

               "*If you need some help/support, just send an email including the invoice number.\n" +
               "See the 'User Guide.pdf' file for the links and email support.\n\n" +

               "*If you want RealToon Shader to work on PS4 or future PS console, just send an email and i'll help you to make it work.\n" +
               "There are some things that are needed to be done before it works on PS4/PS."
               ,

               "Ok") )

            {
                Check_Files();
                AssetDatabase.Refresh();

            }

        }

        public static void Check_Files()
        {
            if ( AssetDatabase.LoadAssetAtPath(rt_welcome_path, typeof(object)) && AssetDatabase.LoadAssetAtPath(rt_welcome_settings, typeof(object)) )
            {
                File.WriteAllText(rt_welcome_settings, "1");
            }
            else
            {
                EditorUtility.DisplayDialog("The script and settings are not exist", "Cant run 'RT_Welcome.cs' and can't find 'RTW.sett'", "Ok");
            }
        }

        #endregion

    }

}