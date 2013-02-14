using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Examples;
using OpenTK;
using OpenTK.Graphics;
using System.Threading;
using OpenTK.Graphics.OpenGL;


namespace Monocle
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Set this to the foulder containing all assets used. 
            string resourceFolder = "C:\\Users\\Lukas\\Desktop\\MonocleResources";
            
          /*  LabelExample example = new LabelExample(resourceFolder);
            example.Run();

            /*
            
            AnimationExample example2 = new AnimationExample(resourceFolder);
            example2.Run();       */    
            
            /*ButtonExample example = new ButtonExample(resourceFolder);
            example.Run(); */



           // TextExample example = new TextExample(resourceFolder);
           // example.Run(); 

            Particles example = new Particles(resourceFolder);
            example.Run();
        }

    }
}
