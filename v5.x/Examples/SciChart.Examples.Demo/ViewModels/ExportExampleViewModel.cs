using System;
using System.ComponentModel;


using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Behaviors;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.ProjectExport;
using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Examples.Demo.ViewModels
{
    public class ExportExampleViewModel : ViewModelWithTraitsBase, IDataErrorInfo
    {
        private readonly ExampleViewModel _parent;
        private readonly ActionCommand _selectExportPathCommand;
        private readonly ActionCommand _selectLibraryCommand;
        private readonly ActionCommand _okCommand;
        private readonly ActionCommand _cancelCommand;

        public ExportExampleViewModel(IModule module, ExampleViewModel parent)
        {
            _parent = parent;
            WithTrait<DiscoverCoreAssembliesBehavior>();

            _selectExportPathCommand = new ActionCommand(() =>
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ExportPath = dialog.SelectedPath;
                }
            });

            _selectLibraryCommand = new ActionCommand(() =>
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LibrariesPath = dialog.SelectedPath;
                }
            });

            _okCommand = new ActionCommand(
                () =>
                {
                    ProjectWriter.WriteProject(module.CurrentExample, ExportPath + @"\", LibrariesPath);
                    _parent.Usage.Exported = true;
                    CloseTrigger = true;
                },
                () => !string.IsNullOrEmpty(ExportPath) && ValidateExportPath() == null);

            _cancelCommand = new ActionCommand(() => IsExportVisible = false);

            ExportPath = "";
        }

        public bool IsExportVisible
        {
            get
            {
                return GetDynamicValue<bool>("IsExportVisible");                
            }
            set
            {
                if (IsExportVisible != value)
                {
                    SetDynamicValue("IsExportVisible", value);

                    if (IsExportVisible)
                    {
                        _parent.SmileFrownViewModel.SmileVisible = false;
                        _parent.SmileFrownViewModel.FrownVisible = false;
                        _parent.BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
                    }
                    _parent.InvalidateDialogProperties();
                }
            }
        }

        public string ExportPath
        {
            get { return GetDynamicValue<string>("ExportPath"); }
            set
            {
                SetDynamicValue("ExportPath", value);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public string LibrariesPath
        {
            get { return GetDynamicValue<string>("LibrariesPath"); }
            set { SetDynamicValue("LibrariesPath", value); }
        }

        public bool IsAssemblyOk
        {
            get { return GetDynamicValue<bool>("IsAssemblyOk"); }
            set { SetDynamicValue("IsAssemblyOk", value); }
        }

        public bool CloseTrigger
        {
            get { return GetDynamicValue<bool>("CloseTrigger"); }
            set { SetDynamicValue("CloseTrigger", value); }
        }

        public ActionCommand SelectExportPathCommand
        {
            get { return _selectExportPathCommand; }
        }
        
        public ActionCommand SelectLibraryCommand
        {
            get { return _selectLibraryCommand; }
        }    
        
        public ActionCommand OkCommand
        {
            get { return _okCommand; }
        }   
        
        public ActionCommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        #region IDataErrorInfo

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }

        private static readonly string[] ValidatedProperties =
        {
            "ExportPath",
            "LibrariesPath"
        };

        private string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;
            string error = null;

            switch (propertyName)
            {
                case "ExportPath":
                    error = ValidateExportPath();
                    break;
                case "LibrariesPath":
                    error = ValidateLibrariesPath();
                    break;
            }

            return error;
        }

        private string ValidateExportPath()
        {
            string error;

            if (DirectoryHelper.IsValidPath(ExportPath, out error))
            {
                DirectoryHelper.HasWriteAccessToFolder(ExportPath, out error);
            }

            return error;
        }

        private string ValidateLibrariesPath()
        {
            string error = null;

            if (ViewModelTraits.Contains<DiscoverCoreAssembliesBehavior>())
            {                
                var assembliesExist = ExportExampleHelper.SearchForCoreAssemblies(ExportPath);
                if (!assembliesExist)
                {
                    error = "We are sorry, but you have to manually select path to SciChart installation folder!";
                }
            }

            return error;
        }

        #endregion
    }
}