using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    public class BackupViewModel: ViewModelBase
    {
        private String _backupDirectoryPath;
        public BackupViewModel()
        {
            _createBackupDirectoryIfNotExist();
            FileList = new ObservableCollection<String>();
            _loadFileList();
        }

        public String BackupDirectoryPath 
        { 
            get 
            {
                DirectoryInfo di = new DirectoryInfo(_backupDirectoryPath);
                return di.FullName;
            } 
        }

        private void _loadFileList()
        {
            FileList.Clear();
            DirectoryInfo di = new DirectoryInfo(_backupDirectoryPath);
            var files = di.GetFiles("*.sdf");
            foreach (var file in files)
            {
                FileList.Insert(0, String.Format("{0} ({1}Кб)", file.Name, file.Length / 1024));
            }
        }

        public IList<String> FileList { get; private set; }

        private void _createBackupDirectoryIfNotExist()
        {
            var databasePath = Properties.Settings.Default.DatabasePath;
            var directoryPath = Path.GetDirectoryName(databasePath);
            _backupDirectoryPath = directoryPath + @"\backup";
            if (!Directory.Exists(_backupDirectoryPath))
            {
                Directory.CreateDirectory(_backupDirectoryPath);
            }
        }

        private ICommand _backupDatabaseCommand;
        public ICommand BackupDatabaseCommand 
        {
            get
            {
                _backupDatabaseCommand = _backupDatabaseCommand ?? new RelayCommand(() =>
                    {
                        var databasePath = Properties.Settings.Default.DatabasePath;
                        FileInfo databaseFile = new FileInfo(databasePath);
                        var databaseFileName = Path.GetFileNameWithoutExtension(databasePath);
                        var databaseFileExt = Path.GetExtension(databasePath);
                        var now = DateTime.Now;
                        var date = String.Format("{0:0000}-{1:00}-{2:00}_{3:00}h{4:00}m{5:00}s", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                        var backupFileName = String.Format(@"{0}\{1}_{2}{3}", _backupDirectoryPath, databaseFileName, date, databaseFileExt);
                        File.Copy(databasePath, backupFileName, true);
                        _loadFileList();
                    });
                return _backupDatabaseCommand;
            }
        }

        private ICommand _restoreDatabaseCommand;
        public ICommand RestoreDatabaseCommand
        {
            get
            {
                _restoreDatabaseCommand = _restoreDatabaseCommand ?? new RelayCommand(() =>
                    {
                        if (String.IsNullOrEmpty(_selectedFile))
                            return;
                    });
                return _restoreDatabaseCommand;
            }
        }

        private String _selectedFile;
        public String SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                if (_selectedFile != value)
                {
                    _selectedFile = value;
                    OnPropertyChanged(() => SelectedFile);
                }
            }
        }
    }
}
