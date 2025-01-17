using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;

namespace Raven
{
    public class GHC_UserTxtSectionSet : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_SetUserTxtSection class.
        /// </summary>
        public GHC_UserTxtSectionSet()
          : base("Set User Text By Section", "TxtSec",
              "Set document user text by section, entry, and value." +
                "\nUser text key/value pairs can be grouped into sections" +
                "\nwith the format \"<section>\\<entry>:<value>\".",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Section", "S", "The section name", GH_ParamAccess.item);
            pManager.AddTextParameter("Entry", "E", "The entry name", GH_ParamAccess.list);
            pManager.AddTextParameter("Value", "V", "The value to set", GH_ParamAccess.list);
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
            string section = String.Empty;
            List<string> entries = new List<string>();
            List<string> values = new List<string>();
            bool set = false;
            if (!DA.GetData(0, ref section)) return;
            if (!DA.GetDataList<string>(1, entries)) return;
            if (!DA.GetDataList<string>(2, values)) return;
            if (entries.Count != values.Count)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Entry and Value lists must be the same length.");
                return;
            }
            DA.GetData(3, ref set);

            if (!set) return;

            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;

            if (set)
            {
                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    var value = values[i];
                    userData.SetString(section, entry, value);
                }
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
            get { return new Guid("1BB0602C-3AAE-422E-BF87-389E8D0A6B53"); }
        }
    }
}