using Bogus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlyweightPattern.Problem
{
   
    public class TreeFactory
    {
        public static ICollection<Tree> Create(int numberOfTrees)
        {
            TreeFaker treeFaker = new TreeFaker(new MeshFaker(), new TextureFaker(), new VectorFaker());

            return treeFaker.Generate(numberOfTrees);            
        }
    }


    public class Game
    {
        private ICollection<Tree> trees { get; set; }

        public Game(ICollection<Tree> trees)
        {
            this.trees = trees;
        }

        public void Play()
        {
            foreach (var tree in trees)
            {
                tree.Draw();
            }
        }
    }


    public class Tree
    {
        private Mesh mesh;
        private Texture bark;
        private Texture leaves;
        private Vector position;
        private double height;
        private double thickness;
        private Color barkTint;
        private Color leafTint;

        public Tree()
        {

        }

        public Tree(Mesh mesh, Texture bark, Texture leaves, Vector position, double height, double thickness, Color barkTint, Color leafTint)
            : this()
        {
            this.mesh = mesh;
            this.bark = bark;
            this.leaves = leaves;
            this.position = position;
            this.height = height;
            this.thickness = thickness;
            this.barkTint = barkTint;
            this.leafTint = leafTint;
        }

        public Mesh Mesh { get => mesh; set => mesh = value; }
        public Texture Bark { get => bark; set => bark = value; }
        public Texture Leaves { get => leaves; set => leaves = value; }
        public Vector Position { get => position; set => position = value; }
        public double Height { get => height; set => height = value; }
        public double Thickness { get => thickness; set => thickness = value; }
        public Color BarkTint { get => barkTint; set => barkTint = value; }
        public Color LeafTint { get => leafTint; set => leafTint = value; }

        public void Draw()
        {
            Console.WriteLine($"Tree: mesh: {mesh} ({position.X}:{position.Y}) height: {height} thickness: {thickness} bark: {bark} barkColor: {barkTint} leaves: {leaves} leafColor={leafTint}");
        }
    }

    #region Faker

    // dotnet add package Bogus

    public class TreeFaker : Faker<Tree>
    {
        public TreeFaker(Faker<Mesh> meshFaker, Faker<Texture> textureFaker, Faker<Vector> vectorFaker)
        {
            RuleFor(p => p.Mesh, f => meshFaker.Generate());
            RuleFor(p => p.Bark, f => textureFaker.Generate());
            RuleFor(p => p.Leaves, f => textureFaker.Generate());
            RuleFor(p => p.Position, f => vectorFaker.Generate());
            RuleFor(p => p.Height, f => f.Random.Double(0, 1000));
            RuleFor(p => p.Thickness, f => f.Random.Double(0, 10));
            RuleFor(p => p.BarkTint, f => new Color(f.Random.Byte(), f.Random.Byte(), f.Random.Byte()));
            RuleFor(p => p.LeafTint, f => new Color(f.Random.Byte(), f.Random.Byte(), f.Random.Byte()));
        }
    }

   


    #endregion

}
