using System;
using System.Collections.Generic;
using Eto.Forms;
using Grasshopper.Kernel;
using Rhino;
using Rhino.DocObjects.Tables;
using Rhino.Geometry;

namespace Corvid.Components
{
    public class GetAllUserText : GH_Component
    {
        // TODO:
        // - Add refresh button (like Elefront)

        /// <summary>
        /// Initializes a new instance of the GetAllUserText class.
        /// </summary>
        public GetAllUserText()
          : base("Get Document User Text", "DocUsrTxt",
              "Gets all the user text from the current Rhino document" +
                "\nand any section headings if present.",
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
            pManager.AddTextParameter("Keys", "K", "All Rhino document user text keys.", GH_ParamAccess.list);
            pManager.AddTextParameter("Values", "V", "All Rhino document user text values.", GH_ParamAccess.list);
            pManager.AddTextParameter("Sections", "S", "All Rhino document user text section headings.", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RhinoDoc document = RhinoDoc.ActiveDoc;
            StringTable userText = document.Strings;
            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            List<string> sections = new List<string>();

            for (int i = 0; i < userText.Count; i++)
            {
                string key = userText.GetKey(i);
                keys.Add(key);
                values.Add(userText.GetValue(key));
                if (key.Contains("\\"))
                {
                    string section = key.Split('\\')[0];
                    if (!sections.Contains(section))
                    {
                        sections.Add(section);
                    }
                }
            }

            DA.SetDataList(0, keys);
            DA.SetDataList(1, values);
            DA.SetDataList(2, sections);
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
                return Resources.GetAllUserText_24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("59E30A31-4D2E-4B9D-9D54-64CC7114F987"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}