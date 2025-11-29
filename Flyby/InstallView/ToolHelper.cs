using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flyoobe
{
    internal static class ToolHelpers
    {
        /// <summary>Try last path, app base, CWD, then PATH (wildcards allowed).</summary>
        public static string ResolveToolPath(string lastPath, string[] exactFileNames, string[] patterns)
        {
            try
            {
                if (!string.IsNullOrEmpty(lastPath) && File.Exists(lastPath)) return lastPath;

                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var cwd = Environment.CurrentDirectory;

                // exact files in base/cwd
                if (exactFileNames != null)
                {
                    foreach (var f in exactFileNames)
                    {
                        var p1 = Path.Combine(baseDir, f); if (File.Exists(p1)) return p1;
                        var p2 = Path.Combine(cwd, f); if (File.Exists(p2)) return p2;
                    }
                }

                // wildcards in base/cwd
                if (patterns != null)
                {
                    foreach (var pat in patterns)
                    {
                        var m1 = SafeGetFiles(baseDir, pat); if (m1?.Length > 0) return m1[0];
                        var m2 = SafeGetFiles(cwd, pat); if (m2?.Length > 0) return m2[0];
                    }
                }

                // PATH dirs (wildcards)
                var env = Environment.GetEnvironmentVariable("PATH");
                if (!string.IsNullOrEmpty(env) && patterns != null)
                {
                    foreach (var dir in env.Split(Path.PathSeparator))
                    {
                        foreach (var pat in patterns)
                        {
                            var m = SafeGetFiles(dir, pat); if (m?.Length > 0) return m[0];
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private static string[] SafeGetFiles(string dir, string pattern)
        {
            try { return Directory.GetFiles(dir, pattern); } catch { return null; }
        }

        public static bool Confirm(IWin32Window owner, string msg)
            => MessageBox.Show(owner, msg, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;

        public static bool Run(string file, string args, bool asAdmin)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = file,
                    Arguments = args ?? "",
                    UseShellExecute = true
                };
                if (asAdmin) psi.Verb = "runas";
                Process.Start(psi);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Start failed:\n" + file + " " + args + "\n\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void OpenUri(string uri)
        {
            try { Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true }); }
            catch (Exception ex) { MessageBox.Show("Could not open link: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public static async Task<bool> DownloadAsync(IWin32Window owner, string url, string defaultFileName)
        {
            using (var sfd = new SaveFileDialog { FileName = defaultFileName, Filter = "Programs (*.exe)|*.exe|All files (*.*)|*.*" })
            {
                if (sfd.ShowDialog(owner) != DialogResult.OK) return false;

                try
                {
                    try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; } catch { }
                    using (var wc = new WebClient())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await wc.DownloadFileTaskAsync(new Uri(url), sfd.FileName);
                    }
                    MessageBox.Show("Download complete: " + sfd.FileName, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Download failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally { Cursor.Current = Cursors.Default; }
            }
        }
    }
}
