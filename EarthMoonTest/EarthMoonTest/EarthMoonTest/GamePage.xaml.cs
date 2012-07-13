using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Devices.Sensors;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace EarthMoonTest
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        RenderTarget2D target2D;

        Compass compass;
        Motion motion;
        bool useMotion = false;
        Vector3 rawMagnetometerReading;
        bool isDataValid;
        bool capture = false;

        List<TexturedMeshObject> meshes;
        float earthdOffset = 240f;
        Vector3 center;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            graphicsDevice = SharedGraphicsDeviceManager.Current.GraphicsDevice;
            graphicsDevice.SetSharingMode(true);

            // Create a new RenderTarget2D to handle screen captures.
            target2D = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, false, SurfaceFormat.Alpha8, DepthFormat.Depth24);

            // Motion
            if (Motion.IsSupported)
            {
                motion = new Motion();
                motion.Start();
                useMotion = true;
            }

            if (Compass.IsSupported)
            {
                // Instantiate the compass.
                compass = new Compass();

                // Specify the desired time between updates. The sensor accepts
                // intervals in multiples of 20 ms.
                compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(compass_CurrentValueChanged);
                compass.Start();
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
            meshes = new List<TexturedMeshObject>();

            center = new Vector3(0, earthdOffset, 0);

            AddPlanetoidMesh(ref projection, center, "Earth");
            AddPlanetoidMesh(ref projection, new Vector3(2038.0f, earthdOffset, 0), "Moon");
            AddPlanetoidMesh(ref projection, new Vector3(203.8f, earthdOffset, 0), "Moon");

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        private void AddPlanetoidMesh(ref Matrix projection, Vector3 initial, string meshName)
        {
            Matrix world = Matrix.Identity;
            world = Matrix.CreateRotationX(MathHelper.ToRadians(90)) * world;
            world.M41 = initial.X;
            world.M42 = initial.Y;
            world.M43 = initial.Z;

            TexturedMeshObject mesh = 
                new TexturedMeshObject(graphicsDevice, contentManager, meshName);
            mesh.World = world;
            mesh.Projection = projection;
            meshes.Add(mesh);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Stop Accelerameter
            if (useMotion) motion.Stop();

            // Stop data acquisition from the compass.
            if (compass != null && compass.IsDataValid)
                compass.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        void compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            isDataValid = compass.IsDataValid;

            rawMagnetometerReading = e.SensorReading.MagnetometerReading;
        }

        float earthdAngle = 5f;
        float moonRotRatio = 27.3f;

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            if (useMotion)
            {
                var pitch = motion.CurrentValue.Attitude.Pitch;
                var roll = motion.CurrentValue.Attitude.Roll;
                var yaw = motion.CurrentValue.Attitude.Yaw;

                Matrix matrix = Matrix.CreateRotationX(-pitch);
                matrix = Matrix.CreateRotationY(-roll) * matrix;
                matrix = Matrix.CreateRotationZ(-yaw) * matrix;

                foreach (var mesh in meshes)
                {
                    mesh.View = matrix;
                }

                Matrix world = meshes[0].World;
                world = Matrix.CreateRotationY(
                    MathHelper.ToRadians(earthdAngle)) * world;
                meshes[0].World = world;

                RotatePlanetoid(meshes[1], center, earthdAngle / moonRotRatio);
                RotatePlanetoid(meshes[2], center, earthdAngle / moonRotRatio);
            }
        }

        private void RotatePlanetoid(
            TexturedMeshObject mesh, Vector3 center, float angle)
        {
            Matrix world = mesh.World;
            world.M41 -= center.X;
            world.M42 -= center.Y;
            world.M43 -= center.Z;
            world = world * Matrix.CreateRotationZ(
                MathHelper.ToRadians(angle));
            world.M41 += center.X;
            world.M42 += center.Y;
            world.M43 += center.Z;
            mesh.World = world;
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            if (capture)
            {
                // Set the render target to our temp target
                graphicsDevice.SetRenderTarget(target2D);
            }

            foreach (var mesh in meshes)
            {
                mesh.Draw();
            }

            if (capture)
            {
                capture = false;

                // Reset the render target to the screen
                graphicsDevice.SetRenderTarget(null);

                SaveFromTarget2D();
            }
        }


        private void SaveFromTarget2D()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                target2D.SaveAsJpeg(stream, target2D.Width, target2D.Height);
                stream.Seek(0, SeekOrigin.Begin);

                MediaLibrary library = new MediaLibrary();
                string filename = "ScreenShot_" + DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss");
                library.SavePicture(filename, stream);
            }
        }
    }
}