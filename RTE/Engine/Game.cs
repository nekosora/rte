﻿using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using RTE.Engine.Materials;

namespace RTE.Engine
{
    class Game
    {
        public readonly string VideoVersion;

        private readonly Camera camera;

        private readonly MousePosition mousePosition;

        private readonly HashSet<Key> pressedKeys = new HashSet<Key>();
        
        private Actor actor;
        private Scene scene;

        private GameWindow gameWindow;

        public Game(int width, int height, string title, int pixelSize = 1)
        {
            gameWindow = new GameWindow(
                width,
                height,
                new GraphicsMode(new ColorFormat(32), 8),
                title,
                GameWindowFlags.Default
                );

            gameWindow.VSync = VSyncMode.On;
            gameWindow.CursorVisible = false;

            mousePosition = new MousePosition();

            GL.Enable(EnableCap.Texture2D);

            VideoVersion = GL.GetString(StringName.Version);

            Viewport.SetPixelSize(pixelSize);

            camera = new Camera(
                new Vector3(0.0f, 0.0f, -2.0f),
                new Vector3(0.0f, 0.0f, -1.0f)
                );

            var client = gameWindow.ClientRectangle;

            MeshRenderer.SetCamera(camera);
            MeshRenderer.SetPerspectiveAspect(
                    client.Width / (float)client.Height
                    );

            gameWindow.KeyDown += OnKeyDown;
            gameWindow.KeyUp += OnKeyUp;
            gameWindow.Load += OnLoad;
            gameWindow.FocusedChanged += OnFocusedChanged;
            gameWindow.UpdateFrame += OnUpdateFrame;
            gameWindow.RenderFrame += OnRenderFrame;
            gameWindow.Resize += OnResize;
        }

        private static void CheckOpenGLError()
        {
            ErrorCode errCode = GL.GetError();

            if (errCode != ErrorCode.NoError)
                throw new Exception(
                    $"OpenGl error! - {errCode}"
                );
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            pressedKeys.Add(e.Key);

            if (e.Key == Key.F11)
                gameWindow.WindowState = gameWindow.WindowState == WindowState.Fullscreen
                    ? WindowState.Normal
                    : WindowState.Fullscreen;

            if (e.Key == Key.Escape)
                gameWindow.Exit();
        }

        private void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Viewport.Resize(gameWindow.ClientRectangle);
            
            Color4 specularColor = Color4.Yellow;

            Material defMaterial = new MaterialDefault(
                "def",
                new Texture("EmptyTexture.png"),
                specularColor,
                16
                );

            Material texMaterial = new MaterialDefault(
                "tex",
                new Texture("BaseTexture.png"),
                Color.Green,
                16
                );

            Material emissiveMaterial = new MaterialEmissive(
                "em",
                new Texture("BaseTexture2.png"),
                Color4.White
                );

            scene = new Scene("main");

            scene.AddActor(new Actor(
                    "actor",
                    new Mesh("Icosphere.obj", defMaterial)
                    ));

            scene.AddActor(new Actor(
                    "block",
                    new Mesh("Block.obj", defMaterial),
                    new Transform(new Vector3(4, 0, 0), new Quaternion(0, 0.1f, 0.6f))
                    ));

            scene.AddActor(new Actor(
                    "block2",
                    new Mesh("Block.obj", texMaterial),
                    new Transform(new Vector3(2, 1, 2), new Quaternion(0.4f, 0, -0.6f))
                    ));

            scene.AddActor(new Actor(
                    "stone",
                    new Mesh("Stone.obj", texMaterial),
                    new Transform(new Vector3(-3, 0, 2))
                    ));

            scene.AddActor(new Actor(
                    "glow",
                    new Mesh("Glow.obj", emissiveMaterial),
                    new Transform(
                        new Vector3(4.5f, -1.5f, -3),
                        new Quaternion(2.6f, -1.2f, -1),
                        Vector3.One * 0.9f
                        )
                    ));

