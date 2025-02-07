using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Raven.Components
{
    public class GetForumAvatar : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetForumAvatar class.
        /// </summary>
        public GetForumAvatar()
          : base("Forum Avatar", "Avatar",
              "Get any username avatar bitmap from the McNeel Discourse forum",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Username", "U", "The username of the forum member", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Size", "S",
                "The size of the avatar image:" +
                "\n0 - S (72x72)" +
                "\n1 - M (144x144)" +
                "\n2 - L (288x288)",
                GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Avatar", "A", "The avatar bitmap", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int sizeChoice = 3;
            string username = string.Empty;

            if (!DA.GetData(0, ref username)) return;
            DA.GetData(1, ref sizeChoice);

            switch (sizeChoice)
            {
                case 0:
                    sizeChoice = 72;
                    break;
                case 1:
                    sizeChoice = 144;
                    break;
                case 2:
                    sizeChoice = 288;
                    break;
                default:
                    sizeChoice = 144;
                    break;
            }

            string url = $"https://discourse.mcneel.com/u/{username}.json";


            // Get JSON data from forum user page
            string jsonText = string.Empty;
            Task<string> jsonTask;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    jsonTask = client.GetStringAsync(url);
                    jsonText = jsonTask.Result;
                }
                catch (Exception e)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                        $"Error downloading JSON data for user \"{username}\": \n{e.Message}");
                    return;
                }
            }

            // Parse JSON data
            JObject jsonRoot;
            try
            {
                jsonRoot = JObject.Parse(jsonText);
            }
            catch (Exception e)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                    $"Error parsing JSON data for user \"{username}\": \n{e.Message}");
                return;
            }

            string avatarTemplate = (string)jsonRoot["user"]?["avatar_template"];
            if (string.IsNullOrEmpty(avatarTemplate))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                    $"Error parsing JSON data for user \"{username}\": \nNo avatar template found");
                return;
            }

            // Make sure McNeel forum URL is correct
            if (avatarTemplate.StartsWith("/"))
            {
                avatarTemplate = "https://discourse.mcneel.com" + avatarTemplate;
            }

            // Set size for output bitmap
            string avatarUrl = avatarTemplate.Replace("{size}", sizeChoice.ToString());

            // Download avatar image data
            byte[] avatarBytes;
            Task<byte[]> avatarTask;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    avatarTask = client.GetByteArrayAsync(avatarUrl);
                    avatarBytes = avatarTask.Result;
                }
                catch (Exception e)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                        $"Error downloading avatar image for user \"{username}\": \n{e.Message}");
                    return;
                }
            }

            // Convert image data to bitmap
            try
            {
                using (MemoryStream stream = new MemoryStream(avatarBytes))
                {
                    Bitmap avatarBitmap = new Bitmap(stream);
                    DA.SetData(0, avatarBitmap);
                }
            }
            catch (Exception e)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                    $"Error converting image bytes to bitmap: \n{e.Message}");
                return;
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.Avatar_24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("F0F09B8D-40B4-4E03-B037-284E25804F0C"); }
        }
    }
}