﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Wr.UmbracoCampaignPhoneManager.Models;
using Wr.UmbracoCampaignPhoneManager.Providers;
using Wr.UmbracoCampaignPhoneManager.Tests.Providers.Storage;

namespace Wr.UmbracoCampaignPhoneManager.Tests
{
    [TestClass]
    public class CampaignPhoneManagerAppTest
    {
        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessAllPotentialCandidatePhoneNumbers_WithNoCookie_WithNoFoundPhoneNumber_WithNoDefaultPhonenumber_ReturnsLastResortPhoneNumber()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel() { DefaultPhoneNumber = "", DefaultCampaignQueryStringKey = "fsource", DefaultPersistDurationInDays = 32 };
            dataModel.CampaignDetail = new List<CampaignDetail>() { new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" } };
            var testPhoneManagerData = dataModel.ToXmlString();

            var _dataProvider = MockProviders.Repository(testPhoneManagerData);

            // generate the required result
            var correctResult = new FinalResultModel()
            {
                OutputCookieHolder = null,
                OutputModelResult = new OutputModel() { TelephoneNumber = AppConstants.LastResortPhoneNumberPlaceholder },
                OutputResultSource = OutputSource.LastResortPlaceholder
            };

            CampaignPhoneManagerApp target = new CampaignPhoneManagerApp(null, _dataProvider, null, null, null, null);
            PrivateObject obj = new PrivateObject(target);

            //Act
            FinalResultModel retVal = (FinalResultModel)obj.Invoke("ProcessAllPotentialCandidatePhoneNumbers", new object[] { new CookieHolder(), new CampaignDetail() });

            //Assert
            Assert.AreEqual(retVal.OutputResultSource, correctResult.OutputResultSource);
            Assert.AreEqual(retVal.OutputModelResult.TelephoneNumber, correctResult.OutputModelResult.TelephoneNumber);
        }

        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessAllPotentialCandidatePhoneNumbers_WithNoCookie_WithNoFoundPhoneNumber_WithDefaultPhonenumber_ReturnsDefaultPhoneNumber()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel() { DefaultPhoneNumber = "0800 000 0001", DefaultCampaignQueryStringKey = "fsource", DefaultPersistDurationInDays = 32 };
            dataModel.CampaignDetail = new List<CampaignDetail>() { new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" } };
            var testPhoneManagerData = dataModel.ToXmlString();

            var _dataProvider = MockProviders.Repository(testPhoneManagerData);

            // generate the required result
            var correctResult = new FinalResultModel()
            {
                OutputCookieHolder = null,
                OutputModelResult = new OutputModel() { TelephoneNumber = "0800 000 0001" },
                OutputResultSource = OutputSource.DefaultNumberFromAdmin
            };

            CampaignPhoneManagerApp target = new CampaignPhoneManagerApp(null, _dataProvider, null, null, null, null);
            PrivateObject obj = new PrivateObject(target);

            //Act
            FinalResultModel retVal = (FinalResultModel)obj.Invoke("ProcessAllPotentialCandidatePhoneNumbers", new object[] { new CookieHolder(), new CampaignDetail() });

            //Assert
            Assert.AreEqual(retVal.OutputResultSource, correctResult.OutputResultSource);
        }

        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessAllPotentialCandidatePhoneNumbers_WithNoCookie_WithFoundPhoneNumber_ReturnsFoundPhoneNumber()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel() { DefaultPhoneNumber = "0800 000 0001", DefaultCampaignQueryStringKey = "fsource", DefaultPersistDurationInDays = 32 };
            dataModel.CampaignDetail = new List<CampaignDetail>() { new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" } };
            var testPhoneManagerData = dataModel.ToXmlString();

            var _dataProvider = MockProviders.Repository(testPhoneManagerData);

            var foundRecord = new CampaignDetail() { Id = "1201", TelephoneNumber = "FOUND PHONENUMBER" };
            // generate the required result
            var correctResult = new FinalResultModel()
            {
                OutputCookieHolder = null,
                OutputModelResult = new OutputModel() { TelephoneNumber = "FOUND PHONENUMBER" },
                OutputResultSource = OutputSource.FoundRecordFromCriteria
            };

            CampaignPhoneManagerApp target = new CampaignPhoneManagerApp(null, _dataProvider, null, null, null, null);
            PrivateObject obj = new PrivateObject(target);

            //Act
            FinalResultModel retVal = (FinalResultModel)obj.Invoke("ProcessAllPotentialCandidatePhoneNumbers", new object[] { new CookieHolder(), foundRecord });

            //Assert
            Assert.AreEqual(retVal.OutputResultSource, correctResult.OutputResultSource);
            Assert.AreEqual(retVal.OutputModelResult.TelephoneNumber, correctResult.OutputModelResult.TelephoneNumber);
        }

        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessAllPotentialCandidatePhoneNumbers_WithCookie_WithFoundPhoneNumberNoPersist_ReturnsCookie()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel() { DefaultPhoneNumber = "0800 000 0001", DefaultCampaignQueryStringKey = "fsource", DefaultPersistDurationInDays = 32 };
            dataModel.CampaignDetail = new List<CampaignDetail>() { new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" } };
            var testPhoneManagerData = dataModel.ToXmlString();

            var _dataProvider = MockProviders.Repository(testPhoneManagerData);

            var foundRecord = new CampaignDetail() { Id = "1201", TelephoneNumber = "FOUND PHONENUMBER" };

            var _cookie = new CookieHolder() { Model =
                new OutputModel()
                {
                    Id = "1202",
                    TelephoneNumber = "9999 999 9999"
                }
            };

            // generate the required result
            var correctResult = new FinalResultModel()
            {
                OutputCookieHolder = new CookieHolder(),
                OutputModelResult = new OutputModel() {
                    Id = "1202",
                    TelephoneNumber = "9999 999 9999"
                },
                OutputResultSource = OutputSource.ExisitingCookie
            };

            CampaignPhoneManagerApp target = new CampaignPhoneManagerApp(null, _dataProvider, null, null, null, null);
            PrivateObject obj = new PrivateObject(target);

            //Act
            FinalResultModel retVal = (FinalResultModel)obj.Invoke("ProcessAllPotentialCandidatePhoneNumbers", new object[] { _cookie, foundRecord });

            //Assert
            Assert.AreEqual(retVal.OutputResultSource, correctResult.OutputResultSource);
            Assert.AreEqual(retVal.OutputModelResult.Id, correctResult.OutputModelResult.Id);
        }

        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessAllPotentialCandidatePhoneNumbers_WithCookie_WithFoundPhoneNumberWithPersist_ReturnsFoundRecordAndSetCookie()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel() { DefaultPhoneNumber = "0800 000 0001", DefaultCampaignQueryStringKey = "fsource", DefaultPersistDurationInDays = 32 };
            dataModel.CampaignDetail = new List<CampaignDetail>() { new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" } };
            var testPhoneManagerData = dataModel.ToXmlString();

            var _dataProvider = MockProviders.Repository(testPhoneManagerData);

            var foundRecord = new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", OverwritePersistingItem = true };

            var _cookie = new CookieHolder()
            {
                Model =
                new OutputModel()
                {
                    Id = "1202",
                    TelephoneNumber = "9999 999 9999"
                }
            };

            // generate the required result
            var correctResult = new FinalResultModel()
            {
                OutputCookieHolder = new CookieHolder() { Model = 
                    new OutputModel()
                    {
                        Id = "1201",
                        TelephoneNumber = "0800 123 4567"
                    }
                },
                OutputModelResult = new OutputModel()
                {
                    Id = "1201",
                    TelephoneNumber = "0800 123 4567"
                },
                OutputResultSource = OutputSource.FoundRecordFromCriteria
            };

            CampaignPhoneManagerApp target = new CampaignPhoneManagerApp(null, _dataProvider, null, null, null, null);
            PrivateObject obj = new PrivateObject(target);

            //Act
            FinalResultModel retVal = (FinalResultModel)obj.Invoke("ProcessAllPotentialCandidatePhoneNumbers", new object[] { _cookie, foundRecord });

            //Assert
            Assert.AreEqual(retVal.OutputResultSource, correctResult.OutputResultSource);
            Assert.AreEqual(retVal.OutputModelResult.Id, correctResult.OutputModelResult.Id);
            Assert.AreEqual(retVal.OutputCookieHolder.Model.Id, correctResult.OutputCookieHolder.Model.Id);
        }

        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessRequest_WithNoInputsNoDefaultTelephoneNumber_ReturnsLastResortPhoneNumber()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel();
            dataModel.CampaignDetail =
                   new List<CampaignDetail>()
                   {
                       new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" },
                       new CampaignDetail() { Id = "1202", TelephoneNumber = "0800 000 1111", CampaignCode = "testcode2" }
                   };
            var testPhoneManagerData = dataModel.ToXmlString();

