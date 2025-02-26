using System;
using System.ComponentModel;
using System.Threading.Tasks;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.ProjectExport;
using SciChart.UI.Reactive.Observability;

namespace SciChart.Examples.Demo.ViewModels
{
    public class ExportExampleViewModel : ViewModelWithTraitsBase, IDataErrorInfo
    {
        private readonly ExampleViewModel _parent;

        public ExportExampleViewModel(IModule module, ExampleViewModel parent)
        {
            _parent = parent;

            ExportVersionMajor = ProjectWriter.VersionMajor;

            SelectExportPathCommand = new ActionCommand(() =>
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ExportPath = dialog.SelectedPath;
                }
            });

            SelectLibraryCommand = new ActionCommand(() =>
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LibrariesPath = dialog.SelectedPath;
                }
            });

            ExportCommand = new ActionCommand(async () =>
            {
                CanExport = false;

                var projectName = ProjectWriter.WriteProject(module.CurrentExample, ExportPath, LibrariesPath, IsLibFromFolder);

                if (string.IsNullOrEmpty(projectName))
                {
                    OnExportError = true;

                    await Task.Delay(4000);

                    OnExportError = false;
                }
                else
                {
                    OnExportSuccess = true;

                    if (_parent.Usage != null)
                        _parent.Usage.Exported = true;
                    
                    await Task.Delay(2000);

                    OnExportSuccess = false;
                }

                CanExport = true;

            }, () => IsValid);

            CancelCommand = new ActionCommand(() => IsExportVisible = false);

            LibrariesPath = ExportExampleHelper.TryAutomaticallyFindAssemblies();
        }

        public ActionCommand SelectExportPathCommand { get; }
        public ActionCommand SelectLibraryCommand { get; }

        public ActionCommand ExportCommand { get; }
        public ActionCommand CancelCommand { get; }

        public int ExportVersionMajor { get; }

        public bool IsExportVisible
        {
            get => GetDynamicValue<bool>();
            set
            {
                if (IsExportVisible != value)
                {
                    SetDynamicValue(value);

                    CanExport = true;
                    OnExportSuccess = false;
                    OnExportError = false;

                    if (IsExportVisible)
                    {
                        _parent.FeedbackViewModel.IsFeedbackVisible = false;
                        _parent.BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
                    }

                    _parent.InvalidateDialogProperties();
                }
            }
        }

        public bool IsLibFromFolder
        {
            get => GetDynamicValue<bool>();
            set
            {
                SetDynamicValue(value);
                ExportCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanExport
        {
            get => GetDynamicValue<bool>();
            set => SetDynamicValue(value);
        }

        public bool OnExportSuccess
        {
            get => GetDynamicValue<bool>();
            set => SetDynamicValue(value);
        }

        public bool OnExportError
        {
            get => GetDynamicValue<bool>();
            set => SetDynamicValue(value);
        }

        public string ExportPath
        {
            get => GetDynamicValue<string>();
            set
            {
                SetDynamicValue(value);
                ExportCommand.RaiseCanExecuteChanged();
            }
        }

        public string LibrariesPath
        {
            get => GetDynamicValue<string>();
            set
            {
                SetDynamicValue(value);
                ExportCommand.RaiseCanExecuteChanged();
            }
        }

        #region IDataErrorInfo

        private static readonly string[] ValidatedProperties =
        {
            nameof(ExportPath),
            nameof(LibrariesPath)
        };

        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                {
                    if (GetValidationError(property) != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public string Error { get; }

        public string this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }

        private string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
            {
                return null;
            }

            string error = null;

            switch (propertyName)
            {
                case nameof(ExportPath):
                    error = ValidateExportPath();
                    break;

                case nameof(LibrariesPath):
                    error = ValidateLibrariesPath();
                    break;
            }

            return error;
        }

        private string ValidateExportPath()
        {
            if (DirectoryHelper.IsValidPath(ExportPath, out string error))
            {
                DirectoryHelper.HasWriteAccessToFolder(ExportPath, out error);
            }

            return error;
        }

        private string ValidateLibrariesPath()
        {
            string error = null;

            if (IsLibFromFolder && DirectoryHelper.IsValidPath(LibrariesPath, out error))
            {
                if (!ExportExampleHelper.SearchForCoreAssemblies(LibrariesPath))
                {
                    error = "We are sorry, but you have to manually select path to SciChart installation folder!";
                }
            }

            return error;
        }

        #endregion
    }
}