using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA
{
    struct MyVertexType : IVertexType  
    {
        Vector3 position;
        Vector2 texture;
        float light;

        public MyVertexType(Vector3 pos, Vector2 texture, float light)
        {
            this.position = pos;
            this.texture = texture;
            this.light = light;
        }

        public readonly static VertexDeclaration Declaration = 
            new VertexDeclaration(
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0),
                    new VertexElement(sizeof(float) * 5, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0));

        public VertexDeclaration VertexDeclaration { get { return Declaration; } }
    }
}
