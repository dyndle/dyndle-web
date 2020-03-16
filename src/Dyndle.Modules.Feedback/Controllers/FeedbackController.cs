using System;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Feedback.Contracts;
using Dyndle.Modules.Feedback.Models;
using Dyndle.Modules.Feedback.ViewModels;

namespace Dyndle.Modules.Feedback.Controllers
{
    /// <summary>
    /// Handles display and validation of the feedback form
    /// </summary>
    public class FeedbackController : ModuleControllerBase
    {
        private readonly IFeedbackFormDataProvider _formDataProvider;
        private readonly IFeedbackFormSenderService _senderService;
        private readonly IFeedbackConfiguration _configuration;

        /// <summary>
        /// Injected dependencies
        /// </summary>
        /// <param name="contentProvider"></param>
        /// <param name="formDataProvider"></param>
        /// <param name="senderService"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public FeedbackController(IContentProvider contentProvider,
            IFeedbackFormDataProvider formDataProvider,
            IFeedbackFormSenderService senderService,
            ILogger logger,
            IFeedbackConfiguration configuration) : base(contentProvider, logger)
        {
            formDataProvider.ThrowIfNull(nameof(formDataProvider));
            senderService.ThrowIfNull(nameof(senderService));
            configuration.ThrowIfNull(nameof(configuration));

            _formDataProvider = formDataProvider;
            _senderService = senderService;
            _configuration = configuration;
        }

        /// <summary>
        /// Fetch data from service and populate option lists
        /// </summary>
        /// <param name="viewModel"></param>
        private void PrePopulateViewModel(FeedbackFormViewModel viewModel)
        {
            viewModel.Titles = _formDataProvider.Titles;
            viewModel.Countries = _formDataProvider.Countries;
            viewModel.AgeRanges = _formDataProvider.AgeRanges;
            viewModel.Products = _formDataProvider.Products(_configuration.SiteId);
            viewModel.Reasons = _formDataProvider.Reasons;
        }

        /// <summary>
        /// Display feedback form
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FeedbackForm(IEntityModel entity)
        {
            FeedbackFormViewModel viewModel = new FeedbackFormViewModel();
            PrePopulateViewModel(viewModel);
            return View(entity.GetView(), viewModel);
        }

        /// <summary>
        /// Process posted feedback form
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeedbackForm(FeedbackFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SendResult result = _senderService.SendData(_configuration.SiteId, viewModel.GetData());
                if (result.Success)
                {
                    // ugly hack to be able to redirect from a child action
                   HttpContext.Response.Redirect(_configuration.ThankYouUrl, true);
                }
                Logger.Error($"Error sending form data: {result.Message}");
            }
            PrePopulateViewModel(viewModel);
            return View(viewModel);
        }
    }
}