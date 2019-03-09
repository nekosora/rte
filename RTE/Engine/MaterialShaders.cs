﻿using RTE.Engine.Shaders;

namespace RTE.Engine
{
    static class MaterialShaders
    {
        private static ShaderProgram defaultMeshShader;
        public static ShaderProgram DefaultMeshShader
        {
            get
            {
                if (defaultMeshShader == null)
                    defaultMeshShader = new ShaderProgram(
                        new ShaderVertex("meshVS.glsl"),
                        new ShaderFragment("meshFS.glsl")
                        );

                return defaultMeshShader;
            }
        }

        private static ShaderProgram emissiveMeshShader;
        public static ShaderProgram EmissiveMeshShader
        {
            get
            {
                if (emissiveMeshShader == null)
                    emissiveMeshShader = new ShaderProgram(
                        new ShaderVertex("meshVS.glsl"),
                        new ShaderFragment("emissiveFS.glsl")
                        );

                return emissiveMeshShader;
            }
        }
    }
}