            scene.AddActor(new Actor(
                    "lamp",
                    new Mesh("Lamp.obj", emissiveMaterial),
                    new Transform(
                        new Vector3(-1.5f, -1.2f, 0),
                        new Quaternion(1.2f, 1.2f, 1),
                        Vector3.One * 0.5f
                        )
                    ));

            scene.AddActor(new Actor(
                    "lamp2",
                    new Mesh("Lamp.obj", emissiveMaterial),
                    new Transform(
                        new Vector3(3, 3, 1),
                        new Quaternion(2.2f, 1.2f, 0),
                        Vector3.One * 0.5f
                        )
                    ));

            actor = scene.GetActor("actor");

            scene.BackgroundColor = Color4.DarkSlateBlue;

            Material base2 = new MaterialDefault(
                "base",
                new Texture("BaseTexture2.png"),
                specularColor,
                16
                );

            for (int x = -10; x < 10; x++)
                for (int y = -10; y < 10; y++)
                    scene.AddActor(new Actor(
                        $"obj.{x}.{y}",
                        new Mesh("Bricks.obj", base2),
                        new Transform(new Vector3(x * 2, -4, y * 2))
                        ));
            

            scene.AmbientColor = new Vector3(0.1f, 0.1f, 0.1f);

            scene.Lights = new PointLight[3]
            {
                new PointLight(Color4.Red, new Vector3(-1.5f, -1.2f, 0)),
                new PointLight(Color4.Blue, new Vector3(4.5f, -1.5f, -3)),
                new PointLight(Color4.Green, new Vector3(3, 3, 1))
            };
            
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            CheckOpenGLError();
        }

        private void OnFocusedChanged(object sender, EventArgs e)
        {
            mousePosition.Update();
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            if (gameWindow.Focused)
            {
                const float sensitivity = 0.3f;

                Vector2 delta = mousePosition.Update();

                camera.Rotate(delta * sensitivity);
            }


            float cameraSpeed = 2.0f * (float) e.Time;

            if (pressedKeys.Contains(Key.W))
                camera.Move(cameraSpeed * camera.Front);

            if (pressedKeys.Contains(Key.S))
                camera.Move(cameraSpeed * -camera.Front);

            if (pressedKeys.Contains(Key.A))
                camera.Move(-Vector3.Normalize(
                    Vector3.Cross(camera.Front, Vector3.UnitY)) * cameraSpeed
                    );

            if (pressedKeys.Contains(Key.D))
                camera.Move(Vector3.Normalize(
                    Vector3.Cross(camera.Front, Vector3.UnitY)) * cameraSpeed
                    );


            const float rotationSpeed = 0.1f;

            Transform tr = actor.Transform;

            if (pressedKeys.Contains(Key.Number1))
                tr.RotateByX(rotationSpeed);

            if (pressedKeys.Contains(Key.Number2))
                tr.RotateByX(-rotationSpeed);

            if (pressedKeys.Contains(Key.Number3))
                tr.RotateByY(rotationSpeed);

            if (pressedKeys.Contains(Key.Number4))
                tr.RotateByY(-rotationSpeed);

            if (pressedKeys.Contains(Key.Number5))
                tr.RotateByZ(rotationSpeed);

            if (pressedKeys.Contains(Key.Number6))
                tr.RotateByZ(-rotationSpeed);

            const float scaleSpeed = 0.1f;

            if (pressedKeys.Contains(Key.Z))
                tr.ScaleByX(scaleSpeed);

            if (pressedKeys.Contains(Key.X))
                tr.ScaleByX(-scaleSpeed);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            scene.Draw();

            gameWindow.SwapBuffers();
        }

        private void OnResize(object sender, EventArgs e)
        {
            var client = gameWindow.ClientRectangle;

            Viewport.Resize(client);
            
            MeshRenderer.SetPerspectiveAspect(
                client.Width / (float)client.Height
                );
        }

        public void Run(double updateRate)
        {
            gameWindow.Run(updateRate);
        }
    }
}
