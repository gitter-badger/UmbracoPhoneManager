﻿using System.Collections.Generic;
using Wr.Umbraco.CampaignPhoneManager.Models;
using Wr.Umbraco.CampaignPhoneManager.Providers;

namespace Wr.Umbraco.CampaignPhoneManager.Criteria
{
    public class QueryStringCriteria : ICampaignPhoneManagerCriteria
    {
        private IDataProvider _iQueryStringDataProvider;
        private readonly QueryStringProvider _querystringProvider;

        public QueryStringCriteria()
        {
            _querystringProvider = new QueryStringProvider(new HttpContextQueryStringProviderSource());
        }

        public QueryStringCriteria(IDataProvider iQueryStringDataProvider, QueryStringProvider querystringProvider)
        {
            _querystringProvider = querystringProvider;
            _iQueryStringDataProvider = iQueryStringDataProvider;
        }

        public List<CampaignDetail> GetMatchingRecordsFromPhoneManager()
        {
            var cleansedQueryStrings = _querystringProvider.GetCleansedQueryStrings();

            if (cleansedQueryStrings.Count > 0)
            {
                if (_iQueryStringDataProvider == null)
                    _iQueryStringDataProvider = new QueryStringCriteria_DataSource_XPath(cleansedQueryStrings);

                return _iQueryStringDataProvider.GetMatchingRecordsFromPhoneManager();
            }

            return new List<CampaignDetail>();
        }
    }
}