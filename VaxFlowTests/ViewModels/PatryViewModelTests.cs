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
            ObservableCollection<PartySummaryModel> collection1 =
            [
                new() { Id = 1, PartyName = "OldName" }
            ];
            ObservableCollection<PartySummaryModel> collection2 =
            [];

            PatryViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual(0, collection1.Count);
        }
        [TestMethod()]
        public void RefreshCollections_ReplaceItem()
        {
            ObservableCollection<PartySummaryModel> collection1 = 
            [
                new() { Id = 1, PartyName = "OldName" }
            ];
            ObservableCollection<PartySummaryModel> collection2 =
            [
                new() { Id = 1, PartyName = "NewName" }
            ];

            PatryViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual("NewName", collection1[0].PartyName);
        }
        [TestMethod()]
        public void RefreshCollections_NewItem()
        {
            ObservableCollection<PartySummaryModel> collection1 =
            [];
            ObservableCollection<PartySummaryModel> collection2 =
            [
                new() { Id = 1, PartyName = "NewName" }
            ];

            PatryViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual("NewName", collection1[0].PartyName);
        }
        [TestMethod()]
        public void RefreshCollections_RemoveOldItem()
        {
            ObservableCollection<PartySummaryModel> collection1 =
            [
                new() { Id = 1, PartyName = "OldName" }
            ];
            ObservableCollection<PartySummaryModel> collection2 =
            [
                new() { Id = 2, PartyName = "NewName" }
            ];

            PatryViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual(1, collection1.Count);
            Assert.AreEqual(2, collection1[0].Id);
            Assert.AreEqual("NewName", collection1[0].PartyName);
        }
        [TestMethod()]
        public void RefreshCollections_AddNewItemAndRefreshOldItem()
        {
            ObservableCollection<PartySummaryModel> collection1 =
            [
                new() { Id = 1, PartyName = "One" }
            ];
            ObservableCollection<PartySummaryModel> collection2 =
            [
                new() { Id = 1, PartyName = "NewOne" },
                new() { Id = 2, PartyName = "Two" }
            ];

            PatryViewModel.RefreshCollections(collection1, collection2);
            Assert.AreEqual(2, collection1.Count);
            Assert.AreEqual("NewOne", collection1[0].PartyName);
            Assert.AreEqual("Two", collection1[1].PartyName);
        }
    }
}