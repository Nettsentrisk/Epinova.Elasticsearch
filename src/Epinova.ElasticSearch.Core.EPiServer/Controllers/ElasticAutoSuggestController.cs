﻿using System;
using System.Linq;
using System.Web.Mvc;
using Epinova.ElasticSearch.Core.EPiServer.Contracts;
using Epinova.ElasticSearch.Core.EPiServer.Controllers.Abstractions;
using Epinova.ElasticSearch.Core.EPiServer.Models.ViewModels;
using EPiServer;
using EPiServer.DataAbstraction;

namespace Epinova.ElasticSearch.Core.EPiServer.Controllers
{
    public class ElasticAutoSuggestController : ElasticSearchControllerBase
    {
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly IAutoSuggestRepository _autoSuggestRepository;

        public ElasticAutoSuggestController(IContentLoader contentLoader, ILanguageBranchRepository languageBranchRepository, IAutoSuggestRepository autoSuggestRepository)
        {
            _languageBranchRepository = languageBranchRepository;
            _autoSuggestRepository = autoSuggestRepository;
        }


        [Authorize(Roles = "ElasticsearchAdmins")]
        public ActionResult Index()
        {
            var languages = _languageBranchRepository.ListEnabled()
                .Select(lang => new {lang.LanguageID, lang.Name})
                .ToArray();

            AutoSuggestViewModel model = new AutoSuggestViewModel(CurrentLanguage);

            foreach (var language in languages)
            {
                var name = language.Name;
                name = String.Concat(name.Substring(0, 1).ToUpper(), name.Substring(1));

                model.WordsByLanguage.Add(new LanguageAutoSuggestWords
                {
                    LanguageName = name,
                    LanguageId = language.LanguageID,
                    Words = _autoSuggestRepository.GetWords(language.LanguageID)
                });
            }

            return View("~/Views/ElasticSearchAdmin/AutoSuggest/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddWord(string languageId, string word)
        {
            if(!String.IsNullOrWhiteSpace(word))
                _autoSuggestRepository.AddWord(languageId, word.Replace("|", String.Empty));

            CurrentLanguage = languageId;
            return RedirectToAction("Index");
        }

        public ActionResult DeleteWord(string languageId, string word)
        {
            if (!String.IsNullOrWhiteSpace(word))
                _autoSuggestRepository.DeleteWord(languageId, word);

            CurrentLanguage = languageId;
            return RedirectToAction("Index");
        }
    }
}