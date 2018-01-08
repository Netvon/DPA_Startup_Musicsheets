using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor
{
    public static class Commands
    {
        public const string InsertTimeCommandName = "insert_time";
        public const string SaveFileCommandName = "save_file";
        public const string OpenFileCommandName = "open_file";
        public const string InsertClefTrebleCommandNames = "inser_clef_treble";
        public const string InsertTempoCommandName = "insert_tempo";
        public const string InsertTime44CommandName = "insert_time_44";
        public const string InsertTime34CommandName = "insert_time_34";
        public const string InsertTime68CommandName = "insert_time_68";

        static public Core.Editor.Commands Factory
        {
            get
            {
                var temp = new Core.Editor.Commands(Assembly.GetAssembly(typeof(Commands)));

                temp.AddBinding(InsertTempoCommandName, "(LeftAlt|RightAlt) S");
                temp.AddBinding(InsertClefTrebleCommandNames, "(LeftAlt|RightAlt) C");
                temp.AddBinding(InsertTime44CommandName, "(LeftAlt|RightAlt) T");
                temp.AddBinding(InsertTime44CommandName, "(LeftAlt|RightAlt) T 4");
                temp.AddBinding(InsertTime34CommandName, "(LeftAlt|RightAlt) T 3");
                temp.AddBinding(InsertTime68CommandName, "(LeftAlt|RightAlt) T 6");

                return temp;
            }
        }
    }
}
