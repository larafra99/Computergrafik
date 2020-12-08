using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Engine.Core.Effects;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;   
        private Transform _cubeTransform;
        private Transform _cubeTransform2;
        private Transform _cubeTransform3;
        private DefaultSurfaceEffect _cubeEffect;

        private float _camAngle = 0;



        public override void Init()
    {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.5f, 0, 1, 0.4f);

            // Create a scene with a cube
            // The three components: one Transform, one ShaderEffect (blue material) and the Mesh
            _cubeTransform = new Transform {Scale = new float3(1, 1, 1), Translation = new float3(-50, 50, 50)};
            _cubeTransform2 = new Transform {Scale = new float3(1,1,1), Translation = new float3(0,0,-50)};
            _cubeTransform3 = new Transform {Scale = new float3(1,1,1), Translation = new float3(0,0,-50)};
            _cubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue, float4.Zero);
            var cubeShader = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue, float4.Zero);
            var cubeShader2 = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue, float4.Zero);
            var cubeShader3 = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Yellow, float4.Zero);
            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));
            var cubeMesh2 = SimpleMeshes.CreateCuboid(new float3(5, 20, 10));
            var cubeMesh3 = SimpleMeshes.CreateCuboid(new float3(6, 7, 8));
        
            // Assemble the cube node containing the three components
            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(_cubeEffect);
            cubeNode.Components.Add(cubeMesh);
            var cubeNode2 = new SceneNode();
            cubeNode2.Components.Add(_cubeTransform2);
            cubeNode2.Components.Add(cubeShader2);
            cubeNode2.Components.Add(cubeMesh2);
            var cubeNode3 = new SceneNode();
            cubeNode3.Components.Add(_cubeTransform3);
            cubeNode3.Components.Add(cubeShader3);
            cubeNode3.Components.Add(cubeMesh3);

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cubeNode);
            _scene.Children.Add(cubeNode2);
            _scene.Children.Add(cubeNode3);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
    }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

             // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

             // Animate the camera angle
            _camAngle = _camAngle + 90.0f * M.Pi/180.0f * DeltaTime;
            _cubeEffect.SurfaceInput.Albedo = new float4(0, 0.2f + 0.8f * M.Sin(Time.TimeSinceStart), 0, 1);
             // Animate the cube
            _cubeTransform.Translation = new float3(2, 5 * M.Sin(3 * TimeSinceStart), 3);
            _cubeTransform2.Translation = new float3(12 * M.Sin(3 * TimeSinceStart), -2, 5);
            _cubeTransform3.Translation = new float3(3, 1, 6 * M.Sin(3 * TimeSinceStart));
            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

             // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

        public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        

    }
}