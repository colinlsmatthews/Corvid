using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using System.Linq;

namespace Raven.Components
{
    public class UserTxtSectionGet : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_UserTextSectionGet class.
        /// </summary>
        public UserTxtSectionGet()
          : base("Get User Text Value by Section", "UsrTxtSec",
              "Get the value for a user text key/value pair by" +
                "\nsection name, entry name, or both. " +
                "\nUser text key/value pairs can be grouped into sections " +
                "\nwith the format \"<section>\\<entry>:<value>\".\n" +
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
            pManager.AddTextParameter("Section", "S", "The section name", GH_ParamAccess.item);
            pManager.AddTextParameter("Entry", "E", "The entry name", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Keys", "K", "The keys for the section", GH_ParamAccess.list);
            pManager.AddTextParameter("Entries", "E", "The entries for the section", GH_ParamAccess.list);
            pManager.AddTextParameter("Values", "V", "The values for the section", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userData = document.Strings;
            string section = string.Empty;
            List<string> entries = new List<string>();
            List<string> keys = new List<string>();
            List<string> entriesOut = new List<string>();
            List<string> values = new List<string>();
            
            DA.GetData(0, ref section);
            DA.GetDataList(1, entries);

            // When user provides a section name but no entries
            if (!string.IsNullOrEmpty(section) & entries.Count == 0)
            {
                var entriesArray = userData.GetEntryNames(section).ToArray();
                for (int i = 0; i < entriesArray.Length; i++)
                {
                    keys.Add(section + "\\" + entriesArray[i]);
                    entriesOut.Add(entriesArray[i]);
                    values.Add(userData.GetValue(section, entriesArray[i]));
                }
            }
            // When user provides both a section name and entries
            else if (!string.IsNullOrEmpty(section) & entries.Count > 0)
            {
                var entriesArray = entries.ToArray();
                var sectionEntries = userData.GetEntryNames(section).ToArray();
                foreach (string entry in entriesArray)
                {
                    if (sectionEntries.Contains(entry))
                    {
                        keys.Add(section + "\\" + entry);
                        entriesOut.Add(entry);
                        values.Add(userData.GetValue(section, entry));
                    }
                }
            }
            // When user provides no section name and provides entries
            else if (string.IsNullOrEmpty(section) & entries.Count > 0)
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
                            keys.Add(sec + "\\" + entry);
                            entriesOut.Add(entry);
                            values.Add(userData.GetValue(sec, entry));
                        }
                    }
                }
            }
            else
            {
                return;
            }

            DA.SetDataList(0, keys);
            DA.SetDataList(1, entriesOut);
            DA.SetDataList(2, values);
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