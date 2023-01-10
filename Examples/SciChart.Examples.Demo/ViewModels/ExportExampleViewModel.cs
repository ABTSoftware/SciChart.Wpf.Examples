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
        private bool _onExported;

        public ExportExampleViewModel(IModule module, ExampleViewModel parent)
        {
            _parent = parent;

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

            ExportCommand = new ActionCommand(async() =>
            {
                ProjectWriter.WriteProject(module.CurrentExample, ExportPath, LibrariesPath);

                OnExported = true;

                if (_parent.Usage != null)
                    _parent.Usage.Exported = true;
                
                await Task.Delay(TimeSpan.FromMilliseconds(2000));

                OnExported = false;
                CloseTrigger = true;

            }, () => IsValid);

            CancelCommand = new ActionCommand(() => IsExportVisible = false);
       
            LibrariesPath = ExportExampleHelper.TryAutomaticallyFindAssemblies();   
        }

        public bool IsExportVisible
        {
            get
            {
                return GetDynamicValue<bool>();
            }
            set
            {
                if (IsExportVisible != value)
                {
                    SetDynamicValue(value);

                    OnExported = false;

                    if (IsExportVisible)
                    {
                        _parent.FeedbackViewModel.IsFeedbackVisible = false;
                        _parent.BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
                    }
                    _parent.InvalidateDialogProperties();
                }
            }
        }

        public bool OnExported
        {
            get => _onExported;
            set
            {
                if (_onExported != value)
                {
                    _onExported = value;
                    OnPropertyChanged(nameof(OnExported));
                }
            }
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

        public bool CloseTrigger
        {
            get => GetDynamicValue<bool>();
            set => SetDynamicValue(value);
        }

        public ActionCommand SelectExportPathCommand { get; }

        public ActionCommand SelectLibraryCommand { get; }

        public ActionCommand ExportCommand { get; }

        public ActionCommand CancelCommand { get; }


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

        public string Error { get; private set; }

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
            if (DirectoryHelper.IsValidPath(LibrariesPath, out string error))
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