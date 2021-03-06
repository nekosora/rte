﻿using RTE.Engine.Shaders;

namespace RTE.Engine
{
    static class MaterialShaders
    {
        private static ShaderProgram _default;
        public static ShaderProgram Default
        {
            get => _default ?? (_default = new ShaderProgram(
                new ShaderVertex("meshVS.glsl"),
                new ShaderFragment("meshFS.glsl")
                ));
        }

        private static ShaderProgram emissive;
        public static ShaderProgram Emissive
        {
            get => emissive ?? (emissive = new ShaderProgram(
                new ShaderVertex("meshVS.glsl"),
                new ShaderFragment("emissiveFS.glsl")
                ));
        }

        private static ShaderProgram gouraud;
        public static ShaderProgram Gouraud
        {
            get => gouraud ?? (gouraud = new ShaderProgram(
                new ShaderVertex("meshGouraudVS.glsl"),
                new ShaderFragment("meshGouraudFS.glsl")
                ));
        }

        private static ShaderProgram solid;
        public static ShaderProgram Solid
        {
            get => solid ?? (solid = new ShaderProgram(
                new ShaderVertex("meshVS.glsl"),
                new ShaderFragment("solidFS.glsl")
                ));
        }

        private static ShaderProgram transparent;
        public static ShaderProgram Transparent
        {
            get => transparent ?? (transparent = new ShaderProgram(
                new ShaderVertex("meshVS.glsl"),
                new ShaderFragment("transparentFS.glsl")
                ));
        }
    }
}
