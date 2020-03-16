using System;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using MediatR;
using Dyndle.Modules.Feedback.Contracts;
using Dyndle.Modules.Feedback.Notifications;
using Dyndle.Modules.Feedback.ViewModels;

namespace Dyndle.Modules.Feedback.Controllers
{
    /// <summary>
    /// Handles display and validation of the contact form
    /// </summary>
    public class ContactController : ModuleControllerBase
    {
        private readonly IContactFormEmailingService _emailService;
        private readonly IContactConfiguration _configuration;
        private readonly IMediator _notificationService;

        /// <summary>
        /// Injected dependencies
        /// </summary>
        /// <param name="contentProvider"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param> 
        /// <param name="emailService"></param> 
        public ContactController(IContentProvider contentProvider,
            ILogger logger,
            IContactConfiguration configuration,
            IContactFormEmailingService emailService,
            IMediator notificationService) : base(contentProvider, logger)
        {
            emailService.ThrowIfNull(nameof(emailService));
            configuration.ThrowIfNull(nameof(configuration));
            notificationService.ThrowIfNull(nameof(notificationService));

            _emailService = emailService;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Display contact form
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContactForm(IEntityModel entity)
        {
            ContactFormViewModel viewModel = new ContactFormViewModel();
            PrePopulateViewModel(viewModel, entity as IContactFormDataProvider);
            return View(entity.GetView(), viewModel);
        }

        /// <summary>
        /// Process posted contact form
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactForm(IEntityModel entity, ContactFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _emailService.SendFormData(viewModel.GetData());
                _notificationService.Publish(new ContactFormSentNotification());
                // ugly hack to be able to redirect from a child action
                HttpContext.Response.Redirect(_configuration.ThankYouUrl, true);
            }
            PrePopulateViewModel(viewModel, entity as IContactFormDataProvider);
            return View(viewModel);
        }


        /// <summary>
        /// Use data to populate view model
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="dataProvider"></param>
        private void PrePopulateViewModel(ContactFormViewModel viewModel, IContactFormDataProvider dataProvider)
        {
            dataProvider.ThrowIfNull(nameof(dataProvider));
            viewModel.Subjects = dataProvider.Subjects;
        }

    }
}