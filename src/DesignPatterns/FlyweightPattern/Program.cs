using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlyweightPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Flyweight Pattern!");

            BenchmarkRunner.Run<GameBenchmarks>();
        }
    }

    // dotnet add package BenchmarkDotNet
    [MemoryDiagnoser]
    public class GameBenchmarks
    {
        private const int N = 10_000;

        private readonly Problem.Game problemGame = new Problem.Game(Problem.TreeFactory.Create(N));
        private readonly Solution.Game solutionGame = new Solution.Game(Solution.TreeFactory.Create(N));

        [Benchmark]
        public void PlayProblemGame() => problemGame.Play();
        [Benchmark]
        public void PlaySolutionGame() => solutionGame.Play();


    }

    #region Faker

    // dotnet add package Bogus

    public class VectorFaker : Faker<Vector>
    {
        public VectorFaker()
        {
            RuleFor(p => p.X, f => f.Random.Int());
            RuleFor(p => p.Y, f => f.Random.Int());
        }
    }

    public class MeshFaker : Faker<Mesh>
    {
        public MeshFaker()
        {
            RuleFor(p => p.Size, f => f.Random.Int(100, 300));
        }
    }

    public class TextureFaker : Faker<Texture>
    {
        public TextureFaker()
        {
            RuleFor(p => p.Content, f => f.Random.String2(3));
        }
    }


    #endregion


    public class Vector
    {
        public Vector()
        {

        }

        public Vector(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Mesh
    {

        public Mesh()
        {

        }

        public Mesh(int size)
            : this()
        {
            Size = size;
        }

       

        public int Size { get; set; }

        public override string ToString() => Size.ToString();
    }

    public class Texture
    {
        public Texture()
        {

        }

        public Texture(string content)
            : this()
        {
            Content = content;
        }

        public string Content { get; set; }

        public override string ToString() => Content;
    }

    public struct Color
    {
        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public override string ToString() => $"(R={Red}, G={Green}, B={Blue})";

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }
}
