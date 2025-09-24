using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.DialogWindows;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.ViewModels
{
    public partial class PatternsViewModel : ViewModelBase
    {
        private readonly IMyLogger logger;
        private readonly DbContext context;
        private readonly IDialogWindow dialogWindow;
        public PatternsViewModel(IMyLogger logger, DbContext context, IDialogWindow dialogWindow)
        {
            this.logger = logger;
            this.context = context;
            this.dialogWindow = dialogWindow;
            Task.Run(() => LoadingPatternsAsync());
        }

        #region Properties
        [ObservableProperty]
        private ObservableCollection<PatternModel> _Patterns = [];
        private PatternModel? _SelectedPattern;
        public PatternModel? SelectedPattern
        {
            get => _SelectedPattern;
            set
            {
                SetProperty(ref _SelectedPattern, value);
                if (_SelectedPattern != null) Task.Run(() => LoadingPartsAsync(_SelectedPattern.Id));
            }
        }

        [ObservableProperty]
        private ObservableCollection<PartModel>? _Parts;
        private PartModel? _SelectedPart;
        public PartModel? SelectedPart
        {
            get => _SelectedPart;
            set
            {
                SetProperty(ref _SelectedPart, value);
            }
        }
        #endregion

        #region Methods
        private async Task LoadingPatternsAsync()
        {
            try
            {
                Patterns = await context.Pattern.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки шаблонов документов.");
            }
        }
        private async Task LoadingPartsAsync(int id)
        {
            try
            {
                SelectedPart = null;
                Parts = await context.Part.FindByPatternIdAsync(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки частей шаблонов документов.");
            }
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task AddPatternAsync()
        {
            try
            {
                PatternModel newPattern = new() { Desc = "Новый шаблон." };
                int affectedRows = await context.Pattern.CreateAsync(newPattern);
                if (affectedRows > 0)
                {
                    Patterns.Add(newPattern);
                    logger.Info($"Создание шаблона id:{this.SelectedPattern?.Id}.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания шаблона.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", $"Ошибка создания шаблона: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task UpdatePatternAsync()
        {
            try
            {
                if (SelectedPattern == null) return;

                int affectedRows = await context.Pattern.UpdateAsync(SelectedPattern);
                if (affectedRows > 0)
                {
                    logger.Info($"Обновление шаблона id:{SelectedPattern?.Id}, desc:{SelectedPattern?.Desc}");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения шаблона.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", $"Ошибка изменения шаблона: {ex.Message}");
            }
        }
        private bool CanUpdatePatternAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task RemovePatternAsync()
        {
            try
            {
                if (SelectedPattern == null) return;
                bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления.", "Удалить шаблон документа безвозвратно?");
                if (result)
                {
                    int affectedRows = await context.Pattern.DeleteAsync(SelectedPattern);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Удаление шаблона id:{SelectedPattern.Id}, desc:{SelectedPattern.Desc}");
                        Patterns.Remove(SelectedPattern);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления шаблона.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", $"Ошибка удаления шаблона: {ex.Message}");
            }
        }
        private bool CanRemovePatternAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task AddPartAsync()
        {
            try
            {
                if (SelectedPattern == null) return;
                int sn = 1;
                if (Parts != null && Parts.Any())
                {
                    sn = Parts?.Max(x => x.SerialNumber) ?? 1;
                }
                PartModel newPart = new()
                {
                    SerialNumber = ++sn,
                    PatternId = SelectedPattern.Id,
                    Desc = "Новая часть документа.",
                    Body = "Содержание",
                    IsUrl = false,
                    IsBold = false,
                    IsItalic = false,
                    IsUnderline = false
                };

                int newId = await context.Part.CreateAsync(newPart);
                if (newId > 0)
                {
                    Parts?.Add(newPart);
                    logger.Info($"Создание новой части id:{newPart.Id}, для шаблона {SelectedPattern?.Desc}");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания части шаблона.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", $"Ошибка создания части шаблона: {ex.Message}");
            }
        }
        private bool CanAddPartAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task UpdatePartAsync()
        {
            try
            {
                if (SelectedPart == null) return;

                int affectedRows = await context.Part.UpdateAsync(SelectedPart);
                if (affectedRows > 0)
                {
                    logger.Info($"Обновление части id:{SelectedPart.Id}," +
                        $" serialNumber {SelectedPart.SerialNumber}," +
                        $" desc:{SelectedPart.Desc}," +
                        $" body:{SelectedPart.Body}," +
                        $" шаблона {SelectedPattern?.Desc}");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения части шаблона.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", $"Ошибка изменения части шаблона: {ex.Message}");
            }
        }
        private bool CanUpdatePartAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task RemovePartAsync()
        {
            try
            {
                if (SelectedPart == null) return;

                bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления.", "Удалить чать шаблона документа безвозвратно?");
                if (result)
                {
                    int affectedRows = await context.Part.DeleteAsync(SelectedPart);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Удаление части id:{SelectedPart.Id}," +
                            $" serialNumber {SelectedPart.SerialNumber}," +
                            $" desc:{SelectedPart.Desc}," +
                            $" body:{SelectedPart.Body}," +
                            $" из шаблона {SelectedPattern?.Desc}");
                        Parts?.Remove(SelectedPart);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления части шаблона.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", $"Ошибка удаления части шаблона: {ex.Message}");
            }
        }
        private bool CanRemovePartAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
