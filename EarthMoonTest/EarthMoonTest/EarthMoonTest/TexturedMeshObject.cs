using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace EarthMoonTest
{
    public class TexturedMeshObject
    {
        private Model model;

        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public TexturedMeshObject(GraphicsDevice device, 
            ContentManager contentManager, string modelName)
        {
            model = contentManager.Load<Model>(modelName);

            // Setup Default Matices
            this.World = Matrix.Identity;
            this.View = Matrix.CreateLookAt(
                new Vector3(0f, 0f, 50f), Vector3.Zero, Vector3.Up);
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 100.0f);
        }

        public void Draw()
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];

            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (IEffectMatrices effect in mesh.Effects)
                {
                    effect.World = transforms[mesh.ParentBone.Index] * this.World;
                    effect.View = this.View;
                    effect.Projection = this.Projection;
                }

                mesh.Draw();
            }
        }
    }
}
