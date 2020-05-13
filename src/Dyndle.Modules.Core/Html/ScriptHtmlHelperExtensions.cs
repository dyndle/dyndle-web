using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Dyndle.Modules.Core.Html
{
    /// <summary>
    ///     Methods for helping to manage scripts in partials and templates.
    /// </summary>
    public static class ScriptHtmlHelperExtensions
    {
        /// <summary>
        ///     Adds a block of script to be rendered out at a later point in the page rendering when
        ///     <see cref="ScriptHtmlHelperExtensions.RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
        /// </summary>
        /// <param name="htmlHelper">the <see cref="HtmlHelper" /></param>
        /// <param name="scriptBlock">the block of script to render. The block must not include the &lt;script&gt; tags</param>
        /// <param name="renderOnAjax">
        ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
        /// </param>
        /// <remarks>
        ///     A call to <see cref="RenderScripts(HtmlHelper)" /> will render all scripts.
        /// </remarks>
        public static void AddScriptBlock(this HtmlHelper htmlHelper, string scriptBlock, bool renderOnAjax = false)
        {
            AddToScriptContext(htmlHelper, context => context.AddScriptBlock(scriptBlock, renderOnAjax));
        }

        /// <summary>
        ///     Adds a block of script to be rendered out at a later point in the page rendering when
        ///     <see cref="RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
        /// </summary>
        /// <param name="htmlHelper">the <see cref="HtmlHelper" /></param>
        /// <param name="scriptTemplate">
        ///     the template for the block of script to render. The template must include the &lt;script
        ///     &gt; tags
        /// </param>
        /// <param name="renderOnAjax">
        ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
        /// </param>
        /// <remarks>
        ///     A call to <see cref="RenderScripts(HtmlHelper)" /> will render all scripts.
        /// </remarks>
        public static void AddScriptBlock(this HtmlHelper htmlHelper, Func<HelperResult> scriptTemplate, bool renderOnAjax = false)
        {
            AddToScriptContext(htmlHelper, context => context.AddScriptBlock(scriptTemplate, renderOnAjax));
        }

        /// <summary>
        ///     Adds a script file to be rendered out at a later point in the page rendering when
        ///     <see cref="ScriptHtmlHelperExtensions.RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
        /// </summary>
        /// <param name="htmlHelper">the <see cref="HtmlHelper" /></param>
        /// <param name="path">the path to the script file to render later</param>
        /// <param name="renderOnAjax">
        ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
        /// </param>
        /// <remarks>
        ///     A call to <see cref="RenderScripts(HtmlHelper)" /> will render all scripts.
        /// </remarks>
        public static void AddScriptFile(this HtmlHelper htmlHelper, string path, bool renderOnAjax = false)
        {
            AddToScriptContext(htmlHelper, context => context.AddScriptFile(path, renderOnAjax));
        }

        /// <summary>
        ///     Begins a new <see cref="ScriptContext" />. Used to signal that the scripts inside the
        ///     context belong in the same view, partial view or template
        /// </summary>
        /// <param name="htmlHelper">
        ///     the <see cref="HtmlHelper" />
        /// </param>
        /// <returns>
        ///     a new instance of <see cref="ScriptContext" />
        /// </returns>
        public static ScriptContext BeginScriptContext(this HtmlHelper htmlHelper)
        {
            var context = htmlHelper.ViewContext.HttpContext;
            var scriptContext = new ScriptContext(context, htmlHelper.ViewContext.Writer);
            context.Items[ScriptContext.ScriptContextItem] = scriptContext;
            return scriptContext;
        }

        /// <summary>
        ///     Ends a <see cref="ScriptContext" />.
        /// </summary>
        /// <param name="htmlHelper">
        ///     the <see cref="HtmlHelper" />
        /// </param>
        public static void EndScriptContext(this HtmlHelper htmlHelper)
        {
            var context = htmlHelper.ViewContext.HttpContext;
            var scriptContext = context.Items[ScriptContext.ScriptContextItem] as ScriptContext;

            if (scriptContext != null)
            {
                scriptContext.Dispose();
            }
        }

        /// <summary>
        ///     Renders the scripts out into the view using <see cref="UrlHelper.Content" />
        ///     to generate the paths in the &lt;script&gt; elements for the script files
        /// </summary>
        /// <param name="htmlHelper">
        ///     the <see cref="HtmlHelper" />
        /// </param>
        /// <returns>
        ///     an <see cref="IHtmlString" /> of all of the scripts.
        /// </returns>
        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            var scriptContexts =
                htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItems] as Stack<ScriptContext>;

            if (scriptContexts != null)
            {
                var count = scriptContexts.Count;
                var builder = new StringBuilder(scriptContexts.Count);
                var script = new List<string>(scriptContexts.Count);

                for (int i = 0; i < count; i++)
                {
                    var scriptContext = scriptContexts.Pop();

                    builder.Append(ScriptContext.ScriptPathResolver(scriptContext._scriptFiles.ToArray()).ToString());
                    script.InsertRange(0, scriptContext._scriptBlocks);

                    // render out all the scripts in one block on the last loop iteration
                    if (i == count - 1 && script.Any())
                    {
                        foreach (var s in script)
                        {
                            builder.AppendLine(s);
                        }
                    }
                }

                return new HtmlString(builder.ToString());
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        ///     Performs an action on the current <see cref="ScriptContext" />
        /// </summary>
        /// <param name="htmlHelper">
        ///     the <see cref="HtmlHelper" />
        /// </param>
        /// <param name="action">the action to perform</param>
        private static void AddToScriptContext(HtmlHelper htmlHelper, Action<ScriptContext> action)
        {
            var scriptContext =
                htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItem] as ScriptContext;

            if (scriptContext == null)
            {
                throw new InvalidOperationException(
                    "No ScriptContext in HttpContext.Items. Call Html.BeginScriptContext() to create a ScriptContext.");
            }

            action(scriptContext);
        }
    }
}
