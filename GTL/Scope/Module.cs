﻿using System.Collections.Generic;
namespace GreenTea
{
    public sealed class Module
    {
        public string Name { get; private set; }
        public IEnumerable<string> Includes { get; private set; }

        public Scope Root { get; private set; }
        public IExpression Main { get; private set; }

        public Module(string name, IEnumerable<string> incs, IExpression main)
        {
            this.Root = new Scope(true);

            this.Name = name;
            this.Includes = incs;
            this.Main = main;
            Functions.AddTo(Root);
        }
    }
}
