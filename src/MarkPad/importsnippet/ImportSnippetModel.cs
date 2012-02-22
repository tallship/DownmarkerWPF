using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using MarkPad.Framework;
using MarkPad.Metaweblog;
using MarkPad.Services.Interfaces;
using MarkPad.Settings;

namespace MarkPad.ImportSnippet
{
    public class ImportSnippetModel : Screen
    {

        public string tfsServerUri { get; set; }

        public ImportSnippetModel(string tfsServerUri)
        {
            this.tfsServerUri = tfsServerUri;
        }
        public void saveTFSSettings()
        {
            TryClose();
        }
    }
}
