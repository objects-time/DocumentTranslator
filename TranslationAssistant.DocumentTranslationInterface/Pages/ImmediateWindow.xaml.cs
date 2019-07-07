﻿// // ----------------------------------------------------------------------
// // <copyright file="DocumentTranslationPage.xaml.cs" company="Microsoft Corporation">
// // Copyright (c) Microsoft Corporation.
// // All rights reserved.
// // THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// // KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// // IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// // PARTICULAR PURPOSE.
// // </copyright>
// // ----------------------------------------------------------------------
// // <summary>DocumentTranslationPage.xaml.cs</summary>
// // ----------------------------------------------------------------------

namespace TranslationAssistant.DocumentTranslationInterface.Pages
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for ProjectCodeCommentsTranslationPage.xaml
    /// </summary>
    public partial class ImmediateWindow : UserControl
    {
        #region Constructors and Destructors
        ViewModel.DocumentTranslation documentTranslation = new ViewModel.DocumentTranslation();


        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentTranslationPage" /> class.
        /// </summary>
        public ImmediateWindow()
        {
            this.InitializeComponent();
            this.KeyDown += ImmediateWindow_KeyDown;
        }

        private void ImmediateWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.F12:
                        break;
                }
            }
            catch (System.Exception ex) { };
        }

        #endregion

        private async void DetectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Task<string> task = TranslationServices.Core.TranslationServiceFacade.DetectAsync(InputBox.Text, true);
            ResultBox.Text = await task;
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string translateFrom = documentTranslation.SelectedSourceLanguage;
            if (documentTranslation.SourceLanguageList.IndexOf(documentTranslation.SelectedSourceLanguage) == 0) translateFrom = "";
            Task<string> task = TranslationServices.Core.TranslationServiceFacade.TranslateStringAsync(
                InputBox.Text,
                translateFrom,
                TranslationServices.Core.TranslationServiceFacade.LanguageNameToLanguageCode(documentTranslation.SelectedTargetLanguage),
                TranslationServices.Core.TranslationServiceFacade.CategoryID,
                documentTranslation.TranslateModeList.IndexOf(documentTranslation.SelectedTranslateMode)
            );
            ResultBox.Text = await task;
        }

        private async void BreakSentencesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string language = documentTranslation.SelectedSourceLanguage;
            if (documentTranslation.SourceLanguageList.IndexOf(language) == 0) language = string.Empty;
            Task<List<int>> task = TranslationServices.Core.TranslationServiceFacade.BreakSentencesAsync(InputBox.Text, language);
            StringBuilder outputstring = new StringBuilder(string.Empty);
            int startindex = 0;
            List<int> BreakResult = await task;
            foreach (int offset in BreakResult)
            {
                outputstring.Append(InputBox.Text.Substring(startindex, offset));
                outputstring.AppendLine('['+ offset.ToString() + ']');
                startindex += offset;
            }
            outputstring.Append("\n*** "+ Properties.Resources.Translate_SentNum + ' ' + task.Result.Count + " ***");
            ResultBox.Text = outputstring.ToString();
        }

        private void CbTargetLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            documentTranslation.SelectedTargetLanguage = e.AddedItems[0].ToString();
        }

        private void CbSourceLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            documentTranslation.SelectedSourceLanguage = e.AddedItems[0].ToString();
        }
    }
}