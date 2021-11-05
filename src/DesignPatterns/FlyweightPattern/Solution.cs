using Bogus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlyweightPattern.Solution
{
    public class TreeFactory
    {
        public static ICollection<TreeConcret> Create(int numberOfTrees)
        {
            TreeModelFaker treeModelFaker = new TreeModelFaker(new MeshFaker(), new TextureFaker(), new VectorFaker());

            // Szablony drzew
            IEnumerable<TreeModel> treeModels = treeModelFaker.Generate(100);

            // Konkretne wystąpienia drzew
            TreeConcretFaker treeConcretFaker = new TreeConcretFaker(treeModels, new VectorFaker());

            return treeConcretFaker.Generate(numberOfTrees);
        }
    }


    public class Game
    {
        private ICollection<TreeConcret> trees { get; set; }

        public Game(ICollection<TreeConcret> trees)
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


    public class TreeModel
    {
        private Mesh mesh;
        private Texture bark;
        private Texture leaves;

        public TreeModel()
        {

        }

        public TreeModel(Mesh mesh, Texture bark, Texture leaves)
            :this()
        {
            this.Mesh = mesh;
            this.Bark = bark;
            this.Leaves = leaves;
        }

        public Mesh Mesh { get => mesh; set => mesh = value; }
        public Texture Bark { get => bark; set => bark = value; }
        public Texture Leaves { get => leaves; set => leaves = value; }

        public void Draw()
        {
            Console.WriteLine($"Tree: mesh: {Mesh} bark: {Bark} leaves: {Leaves}");
        }
    }

    public class TreeConcret
    {
        public TreeModel TreeModel { get; set; }
        public Vector Position { get => position; set => position = value; }
        public double Height { get => height; set => height = value; }
        public double Thickness { get => thickness; set => thickness = value; }
        public Color BarkTint { get => barkTint; set => barkTint = value; }
        public Color LeafTint { get => leafTint; set => leafTint = value; }

        private Vector position;
        private double height;
        private double thickness;
        private Color barkTint;
        private Color leafTint;

        public TreeConcret()
        {

        }

        public TreeConcret(TreeModel treeModel, Vector position, double height, double thickness, Color barkTint, Color leafTint)
            : this()
        {
            TreeModel = treeModel;
            this.Position = position;
            this.Height = height;
            this.Thickness = thickness;
            this.BarkTint = barkTint;
            this.LeafTint = leafTint;
        }

        public void Draw()
        {
            TreeModel.Draw();
            Console.WriteLine($"({Position.X}:{Position.Y}) leafColor={LeafTint}");
        }

    }

    #region Faker

    // dotnet add package Bogus

    public class TreeModelFaker : Faker<TreeModel>
    {
        public TreeModelFaker(Faker<Mesh> meshFaker, Faker<Texture> textureFaker, Faker<Vector> vectorFaker)
        {
            RuleFor(p => p.Mesh, f => meshFaker.Generate());
            RuleFor(p => p.Bark, f => textureFaker.Generate());
            RuleFor(p => p.Leaves, f => textureFaker.Generate());
           
        }
    }

    public class TreeConcretFaker : Faker<TreeConcret>
    {
        public TreeConcretFaker(IEnumerable<TreeModel> treeModels, Faker<Vector> vectorFaker)
        {
            RuleFor(p => p.TreeModel, f => f.PickRandom(treeModels));
            RuleFor(p => p.Position, f => vectorFaker.Generate());
            RuleFor(p => p.Height, f => f.Random.Double(0, 1000));
            RuleFor(p => p.Thickness, f => f.Random.Double(0, 10));
            RuleFor(p => p.BarkTint, f => new Color(f.Random.Byte(), f.Random.Byte(), f.Random.Byte()));
            RuleFor(p => p.LeafTint, f => new Color(f.Random.Byte(), f.Random.Byte(), f.Random.Byte()));
        }
    }




    #endregion
}
