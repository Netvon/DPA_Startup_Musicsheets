using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Editor
{
    public enum AskQuestionResult
    {
        Yes, No, Cancel
    }

    public interface IMessageService
    {
        AskQuestionResult Ask(string question, string category);
        void Info(string info);
        void Error(string error);
    }
}
