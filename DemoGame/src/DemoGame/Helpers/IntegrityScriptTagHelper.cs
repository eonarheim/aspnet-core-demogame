using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.WebUtilities;

namespace DemoGame.Helpers
{

    /// <summary>
    /// Example tag helper that adds Subresource Integrity (SRI) support
    /// </summary>
    /// <remarks>
    /// To be truly useful, it should take into account fallback local href to calculate the hash for CDN URLs
    /// </remarks>
    [HtmlTargetElement("script", Attributes = AppendIntegrityAttributeName)]
    public class IntegrityScriptTagHelper : ScriptTagHelper
    {
        private const string AppendIntegrityAttributeName = "asp-append-integrity";
        private const string IntegrityAttributeName = "integrity";
        private const string IntegrityHashName = "sha256";
        private const string CrossOriginAttributeName = "crossorigin";
        private const string CrossOriginAnonymousName = "anonymous";
        private const string SrcAttributeName = "src";
        private const string QueryFragment = "?";
        private const string VersionKey = "v";

        private FileVersionProvider _fileVersionProvider;

        public IntegrityScriptTagHelper(IHostingEnvironment hostingEnvironment, IMemoryCache cache, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, IUrlHelperFactory urlHelperFactory) : base(hostingEnvironment, cache, htmlEncoder, javaScriptEncoder, urlHelperFactory)
        {
        }

        public override int Order
        {
            get
            {
                return base.Order + 1;
            }
        }

        /// <summary>
        /// Value indicating if subresource integrity attribute should be appended to the tag.
        /// </summary>
        /// <remarks>
        /// If <c>true</c> then an attribute "integrity" will be added containing the SHA256 hash of the file contents
        /// </remarks>
        [HtmlAttributeName(AppendIntegrityAttributeName)]
        public bool? AppendIntegrity { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AppendIntegrity == true)
            {
                EnsureFileVersionProvider();

                // Pass through attribute that is also a well-known HTML attribute.
                if (Src != null)
                {
                    output.CopyHtmlAttribute(SrcAttributeName, context);
                }

                // process
                ProcessUrlAttribute(SrcAttributeName, output);

                // get the tag helper version of the Href
                Src = output.Attributes[SrcAttributeName]?.Value as string;

                if (Src != null)
                {
                    // get the pre-versioned path or generate it ourselves
                    var versionedPath = AppendVersion == true ? Src : _fileVersionProvider.AddFileVersionToPath(Src);
                    
                    int queryIndex;
                    if ((queryIndex = versionedPath.IndexOf(QueryFragment)) > -1)
                    {
                        var query = QueryHelpers.ParseQuery(versionedPath.Substring(queryIndex));

                        if (query != null && query.ContainsKey(VersionKey))
                        {
                            var version = query[VersionKey];
                            
                            output.Attributes.Add(IntegrityAttributeName, $"{IntegrityHashName}-{version}");
                            output.Attributes.Add(CrossOriginAttributeName, CrossOriginAnonymousName);
                        }
                    }
                }
            }
        }

        private void EnsureFileVersionProvider()
        {
            if (_fileVersionProvider == null)
            {
                _fileVersionProvider = new FileVersionProvider(
                    HostingEnvironment.WebRootFileProvider,
                    Cache,
                    ViewContext.HttpContext.Request.PathBase);
            }
        }
    }
}
