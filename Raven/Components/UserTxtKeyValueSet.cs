using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;

namespace Raven.Components
{
    public class UserTxtKeyValueSet : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_SetUserTxtSection class.
        /// </summary>
        public UserTxtKeyValueSet()
          : base("Set User Text By Key/Value", "UsrTxtKV",
              "Set document user text by key/value pairs.",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Key", "K", "The key to set", GH_ParamAccess.list);
            pManager.AddTextParameter("Value", "V", "The value to set", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Set", "S", "Send to Rhino document", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            bool set = false;
            if (!DA.GetDataList(0, keys)) return;
            if (!DA.GetDataList(1, values)) return;

            if (keys.Count != values.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Key and Value lists must be the same length.");
                return;
            }

            DA.GetData(2, ref set);
            if (!set) return;

            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;

            if (set)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    var value = values[i];
                    userData.SetString(key, value);
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
            get { return new Guid("9DA54B1B-2389-4CD6-8895-E389DE8531E4"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}