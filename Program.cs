﻿using System;
using System.Collections.Generic;
using OpenGLEngine.Engine;


namespace OpenGLEngine
{
    class Program
    {
        static void ShowShadersInfo(ShaderProgram shader)
        {
            foreach (KeyValuePair<string, int> pair in shader.GetAttributes("coord", "tex_coord"))
                Console.WriteLine(pair.Key + ": " + pair.Value);

            foreach (KeyValuePair<string, int> pair in shader.GetUniforms("color", "tex"))
                Console.WriteLine(pair.Key + ": " + pair.Value);

            foreach (Shader sh in shader.Shaders)
                Console.Write(sh.Name + ": " + sh.GetLogInfo());
        }

        static void Main(string[] args)
        {
            Game game = new Game(800, 600, "Engine");

            Console.WriteLine(game.VideoVersion);


            ShaderProgram shader = new ShaderProgram(
                new VertexShader("vertexShader.glsl"),
                new FragmentShader("fragmentShader.glsl")
                );

            ShowShadersInfo(shader);

            game.SetShaderProgram(shader);
            game.Run(10);
        }
    }
}
