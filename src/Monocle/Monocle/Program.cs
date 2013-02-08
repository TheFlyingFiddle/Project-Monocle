using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Examples;

namespace Monocle
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Set this to the foulder containing all assets used. 
            string resourceFolder = "C:\\Users\\Lukas\\Desktop\\MonocleResources";

            /*LabelExample example = new LabelExample(resourceFolder);
            example.Run();

            */
            ButtonExample example = new ButtonExample(resourceFolder);
            example.Run();
            /*
            AnimationExample example = new AnimationExample(resourceFolder);
            example.Run();

            TextExample example = new TextExample(resourceFolder);
            example.Run(); */

        }

    }
}
