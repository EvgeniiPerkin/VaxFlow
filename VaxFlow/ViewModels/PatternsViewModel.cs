using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.ViewModels
{
    public partial class PatternsViewModel : ViewModelBase
    {
        private readonly IMyLogger logger;
        private readonly DbContext context;
        public PatternsViewModel(IMyLogger logger, DbContext context)
        {
            this.logger = logger;
            this.context = context;
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
                var pttrns = await context.Pattern.GetAllAsync();
                Patterns = new(pttrns);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки шаблонов.");
            }
        }
        private async Task LoadingPartsAsync(int id)
        {
            try
            {
                SelectedPart = null;
                List<PartModel> prts = new(await context.Part.GetAllAsync());
                var filtParts = prts.Where<PartModel>(i => i.PatternId == id);
                if (filtParts != null)
                {
                    Parts = new(filtParts);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки частей шаблонов.");
            }
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task AddPatternAsync()
        {
            try
            {
                var p = await context.Pattern.CreateAsync(new() { Desc = "Новый шаблон." });
                Patterns.Add(p);
                logger.Info($"Создание шаблона id:{this.SelectedPattern?.Id}.");

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания шаблона.");
            }
        }

        [RelayCommand]
        public async Task UpdatePatternAsync()
        {
            try
            {
                await context.Pattern.UpdateAsync(this.SelectedPattern);
                logger.Info($"Обновление шаблона id:{this.SelectedPattern?.Id}, desc:{this.SelectedPattern?.Desc}");

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения шаблона.");
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
                await context.Pattern.DeleteAsync(this.SelectedPattern);
                logger.Info($"Удаление шаблона id:{this.SelectedPattern?.Id}, desc:{this.SelectedPattern?.Desc}");
                Patterns.Remove(this.SelectedPattern);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления шаблона.");
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
                var p = await context.Part.CreateAsync(new() { SerialNumber = 1, PatternId = SelectedPattern.Id, Desc = "Новая часть документа.", Body = "Содержание" });
                Parts?.Add(p);
                logger.Info($"Создание новой части id:{p.Id}, для шаблона {this.SelectedPattern?.Desc}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания части шаблона.");
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
                await context.Part.UpdateAsync(this.SelectedPart);
                logger.Info($"Обновление части id:{this.SelectedPart.Id}," +
                    $" serialNumber {this.SelectedPart.SerialNumber}," +
                    $" desc:{this.SelectedPart.Desc}," +
                    $" body:{this.SelectedPart.Body}," +
                    $" шаблона {this.SelectedPattern?.Desc}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения части шаблона.");
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
                await context.Part.DeleteAsync(this.SelectedPart);
                logger.Info($"Удаление части id:{this.SelectedPart.Id}," +
                    $" serialNumber {this.SelectedPart.SerialNumber}," +
                    $" desc:{this.SelectedPart.Desc}," +
                    $" body:{this.SelectedPart.Body}," +
                    $" из шаблона {this.SelectedPattern?.Desc}");
                Parts?.Remove(this.SelectedPart);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления части шаблона.");
            }
        }
        private bool CanRemovePartAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
