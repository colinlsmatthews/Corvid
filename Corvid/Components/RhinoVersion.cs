using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Corvid.Components
{
    public class RhinoVersion : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RhinoVersion class.
        /// </summary>
        public RhinoVersion()
          : base("Rhino Version", "Version",
            "Gets the version of the current Rhino installation.",
            "Rhino", "Corvid")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Rhino Version", "V", "The version of the current Rhino installation.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Major Version", "M", "The major version of the current Rhino installation.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Minor Version", "m", "The minor version of the current Rhino installation.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Build", "B", "The build number of the current Rhino installation.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Revision", "R", "The revision number of the current Rhino installation.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Build Date", "D", "The build date of the current Rhino installation.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, Rhino.RhinoApp.Version.ToString());
            DA.SetData(1, Rhino.RhinoApp.Version.Major);
            DA.SetData(2, Rhino.RhinoApp.Version.Minor);
            DA.SetData(3, Rhino.RhinoApp.Version.Build);
            DA.SetData(4, Rhino.RhinoApp.Version.Revision);
            DA.SetData(5, Rhino.RhinoApp.BuildDate);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override Bitmap Icon => Resources.RhinoVersion_24;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("290721f9-b0e0-409c-9226-9aca12731c8e");
    }
}