using System.Collections.ObjectModel;
using VaxFlow.Models;

namespace VaxFlow.ViewModels.Tests
{
    [TestClass()]
    public class PatryViewModelTests
    {
        [TestMethod()]
        public void RefreshCollections_EmptyItem()
        {
            ObservableCollection<VaccineSummaryModel> collection1 =
            [
                new() { Id = 1, VaccineName = "OldName" }
            ];
            ObservableCollection<VaccineSummaryModel> collection2 =
            [];

            VaccineViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual(0, collection1.Count);
        }
        [TestMethod()]
        public void RefreshCollections_ReplaceItem()
        {
            ObservableCollection<VaccineSummaryModel> collection1 = 
            [
                new() { Id = 1, VaccineName = "OldName" }
            ];
            ObservableCollection<VaccineSummaryModel> collection2 =
            [
                new() { Id = 1, VaccineName = "NewName" }
            ];

            VaccineViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual("NewName", collection1[0].VaccineName);
        }
        [TestMethod()]
        public void RefreshCollections_NewItem()
        {
            ObservableCollection<VaccineSummaryModel> collection1 =
            [];
            ObservableCollection<VaccineSummaryModel> collection2 =
            [
                new() { Id = 1, VaccineName = "NewName" }
            ];

            VaccineViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual("NewName", collection1[0].VaccineName);
        }
        [TestMethod()]
        public void RefreshCollections_RemoveOldItem()
        {
            ObservableCollection<VaccineSummaryModel> collection1 =
            [
                new() { Id = 1, VaccineName = "OldName" }
            ];
            ObservableCollection<VaccineSummaryModel> collection2 =
            [
                new() { Id = 2, VaccineName = "NewName" }
            ];

            VaccineViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual(1, collection1.Count);
            Assert.AreEqual(2, collection1[0].Id);
            Assert.AreEqual("NewName", collection1[0].VaccineName);
        }
        [TestMethod()]
        public void RefreshCollections_AddNewItemAndRefreshOldItem()
        {
            ObservableCollection<VaccineSummaryModel> collection1 =
            [
                new() { Id = 1, VaccineName = "One" }
            ];
            ObservableCollection<VaccineSummaryModel> collection2 =
            [
                new() { Id = 1, VaccineName = "NewOne" },
                new() { Id = 2, VaccineName = "Two" }
            ];

            VaccineViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual(2, collection1.Count);
            Assert.AreEqual("NewOne", collection1[0].VaccineName);
            Assert.AreEqual("Two", collection1[1].VaccineName);
        }
    }
}