using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;

namespace Raven
{
    public class GHC_SetUserTxtKeyValue : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_SetUserTxtSection class.
        /// </summary>
        public GHC_SetUserTxtKeyValue()
          : base("Set User Text By Key/Value", "UserTxtKV",
              "Set document user text by key/value pairs.",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Key", "K", "The key to set", GH_ParamAccess.item);
            pManager.AddTextParameter("Value", "V", "The value to set", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Set", "S", "Send to Rhino document", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string key = String.Empty;
            string value = String.Empty;
            bool set = false;
            DA.GetData(0, ref key);
            DA.GetData(1, ref value);
            DA.GetData(2, ref set);

            if (!set) return;
            if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Missing one or more required inputs");
                return;
            }

            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;

            if (set)
            {
                userData.SetString(key, value);
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9DA54B1B-2389-4CD6-8895-E389DE8531E4"); }
        }
    }
}