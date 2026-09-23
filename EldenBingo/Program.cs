using EldenBingo.Util;
using EldenBingoCommon;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace EldenBingo
{
    internal static class Program
    {
        private static MainForm? _mainForm;
        private static int _fatalExceptionHandling;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => HandleFatalException(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    HandleFatalException(ex);
                }
                else
                {
                    HandleFatalException(new Exception("Unknown unhandled exception."));
                }
            };

            try
            {
                ApplicationConfiguration.Initialize();

                handleSettingsChanges();
                LocalizationManager.CurrentLanguage = Properties.Settings.Default.Language;

                _mainForm = new MainForm();
                Application.Run(_mainForm);
            }
            catch (Exception ex)
            {
                HandleFatalException(ex);
            }
        }

        private static void handleSettingsChanges()
        {
            bool changed = false;
            const int idTokenLength = 10;

            if (Properties.Settings.Default.IsFirstRun)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.IsFirstRun = false;
                changed = true;
            }

            if ((Properties.Settings.Default.IdentityToken?.Length ?? 0) != idTokenLength)
            {
                Properties.Settings.Default.IdentityToken = IdentityToken.GenerateIdentityToken(idTokenLength);
                changed = true;
            }

            // Use previous binding for Click hotkey if set.
            if (Properties.Settings.Default.ClickHotkey > 0)
            {
                Properties.Settings.Default.Hotkey_Check = Properties.Settings.Default.ClickHotkey;
                Properties.Settings.Default.ClickHotkey = 0;
                changed = true;
            }

            if (Properties.Settings.Default.NumpadNavigation)
            {
                Properties.Settings.Default.Hotkey_Up = (int)Keys.NumPad8;
                Properties.Settings.Default.Hotkey_Down = (int)Keys.NumPad2;
                Properties.Settings.Default.Hotkey_Left = (int)Keys.NumPad4;
                Properties.Settings.Default.Hotkey_Right = (int)Keys.NumPad6;
                Properties.Settings.Default.Hotkey_UpLeft = (int)Keys.NumPad7;
                Properties.Settings.Default.Hotkey_UpRight = (int)Keys.NumPad9;
                Properties.Settings.Default.Hotkey_DownLeft = (int)Keys.NumPad1;
                Properties.Settings.Default.Hotkey_DownRight = (int)Keys.NumPad3;
                Properties.Settings.Default.Hotkey_Star = (int)Keys.Multiply;
                Properties.Settings.Default.Hotkey_CountIncrease = (int)Keys.Add;
                Properties.Settings.Default.Hotkey_CountDecrease = (int)Keys.Subtract;
                Properties.Settings.Default.NumpadNavigation = false;
                changed = true;
            }
            else if (Properties.Settings.Default.ArrowNavigation)
            {
                Properties.Settings.Default.Hotkey_Up = (int)Keys.Up;
                Properties.Settings.Default.Hotkey_Down = (int)Keys.Down;
                Properties.Settings.Default.Hotkey_Left = (int)Keys.Left;
                Properties.Settings.Default.Hotkey_Right = (int)Keys.Right;
                Properties.Settings.Default.ArrowNavigation = false;
                changed = true;
            }

            changed |= rewriteOldAddress();

            if (changed)
            {
                Properties.Settings.Default.Save();
            }
        }

        private static bool rewriteOldAddress()
        {
            var serverAddress = Properties.Settings.Default.ServerAddress ?? string.Empty;
            var oldServerAddressesValue = Properties.Settings.Default.OldServerAddresses ?? string.Empty;
            var oldServerAddresses = new HashSet<string>(
                oldServerAddressesValue.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                StringComparer.InvariantCultureIgnoreCase);

            if (oldServerAddresses.Contains(serverAddress))
            {
                var prop = typeof(Properties.Settings).GetProperty("ServerAddress");
                if (prop != null)
                {
                    var defValue = prop.GetCustomAttribute<DefaultSettingValueAttribute>();
                    if (defValue != null)
                    {
                        Properties.Settings.Default.ServerAddress = defValue.Value;
                        return true;
                    }
                }
            }

            return false;
        }

        private static void HandleFatalException(Exception ex)
        {
            if (Interlocked.Exchange(ref _fatalExceptionHandling, 1) != 0)
            {
                return;
            }

            string logPath;
            try
            {
                logPath = CrashLogger.LogException(ex);
            }
            catch
            {
                logPath = "Crash log could not be written.";
            }

            var message =
                $"EldenBingo could not continue.{Environment.NewLine}{Environment.NewLine}" +
                $"{ex.GetType().Name}: {ex.Message}{Environment.NewLine}{Environment.NewLine}" +
                $"Crash log:{Environment.NewLine}{logPath}";

            try
            {
                MessageBox.Show(
                    message,
                    "EldenBingo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                // Avoid masking the original startup failure if Windows cannot show the dialog.
            }
            finally
            {
                Environment.ExitCode = 1;
                Application.Exit();
            }
        }
    }
}
