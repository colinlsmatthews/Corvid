using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;

namespace Raven.Components
{
    public class UserTxtKeyValueGet : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the UserTxtKeyValueGet class.
        /// </summary>
        public UserTxtKeyValueGet()
          : base("Get User Text Value by Key", "UsrTxtKV",
              "Get the value for a user text key/value pair.\n" +
                "\nIf you would like to keep this component synced" +
                "\nwith the Rhino document, use a trigger.",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Key", "K", "The key to get", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Value", "V", "The value for the key", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string key = string.Empty;

            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;

            if (!DA.GetData(0, ref key)) return;

            string value = userData.GetValue(key);
            DA.SetData(0, value);
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
                return Resources.UserTxtKeyValueGet_24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("CA3C7872-AAD8-4D1B-B7DC-9ABC22DF8A8D"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}