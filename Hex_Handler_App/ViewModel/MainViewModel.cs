using Hex_Handler_App.Infrastructure.Enums;
using Hex_Handler_App.Infrastructure.EventsArgs;
using Hex_Handler_App.Infrastructure.Services;
using Hex_Handler_App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        private readonly LoadViewModel _load;

        private readonly MessageManager _messager;

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
            _messager = new MessageManager();
        }

        public Action<object, EventArgs> Messager => ((sender, args) => 
        {
            var arg = (Exception)(args as PackageEventArgs).Message;
            _messager.ShowOkMessage(arg.Message);
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
                                Content = _load;
                                _load.ResettingSubscriptions();
                                _load.ExecuteFunc<string>(_mainModel.PreparingDataForSaving);
                                _load.CloseHandler += ((sender, args) => SaveData(args));
                            }
                            catch (Exception e)
                            {
                                _messager.ShowOkMessage(e.Message);
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
                            Content = _load;
                            _load.ResettingSubscriptions();
                            _load.ExecuteFunc<HashSet<HexadecimalKeyModel>>(_mainModel.GetDuplicates);
                            _load.CloseHandler += ((sender, args) => DeletingAndRecalculatingData(args));
                        }
                        catch (Exception e)
                        {
                            _messager.ShowOkMessage(e.Message);
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
                                Content = _load;
                                _load.ResettingSubscriptions();
                                ModelInitialization();
                                string fileData = _fileWorker.GetFileData(EnteredFilePath);
                                _load.CloseHandler += ((sender, args) => { Content = null; });
                                _load.ExecuteAction<string>(_mainModel.ReadAndParsingData, fileData);
                            }
                            catch (Exception e)
                            {
                                _messager.ShowOkMessage(e.Message);
                            }
                        }
                    }));
            }
        }

        /// <summary>
        /// Initializing the current model
        /// </summary>
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

        /// <summary>
        /// Saving data
        /// </summary>
        /// <param name="args">A package containing the data to be saved</param>
        private async void SaveData(EventArgs args)
        {
            Content = null;
            if (args != EventArgs.Empty)
            {
                string savedData = await (Task<string>)(args as PackageEventArgs).Message;
                if (_fileWorker.SaveData(savedData))
                {
                    _messager.ShowOkMessage("The file has been successfully saved.");
                }
                else
                {
                    _messager.ShowOkMessage("Saving the file was canceled by the user."); 
                }
                _load.CloseHandler -= ((sender, args) => SaveData(args));
            }
        }

        /// <summary>
        /// Find and delete duplicates
        /// </summary>
        /// <param name="args">A package containing a list of data to delete</param>
        private async void DeletingAndRecalculatingData(EventArgs args)
        {
            Content = null;
            if (args != EventArgs.Empty)
            {
                var duplicates = await (Task<HashSet<HexadecimalKeyModel>>)(args as PackageEventArgs).Message;
                if (duplicates != null && duplicates.Count != 0)
                {
                    if (_messager.ShowYesNoMessage($"{duplicates.Count} duplicates detected. Delete it?"))
                    {
                        _load.ExecuteAction<HashSet<HexadecimalKeyModel>>(_mainModel.DeleteDuplicates, duplicates);
                        _messager.ShowOkMessage("The operation is complete. All duplicates are deleted.");
                    }
                }
                else
                {
                    _messager.ShowOkMessage("No duplicates found.");
                }
            }
            _load.CloseHandler -= ((sender, args) => DeletingAndRecalculatingData(args));
        }

        /// <summary>
        /// Checking for data existence
        /// </summary>
        /// <returns>Test result</returns>
        private bool NotNull() =>
            HexModels != null && HexModels.Count != 0;      
    }
}
