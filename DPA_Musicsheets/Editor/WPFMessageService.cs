using Core.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Editor
{
    class WPFMessageService : IMessageService
    {
        public AskQuestionResult Ask(string question, string category)
        {
            var result = MessageBox.Show(question, category,
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.None:
                    return AskQuestionResult.Cancel;
                case MessageBoxResult.OK:
                    return AskQuestionResult.Yes;
                case MessageBoxResult.Cancel:
                    return AskQuestionResult.Cancel;
                case MessageBoxResult.Yes:
                    return AskQuestionResult.Yes;
                case MessageBoxResult.No:
                    return AskQuestionResult.No;
            }

            return AskQuestionResult.Cancel;
        }

        public void Error(string error)
        {
            throw new NotImplementedException();
        }

        public void Info(string info)
        {
            throw new NotImplementedException();
        }
    }
}
