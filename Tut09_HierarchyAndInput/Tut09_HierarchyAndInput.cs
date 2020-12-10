using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
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
    [FuseeApplication(Name = "Tut09_HierarchyAndInput", Description = "Yet another FUSEE App.")]
    public class Tut09_HierarchyAndInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private float _camAngle = 0;
        private Transform _baseTransform;
        private Transform _bodyTransform;
        private Transform _upperArmTransform;
        private Transform _foreArmTransform;
        private Transform _handTopTransform;
        private Transform _handBottomTransform;


        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };
            _bodyTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 6, 0)
            };
            _upperArmTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(2, 4, 0)
            };
            _foreArmTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-2, 8, 0)
            };
            _handTopTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 8, 2)

            };
            _handBottomTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 8, -2)

            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNode>
                {
                    new SceneNode//Grey
                    {
                        Components = new List<SceneComponent>
                        {
                            // TRANSFORM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey, float4.Zero),

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    },

                    new SceneNode
                    { //Red
                        Components = new List<SceneComponent>
                        {
                            // TRANSFORM COMPONENT
                            _bodyTransform,

                            // SHADER EFFECT COMPONENT
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.Red, float4.Zero),

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                        },

                        Children = new ChildList
                        {
                            new SceneNode
                            {
                                Components = new List<SceneComponent>
                                {
                                    // TRANSFORM COMPONENT
                                    _upperArmTransform,
                                },
                                Children = new ChildList
                                {
                                    new SceneNode// green
                                    {
                                        Components = new List<SceneComponent>
                                        {
                                            new Transform { Translation = new float3(0, 4, 0) },
                                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.Green, float4.Zero),
                                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                        }
                                    },

                                    new SceneNode // blue
                                    {
                                        Components = new List<SceneComponent>
                                        {
                                        // TRANSFORM COMPONENT
                                        _foreArmTransform,
                                        },

                                        Children = new ChildList
                                        {
                                            new SceneNode
                                            {
                                                Components = new List<SceneComponent>
                                                {
                                                    new Transform { Translation = new float3(0, 4, 0) },
                                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.Blue, float4.Zero),
                                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                                }
                                            },

                                            new SceneNode // Yellow
                                            {
                                                Components = new List<SceneComponent>
                                                {
                                                // TRANSFORM COMPONENT
                                                    _handTopTransform

                                                },
                                                Children = new ChildList
                                                {
                                                    new SceneNode
                                                    {
                                                        Components = new List<SceneComponent>
                                                        {
                                                            new Transform  { Translation = new float3(0, 2, -0.75f) },
                                                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.Yellow, float4.Zero),
                                                            SimpleMeshes.CreateCuboid(new float3(1.5f, 3, 0.5f))
                                                        }
                                                    }
                                                }
                                            },
                                            new SceneNode // Orange
                                            {
                                                Components = new List<SceneComponent>
                                                {
                                                    // TRANSFORM COMPONENT
                                                    _handBottomTransform,
                                                },
                                                Children = new ChildList
                                                {
                                                    new SceneNode
                                                    {
                                                        Components = new List<SceneComponent>
                                                        {
                                                            new Transform { Translation = new float3(0, 2, 0.75f) },
                                                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.Orange,float4.Zero),
                                                            SimpleMeshes.CreateCuboid(new float3(1.5f, 3, 0.5f))
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }


        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);


            if (Mouse.LeftButton == true)
            {
                _camAngle = _camAngle + Mouse.Velocity.x * DeltaTime / 1000;

            }

            _bodyTransform.Rotation = _bodyTransform.Rotation + new float3(0, 1.5f * Keyboard.ADAxis * Time.DeltaTime, 0);
            _upperArmTransform.Rotation = _upperArmTransform.Rotation + new float3(1.5f * Keyboard.WSAxis * Time.DeltaTime, 0, 0);
            _foreArmTransform.Rotation = _foreArmTransform.Rotation + new float3(1.5f * Keyboard.UpDownAxis * Time.DeltaTime, 0, 0);
            if(_handBottomTransform.Rotation[0] > -0.2 &&_handBottomTransform.Rotation[0] < 0.29){
                _handTopTransform.Rotation = _handTopTransform.Rotation + new float3(1.5f * Keyboard.LeftRightAxis * Time.DeltaTime, 0, 0);
                _handBottomTransform.Rotation = -_handTopTransform.Rotation + new float3(1.5f * Keyboard.LeftRightAxis * Time.DeltaTime, 0, 0);
                if(_handBottomTransform.Rotation[0] <= -0.2 ){
                    _handTopTransform.Rotation = _handTopTransform.Rotation + new float3(1.5f * Keyboard.LeftRightAxis * Time.DeltaTime, 0, 0);
                    _handBottomTransform.Rotation = -_handTopTransform.Rotation + new float3(1.5f * Keyboard.LeftRightAxis * Time.DeltaTime, 0, 0);
                }
                if(_handBottomTransform.Rotation[0] >= 0.29){
                    _handTopTransform.Rotation = _handTopTransform.Rotation + new float3(1.5f * Keyboard.LeftRightAxis * Time.DeltaTime, 0, 0);
                    _handBottomTransform.Rotation = -_handTopTransform.Rotation + new float3(1.5f * Keyboard.LeftRightAxis * Time.DeltaTime, 0, 0);
                }

            }
            


            Console.WriteLine(_handBottomTransform.Rotation);

            
            


            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_camAngle);

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