            var AppParamHolder = new MockProviders.CampaignPhoneManagerAppParamHolder
                (
                    new CookieHolder(), // cookie
                    testPhoneManagerData, // repository data
                    new NameValueCollection(), // querystring
                    "", // referrer
                    new OutputModel(), // session
                    "" // umbraco current page id
                );

            // generate the required result

            CampaignPhoneManagerApp app = new CampaignPhoneManagerApp(AppParamHolder.CookieProvider, AppParamHolder.RepositoryProvider, AppParamHolder.QueryStringProvider, AppParamHolder.ReferrerProvider, AppParamHolder.SessionProvider, AppParamHolder.UmbracoProvider);

            //Act
            OutputModel actualResult = app.ProcessRequest();

            //Assert
            Assert.IsNull(actualResult.Id);
            Assert.AreEqual(AppConstants.LastResortPhoneNumberPlaceholder, actualResult.TelephoneNumber);
        }

        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessRequest_WithNoInputsWithDefaultTelephoneNumber_ReturnsDefaultTelephoneNumber()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel() { DefaultPhoneNumber = "0800 000 0001", DefaultCampaignQueryStringKey = "fsource", DefaultPersistDurationInDays = 32 };
            dataModel.CampaignDetail =
                   new List<CampaignDetail>()
                   {
                       new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" },
                       new CampaignDetail() { Id = "1202", TelephoneNumber = "0800 000 1111", CampaignCode = "testcode2" }
                   };
            var testPhoneManagerData = dataModel.ToXmlString();

