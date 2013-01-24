using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Monocle.Graphics
{
    public abstract class VertexBuffer
    {
        public readonly IGraphicsContext GraphicsContext;
        internal readonly int Handle;
        protected readonly BufferUsageHint hint;

        public VertexBuffer(IGraphicsContext context, BufferUsageHint hint)
        {
            this.GraphicsContext = context;
            this.hint = hint;

            this.GraphicsContext.GenBuffers(1, out Handle);
        }
            
    
    }


    public class VertexBuffer<T>  : VertexBuffer where T : struct, IVertex
    {
        public VertexBuffer(IGraphicsContext context, BufferUsageHint hint)
            : base(context, hint)
        { }

        public void SetData(T[] data)
        {
            if(data == null)
                throw new ArgumentNullException();
            else if(data.Length < 1)
                throw new ArgumentException("Data must contain atleas one element!");

            this.GraphicsContext.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * data[0].SizeInBytes), data, hint);
        }

        public void SetSubData(T[] data, int offset)
        {
            if (data == null)
                throw new ArgumentNullException();
            else if(data.Length < 1)
                throw new ArgumentException("Data must contain atleas one element!");


            this.GraphicsContext.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr)(offset * data[0].SizeInBytes), (IntPtr)(data.Length * data[0].SizeInBytes), data);
        }

        public void SetSubData(T[] data, int offset, int count)
        {
            this.GraphicsContext.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr)(offset * data[0].SizeInBytes), (IntPtr)(count * data[0].SizeInBytes), data);
        }
    }
}