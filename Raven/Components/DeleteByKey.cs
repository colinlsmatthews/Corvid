using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.DocObjects.Tables;
using Rhino.Geometry;

namespace Raven.Components
{
    public class DeleteByKey : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeleteByKey class.
        /// </summary>
        public DeleteByKey()
          : base("Delete By Key", "DelKey",
              "Delete a user text item by specified key",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Key", "K", "The key of the user text item to delete", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Delete", "D", "Delete the user text item", GH_ParamAccess.item, false);
            pManager[1].Optional = true;
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
            string key = string.Empty;
            bool delete = false;
            if (!DA.GetData(0, ref key)) return;
            DA.GetData(1, ref delete);

            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;

            if (delete)
            {
                userData.Delete(key);
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
                return Resources.DeleteByKey_24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("504A9850-0C89-4D2D-893C-626E5A94614D"); }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }
    }
}