using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA
{
    class CubeShape
    {
        public Vector3 Size { get; set; }
        public Vector3 Position { get; set; }

        private VertexPositionColor[] ShapeVertixes { get; set; }
        private VertexBuffer Buffer { get; set; }
        private Quad Quad { get; set; }
        public CubeShape()
        {
            Quad = new Quad();
            Quad.Build();
            ShapeVertixes = new VertexPositionColor[2]{
                new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f), new Color(1f, 0.0f, 0.0f)),
                new VertexPositionColor(new Vector3(0.0f, 1.0f, 0.0f), new Color(0.0f, 1.0f, 0.0f))
            };
        }
        public void Render(GraphicsDevice device, Texture2D texture)
        {
            var effect = new BasicEffect(device);
            effect.TextureEnabled = true;
            effect.LightingEnabled = false;
            effect.View = Matrix.Identity * Matrix.CreateRotationX(0.4f);
            effect.Texture = texture;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {                
                pass.Apply();
                //todo: draw buffer in future
                //Buffer = new VertexBuffer(device, typeof(VertexPositionColor), 2, BufferUsage.WriteOnly);
                //Buffer.SetData(ShapeVertixes);
                //device.SetVertexBuffer(Buffer);
                //device.DrawPrimitives(PrimitiveType.LineList, 0, 2);                
                var buffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), 36, BufferUsage.WriteOnly);
                buffer.SetData(Quad.shapeVertices);
                device.SetVertexBuffer(buffer);
                device.DrawUserPrimitives(PrimitiveType.TriangleList, Quad.shapeVertices, 0, 12);
            }
        }
    }
}