            var AppParamHolder = new MockProviders.CampaignPhoneManagerAppParamHolder
                (
                    new CookieHolder(), // cookie
                    testPhoneManagerData, // repository data
                    new NameValueCollection(), // querystring
                    "", // referrer
                    new OutputModel(), // session
                    "" // umbraco current page id
                );

            // generate the required result

            CampaignPhoneManagerApp app = new CampaignPhoneManagerApp(AppParamHolder.CookieProvider, AppParamHolder.RepositoryProvider, AppParamHolder.QueryStringProvider, AppParamHolder.ReferrerProvider, AppParamHolder.SessionProvider, AppParamHolder.UmbracoProvider);

            //Act
            OutputModel actualResult = app.ProcessRequest();

            //Assert
            Assert.IsNull(actualResult.Id);
            Assert.AreEqual("0800 000 0001", actualResult.TelephoneNumber);
        }

        [TestMethod]
        public void CampaignPhoneManagerApp_ProcessRequest_WithCookie_WithFoundPhoneNumberWithPersist_ReturnsFoundRecordAndSetCookie()
        {
            // Arrange
            // generate test data
            var dataModel = new CampaignPhoneManagerModel() { DefaultPhoneNumber = "0800 000 0001", DefaultCampaignQueryStringKey = "fsource", DefaultPersistDurationInDays = 32 };
            dataModel.CampaignDetail =
                   new List<CampaignDetail>()
                   {
                       new CampaignDetail() { Id = "1201", TelephoneNumber = "0800 123 4567", CampaignCode = "testcode" },
                       new CampaignDetail() { Id = "1202", TelephoneNumber = "0800 000 1111", CampaignCode = "testcode2" }
                   };
            var testPhoneManagerData = dataModel.ToXmlString();

            var AppParamHolder = new MockProviders.CampaignPhoneManagerAppParamHolder
                (
                    new CookieHolder(), // cookie
                    testPhoneManagerData, // repository data
                    new NameValueCollection() { {"fsource", "testcode2" }, { "dummykey", "dummyvalue" } }, // querystring
                    "", // referrer
                    new OutputModel(), // session
                    "" // umbraco current page id
                );

            // generate the required result
            CampaignPhoneManagerApp app = new CampaignPhoneManagerApp(AppParamHolder.CookieProvider, AppParamHolder.RepositoryProvider, AppParamHolder.QueryStringProvider, AppParamHolder.ReferrerProvider, AppParamHolder.SessionProvider, AppParamHolder.UmbracoProvider);

            //Act
            OutputModel actualResult = app.ProcessRequest();

            //Assert
            Assert.AreEqual("1202", actualResult.Id);
            Assert.AreEqual("0800 000 1111", actualResult.TelephoneNumber);
        }
    }
}