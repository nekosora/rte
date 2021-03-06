﻿using RTE.Engine.Vertexes;
using RTE.OBJReader.Data;
using RTE.OBJReader.OBJFormat;
using System.Collections.Generic;
using System;

namespace RTE.Engine
{
    class MeshData
    {
        public readonly Vertex8[] Vectors;
        public readonly uint[] Indexes;

        public MeshData(DataObject data)
        {
            List<IFaceUnit> units = new List<IFaceUnit>();

            foreach (DataFace face in data.Faces)
                foreach (IFaceUnit unit in face.OBJFace.Units)
                    units.Add(unit);

            Indexer indexer = new Indexer(units.ToArray());

            Indexes = indexer.Indexes;


            List<Vertex8> vectors = new List<Vertex8>();

            foreach (IFaceUnit unit in indexer.UniqueFaces)
                switch (unit)
                {
                    case FaceUnitVTN vtn:
                        int v = vtn.V - 1;
                        int n = vtn.N - 1;
                        int t = vtn.T - 1;

                        vectors.Add(new Vertex8(
                            data.Vertices[v].Data[0],
                            data.Vertices[v].Data[1],
                            data.Vertices[v].Data[2],
                            data.VertexNormals[n].Data[0],
                            data.VertexNormals[n].Data[1],
                            data.VertexNormals[n].Data[2],
                            data.VertexTextures[t].Data[0],
                            data.VertexTextures[t].Data[1]
                            ));
                        break;

                    default:
                        throw new Exception("Unsupported type!");
                }

            Vectors = vectors.ToArray();
        }
    }
}
