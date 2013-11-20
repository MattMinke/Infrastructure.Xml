using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class Structure : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteTransformer();
        }

        private void ExecuteTransformer()
        {
            Minx.Xslt.TransformerSettings settings = new Minx.Xslt.TransformerSettings()
            {
                EnableDebug = true,
                EnableDocumentFunction = true
            };

            Minx.Xslt.Transformer transformer = new Minx.Xslt.Transformer(settings);

            transformer.Load(new System.IO.FileInfo(this.Context.Server.MapPath("~/App_Data/Stylesheets/structure.xslt")));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            Minx.Xslt.TransformArguments arguments = new Minx.Xslt.TransformArguments();

            arguments.FunctionProviders.Add(new WebApplication.ExtensionFunctions.UIExtensions());

            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml(@"<root></root>");

            transformer.Transform(document, sb, arguments);

            this.LitTransformationResult.Text = sb.ToString();

        }
    }
}