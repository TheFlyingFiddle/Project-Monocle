using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monocle.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using Monocle.Graphics;
using System.IO;
using System.Runtime.InteropServices;
using Monocle.Content.Serialization;
using Monocle.GUI;

namespace Monocle
{

    class HelloGL3 : GameWindow
    {

        Texture2D texture;
        Texture2D texture1;
        Batch batch;
        GUIRenderingContext context;
        ToggleButton button;


        public HelloGL3()
            : base(1280, 720,
            new GraphicsMode(), "OpenGL 3 Example", 0,
            DisplayDevice.Default, 3, 0,
            GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug)
        { }
        
        protected override void OnLoad (System.EventArgs e)
        {
            VSync = VSyncMode.On;
            batch = new Batch();

            var elements = new Dictionary<string, VisibleElement>();

            using (Stream stream = new FileStream("overButton.png", FileMode.Open))
            {
                this.texture = Texture2D.LoadTexture(stream);
                elements.Add(LookAndFeelAssets.Button_Over.ToString(), new VisibleElement(texture, texture.Bounds));
            }

            using (Stream stream = new FileStream("pressedButton.png", FileMode.Open, FileAccess.ReadWrite))
            {
                this.texture1 = Texture2D.LoadTexture(stream);
                elements.Add(LookAndFeelAssets.Button_Pressed.ToString(), new VisibleElement(texture1, texture.Bounds));
            }

            using (Stream stream = new FileStream("button.png", FileMode.Open))
            {
                var tex = Texture2D.LoadTexture(stream);
                elements.Add(LookAndFeelAssets.Button_BG.ToString(), new VisibleElement(tex, texture.Bounds));
            }

            using (Stream stream = new FileStream("downButton.png", FileMode.Open))
            {
                var tex = Texture2D.LoadTexture(stream);
                elements.Add(LookAndFeelAssets.Button_Down.ToString(), new VisibleElement(tex, texture.Bounds));
            }

            var look = new LookAndFeel(elements);

            this.context = new GUIRenderingContext(this.batch, look, Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10));

            button = new ToggleButton(this.Mouse);
            button.Bounds = new Rect(250, 250, 250 , 250);

            button.MouseEnter += (s, x) => Console.WriteLine("Mouse entered button.");
            button.MouseExit += (s, x) => Console.WriteLine("Mouse exited button.");
            button.Clicked += (s, x) => Console.WriteLine("Just clicked!");
            button.MouseDown += (s, x) => Console.WriteLine("A button was just pressed! Button: {0}", x.Button);


            // Other state
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ClearColor(System.Drawing.Color.MidnightBlue);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[OpenTK.Input.Key.Escape])
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projectionMatrix = Matrix4.CreateTranslation(200, 0, 0);

            projectionMatrix *= Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);

            batch.Begin(ref projectionMatrix);

            batch.Draw(this.texture1, new Rect(200, 400, 200, 200));
            batch.Draw(this.texture1, new Rect(200, 300, 200, 200));
            batch.Draw(this.texture, new Rect(0, 0, 200, 200));
            batch.Draw(this.texture, new Rect(400, 400, 200, 200));
            batch.Draw(this.texture, new Vector2(200,200));

            batch.End();

            this.context.Render(this.button, default(Time));
            
            SwapBuffers();
        }

        public static void Main(string[] args)
        {
            using (HelloGL3 hello = new HelloGL3())
            {
                hello.Run(30);
            }
        }
    }
}