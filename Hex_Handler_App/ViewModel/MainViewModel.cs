using Hex_Handler_App.Infrastructure.Enums;
using Hex_Handler_App.Infrastructure.EventsArgs;
using Hex_Handler_App.Infrastructure.Services;
using Hex_Handler_App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Hex_Handler_App.ViewModel
{
    internal class MainViewModel : ModelBase
    {
        private ObservableCollection<HexadecimalKeyModel> _hexModels;

        private readonly IFileWorker _fileWorker;

        private string _enteredFilePath;

        private object _content;

        private Mode _showMode;

        private MainModel _mainModel;

        private LoadViewModel _load;

        public ObservableCollection<HexadecimalKeyModel> HexModels
        {
            get => _hexModels;
            set => SetProperty(ref _hexModels, value, "HexModels");
        }

        public object Content
        {
            get => _content;
            set => SetProperty(ref _content, value, "Content");
        }

        public string EnteredFilePath
        {
            get => _enteredFilePath;
            set => SetProperty(ref _enteredFilePath, value, "EnteredFilePath");
        }

        public Mode ShowMode
        {
            get => _showMode;
            set
            {
                HexModels.Clear();
                EnteredFilePath = "";
                SetProperty(ref _showMode, value, "ShowMode");
            }
        }

        public MainViewModel()
        {
            _hexModels = new ObservableCollection<HexadecimalKeyModel>();
            _fileWorker = new FileWorker();
            _load = new LoadViewModel();
        }

        public Action<object, EventArgs> Messager => ((sender, args) => 
        {
            var arg = (Exception)(args as PackageEventArgs).Message;
            ShowOkMessage(arg.Message);
        });

        private RellayCommand _saveCommand;

        private RellayCommand _findAndDeleteDuplicatesCommand;

        private RellayCommand _openFileCommand;

        public RellayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                    (_saveCommand = new RellayCommand(obj =>
                    {
                        if (EnteredFilePath != null)
                        {
                            try
                            {
                                _load.CloseHandler += ((sender, args) => 
                                {
                                    Content = null;
                                    SaveData(sender, args); 
                                });
                                _load.ExecuteFunc<string>(_mainModel.PreparingDataForSaving);
                                Content = _load;
                            }
                            catch (Exception e)
                            {
                                ShowOkMessage(e.Message);
                            }
                        }
                    },
                        (obj) => NotNull()
                    ));
            }
        }

        public RellayCommand FindAndDeleteDuplicatesCommand
        {
            get
            {
                return _findAndDeleteDuplicatesCommand ??
                    (_findAndDeleteDuplicatesCommand = new RellayCommand(obj =>
                    {
                        try
                        {
                            _load.ExecuteFunc<HashSet<HexadecimalKeyModel>>(_mainModel.GetDuplicates);
                            _load.CloseHandler += ((sender, args) => DeletingAndRecalculatingData(args));
                            Content = _load;
                        }
                        catch (Exception e)
                        {
                            ShowOkMessage(e.Message);
                        }
                    },
                        (obj) => NotNull()
                    ));
            }
        }

        public RellayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ??
                    (_openFileCommand = new RellayCommand(obj =>
                    {
                        EnteredFilePath = _fileWorker.OpenFileDialog();
                        if (EnteredFilePath != null)
                        {
                            HexModels.Clear();
                            try
                            {
                                ModelInitialization();
                                string fileData = _fileWorker.GetFileData(EnteredFilePath);
                                _load.CloseHandler += ((sender, args) => { Content = null; });
                                _load.ExecuteAction<string>(_mainModel.ReadAndParsingData, fileData);
                                Content = _load;
                            }
                            catch (Exception e)
                            {
                                ShowOkMessage(e.Message);
                            }
                        }
                    }));
            }
        }

        private void ModelInitialization()
        {
            if (ShowMode == Mode.Key)
            {
                _mainModel = new KeyModel(HexModels);
            }
            else if (ShowMode == Mode.Hash)
            {
                _mainModel = new HashModel(HexModels);
            }
        }

        private async void SaveData(object sender, EventArgs args)
        {            
            if (args != EventArgs.Empty)
            {
                string savedData = await (Task<string>)(args as PackageEventArgs).Message;
                if (_fileWorker.SaveData(savedData))
                {
                    ShowOkMessage("The file has been successfully saved.");
                }
                else
                {
                    ShowOkMessage("Saving the file was canceled by the user."); 
                }                
            }
        }

        private async void DeletingAndRecalculatingData(EventArgs args)
        {
            Content = null;
            if (args != EventArgs.Empty)
            {
                var duplicates = await (Task<HashSet<HexadecimalKeyModel>>)(args as PackageEventArgs).Message;
                if (duplicates != null && duplicates.Count != 0)
                {
                    if (ShowYesNoMessage($"{duplicates.Count} duplicates detected. Delete it?"))
                    {
                        _load.ExecuteAction<HashSet<HexadecimalKeyModel>>(_mainModel.DeleteDuplicates, duplicates);
                        ShowOkMessage("The operation is complete. All duplicates are deleted.");
                    }
                }
                else
                {
                    ShowOkMessage("No duplicates found.");
                }
            }
        }

        private bool NotNull() =>
            HexModels != null && HexModels.Count != 0;

        private void ShowOkMessage(string message) => 
            MessageBox.Show(message, "Message", MessageBoxButton.OK);


        private bool ShowYesNoMessage(string message) => 
            MessageBox.Show(message, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes;       
    }
}
