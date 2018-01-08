using System.Text.RegularExpressions;

namespace Core.Editor
{
    public class KeyBind
    {
        string[] keys;
        int matches;
        int matchesRequired;
        string pattern;

        public KeyBind(string pattern)
        {
            this.pattern = pattern;

            keys = pattern.ToString().Split(' ');
            matchesRequired = keys.Length;
        }

        public bool IsMatch => matches == matchesRequired;
        public bool HasPartialMatch => matches != 0 && matches != matchesRequired;

        public bool Match(string keyInput)
        {
            var key = keys[matches];

            if(Regex.Match(keyInput, key).Success)
            {
                matches++;

                if(matches == matchesRequired)
                {
                    Reset();
                    return true;
                }
            }
            else if( matches > 0)
            {
                Reset();
            }

            return false;
        }

        public void Reset()
        {
            matches = 0;
        }
    }
}
