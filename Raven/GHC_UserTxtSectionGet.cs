using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using System.Linq;

namespace Raven
{
    public class GHC_UserTxtSectionGet : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_UserTextSectionGet class.
        /// </summary>
        public GHC_UserTxtSectionGet()
          : base("Get User Text Value by Section", "TxtSec",
              "Get the value for a user text key/value pair by section name. " +
                "\nUser text key/value pairs can be grouped into sections " +
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
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Keys", "K", "The keys for the section entries", GH_ParamAccess.list);
            pManager.AddTextParameter("Values", "V", "The values for the section entries", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;
            string section = String.Empty;
            List<string> entries = new List<string>();
            List<string> values = new List<string>();
            List<string> keys = new List<string>();

            if (!DA.GetData(0, ref section)) return;
            if (!DA.GetDataList<string>(1, entries))
            {
                var entriesArray = userData.GetEntryNames(section).ToArray();
                for (int i = 0; i < entriesArray.Length; i++)
                {
                    keys.Add(entriesArray[i]);
                    values.Add(userData.GetValue(section, entries[i]));
                }
            }
            else
            {
                var entriesArray = entries.ToArray();
                foreach(string entry in entriesArray)
                {
                    keys.Add(entry);
                    values.Add(userData.GetValue(section, entry));
                }
            }

            DA.SetDataList(0, keys);
            DA.SetDataList(1, values);
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
            get { return new Guid("98157918-5709-4C8F-8C99-14DD44A5FE59"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}