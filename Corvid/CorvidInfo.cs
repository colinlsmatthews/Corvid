using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace Corvid
{
    public class CorvidInfo : GH_AssemblyInfo
    {
        public override string Name => "Corvid";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "A plugin for record-keeping and getting information about Rhino";

        public override Guid Id => new Guid("033a8c00-7ab7-43ce-9665-e7d1874b6f85");

        //Return a string identifying you or your company.
        public override string AuthorName => "Colin L.S. Matthews";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "support@colinlsmatthews.com";

        //Return a string representing the version.  This returns the same version as the assembly.
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
    }
}