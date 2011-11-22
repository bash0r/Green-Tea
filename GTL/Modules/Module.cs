namespace GreenTea
{
    public sealed class Module
    {
        public Scope Root { get; private set; }
        public Scope Export { get; private set; }
        public Scope Static { get; private set; }

        public Module()
        {
            this.Root = new Scope(null, this);
            this.Export = new Scope(null, this);
            this.Static = new Scope(null, this);
        }
    }
}
