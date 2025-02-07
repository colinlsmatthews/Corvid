using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using System.Linq;
using Eto.Forms;

namespace Raven.Components
{
    public class DeleteBySectionEntry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeleteBySectionEntry class.
        /// </summary>
        public DeleteBySectionEntry()
          : base("Delete By Section/Entry", "DelSec",
              "Delete a user text item by specifying its section and/or entry",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Section", "S", "The sections to operate on; if empty, then \"Entry\" will delete across all sections", GH_ParamAccess.item);
            pManager.AddTextParameter("Entry", "E", "The section entries to delete; if empty, then all section entries will be deleted", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Delete", "D", "Delete the user text item", GH_ParamAccess.item, false);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
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
            string section = string.Empty;
            List<string> entries = new List<string>();
            bool delete = false;

            DA.GetData(0, ref section);
            DA.GetDataList(1, entries);   
            DA.GetData(2, ref delete);

            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;

            if (delete)
            {
                if (string.IsNullOrEmpty(section) & entries.Count == 0)
                {
                    return;
                }
                else if (string.IsNullOrEmpty(section) & entries.Count != 0)
                {
                    var sectionsArray = userData.GetSectionNames().ToArray();
                    foreach (string sec in sectionsArray)
                    {
                        var entriesArray = entries.ToArray();
                        var sectionEntries = userData.GetEntryNames(sec).ToArray();
                        foreach (string entry in entriesArray)
                        {
                            if (sectionEntries.Contains(entry))
                            {
                                userData.Delete(sec, entry);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(section) & entries.Count != 0)
                {
                    var entriesArray = entries.ToArray();
                    var sectionEntries = userData.GetEntryNames(section).ToArray();
                    foreach (string entry in entriesArray)
                    {
                        if (sectionEntries.Contains(entry))
                        {
                            userData.Delete(section, entry);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(section) & entries.Count == 0)
                {
                    userData.Delete(section, null);
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
            get { return new Guid("B650EE18-9877-4586-8010-46A06ADFD719"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }
    }
}