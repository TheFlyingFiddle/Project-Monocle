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
using Monocle.Content;

namespace Monocle
{

    class HelloGL3 : GameWindow
    {
        Texture2D texture;
        Texture2D texture1;
        Batch batch;

        FontBatch fontBatch;
        TextureFont font;
        TextureFont font1;
        TextureFont font2;

        EditableText text;

        public HelloGL3()
            : base(1280, 720,
            new GraphicsMode(), "OpenGL 3 Example", 0,
            DisplayDevice.Default, 3, 0,
            GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug)
        { }
        
        protected override void OnLoad (System.EventArgs e)
        {
            VSync = VSyncMode.On;

            var loader = new ResourceLoader();
            loader.AddImportersAndProcessors(this.GetType().Assembly);

            ResourceContext resourceContext = new ResourceContext(new TypeReaderFactory(), new TypeWriterFactory(), loader, Environment.CurrentDirectory + "\\Assets", Environment.CurrentDirectory);
            resourceContext.ShouldSaveAllLoadedAssets = false;
            
            this.texture = resourceContext.LoadAsset<Texture2D>("overButton.png");
            this.texture1 = resourceContext.LoadAsset<Texture2D>("pressedButton.png");
            
            this.font = resourceContext.LoadAsset<TextureFont>("Arial_22.fnt");
            this.font1 = resourceContext.LoadAsset<TextureFont>("Consolas.fnt");
            this.font2 = resourceContext.LoadAsset<TextureFont>("Arial_No_Rly.fnt");
                        
            Effect effect = resourceContext.LoadAsset<Effect>("Basic.effect");

            this.fontBatch = new FontBatch(effect);
            batch = new Batch(effect);
            text = new EditableText(-1, true);


            // Other state
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ClearColor(System.Drawing.Color.MidnightBlue);

            this.Keyboard.KeyDown += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyDown);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[OpenTK.Input.Key.Escape])
                Exit();
        }

        int cursorPos;
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            char c = e.KeyChar;
            cursorPos = text.ProcessChar(c, cursorPos);
        }

        void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Delete)
                text.ProcessChar('\u007F', cursorPos);
            else if (e.Key == OpenTK.Input.Key.Left)
                cursorPos = (cursorPos == 0) ? cursorPos : (cursorPos - 1);
            else if (e.Key == OpenTK.Input.Key.Right)
                cursorPos = (cursorPos == text.Length) ? cursorPos : cursorPos + 1;
            else if (e.Key == OpenTK.Input.Key.Up)
                cursorPos = this.LastNewLineIndex();
            else if (e.Key == OpenTK.Input.Key.Down)
                cursorPos = this.NextNewLineIndex();
        }

        private int LastNewLineIndex()
        {
            for (int i = cursorPos - 1; i >= 1; i--)
            {
                if (this.text.ToString()[i] == '\n')
                    return i;
            }
            return cursorPos;
        }

        private int NextNewLineIndex()
        {
            for (int i = cursorPos + 1; i < text.Length; i++)
            {
                if (this.text.ToString()[i] == '\n')
                    return i;
            }

            return cursorPos;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);

            fontBatch.Begin(ref projectionMatrix);
            Vector2 origin = font.MessureString("The brown dog jumped over the lazy fox!?");
            fontBatch.Draw(font, "The brown dog jumped over the lazy fox!?", Vector2.Zero);
            fontBatch.Draw(font, "The brown dog jumped over the lazy fox!?", new Vector2(0, origin.Y), Color4.Black);
            fontBatch.Draw(font, "This is a \nmulti line text!", new Vector2(0, origin.Y) * 2, Color4.Yellow);
                       
            fontBatch.Draw(font, "This is scaled text!.", new Vector2(0, origin.Y) * 4, Color4.Green, Vector2.Zero, new Vector2(2,2));
            fontBatch.Draw(font, "This is mirrored text!.", new Vector2(0, origin.Y) * 6, Color4.Red, Vector2.Zero, Vector2.One, 0, true);
            fontBatch.Draw(font, "This is rotated and scaled text!.", new Vector2(15, origin.Y) * 8, Color4.Purple, Vector2.Zero, new Vector2(1.5f,1.4f), 0.5f, false);

            fontBatch.Draw(font1, "This is a consolas text!", new Vector2(400, 100), Color4.DimGray, Vector2.Zero);
            fontBatch.Draw(font2, "This is a small arial text.", new Vector2(700, 300), Color4.Gold, Vector2.Zero);

            fontBatch.Draw(font1, text.ToString(), new Vector2(650, 0), Color4.White);

            fontBatch.Draw(font2, "This is a small very scaled font text.", new Vector2(300, 400), Color4.Gold, Vector2.Zero, new Vector2(5,5));

            fontBatch.End();
            
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