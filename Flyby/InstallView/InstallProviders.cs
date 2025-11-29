using System.Windows.Forms;

namespace Flyoobe
{
    /// <summary>
    /// Shared "last selection" holder reused by providers (ISO/exe).
    /// </summary>
    public sealed class LastSelections
    {
        public string LastIsoPath { get; set; }
        public string LastExePath { get; set; }
    }

    /// <summary>
    /// Contract for install sources like Rufus, MCT, Ventoy, ...
    /// </summary>
    public interface IInstallProvider
    {
        string Id { get; }
        string DisplayName { get; }
        string HomepageUrl { get; }           // may be null
        string DirectDownloadUrl { get; }     // may be null
        string[] ExactExeNames { get; }       // e.g. "rufus.exe"
        string[] WildcardExePatterns { get; } // e.g. "rufus*.exe"
        bool TypicallyNeedsIso { get; }       // hint for UI
        bool IsExternalTool { get; }         // if true > resolve/browse EXE; if false > launch by command name (no file picker)

        /// <summary>
        /// Short hint shown in the UI.
        /// </summary>
        string Hint { get; }

        /// <summary>
        /// Show provider-specific options dialog (if any) and build CLI args.
        /// Return null if user cancelled.
        /// </summary>
        string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last);
    }
}
