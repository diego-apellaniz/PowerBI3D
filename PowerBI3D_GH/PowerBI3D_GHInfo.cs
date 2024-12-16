using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace PowerBI3D_GH
{
    public class PowerBI3D_GHInfo : GH_AssemblyInfo
    {
        public override string Name => "PowerBI3D";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => Properties.Resources.logo_powerbi3d_small;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "This plug-in helps you load your 3D model into Power BI.";

        public override Guid Id => new Guid("7a1d3d4f-f982-4f76-9bb1-a54701940289");

        //Return a string identifying you or your company.
        public override string AuthorName => "Diego Apellániz";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "diego.apellaniz@kevee.com";
    }
}