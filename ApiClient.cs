using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bondora.Api.Client.Sample.DotNet.Models;
using Bondora.Api.Client.Sample.DotNet.Models.OAuth;

namespace Bondora.Api.Client.Sample.DotNet
{
    public class ApiClient
    {
        public string BaseUri { get; private set; }
        public string AccessToken { get; set; }

        public ApiClient(string baseUri)
        {
            BaseUri = baseUri;
        }

        #region Auctions
        public async Task<ApiResultAuctions> GetAuctions(int pageNr = 1, int pageSize = 10, List<string> ratings = null)
        {
            string queryParams = string.Empty;
            if (ratings != null && ratings.Count > 0)
            {
                var getParams = new NameValueCollection();
                ratings.ForEach(rating => getParams.Add("Ratings", rating));
                queryParams = GetQueryString(getParams);
            }

            using (var client = InitializeHttpClientWithAccessToken(AccessToken))
            {
                HttpResponseMessage auctionListResponse = await client.GetAsync(string.Format("api/v1/auctions?PageNr={0}&PageSize={1}&{2}", pageNr, pageSize, queryParams));
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    return await auctionListResponse.Content.ReadAsAsync<ApiResultAuctions>();
                }
                else
                {
                    Console.WriteLine("Getting list of auctions failed, Reason: " + await auctionListResponse.Content.ReadAsStringAsync());
                }
            }
            return null;
        }
        #endregion

        #region GetAuction
        public async Task<Auction> GetAuction(Guid auctionId)
        {
            using (var client = InitializeHttpClientWithAccessToken(AccessToken))
            {
                HttpResponseMessage auctionListResponse = await client.GetAsync("api/v1/auction/" + auctionId);
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n\nAuction chosen: \n");
                    var listAuctionResult = await auctionListResponse.Content.ReadAsAsync<ApiResultAuction>();
                    return listAuctionResult.Payload;

                }
                else
                {
                    Console.WriteLine("Getting auction failed, Reason : " + await auctionListResponse.Content.ReadAsStringAsync());
                }
                return null;
            }
        }
        #endregion

        #region Bid On Auction
        public async Task<bool> BidOnAuction(List<Bid> bids)
        {
            var bidRequest = new BidRequest(bids);

            using (var client = InitializeHttpClientWithAccessToken(AccessToken))
            {
                HttpResponseMessage bidResponse = await client.PostAsJsonAsync("api/v1/bid", bidRequest);
                if (bidResponse.StatusCode == HttpStatusCode.Accepted)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Bid Request failed, Reason : " + await bidResponse.Content.ReadAsStringAsync());
                }
                return false;
            }
        }
        #endregion

        #region Bids
        public async Task<ApiResultBids> GetBids(int pageNr = 1, int pageSize = 10, int? bidStatus = null,
            DateTime? startDate = null, DateTime? endDate = null, Guid? partyId = null)
        {
            using (var client = InitializeHttpClientWithAccessToken(AccessToken))
            {
                // Add GET parameters
                var getParams = new NameValueCollection();
                if (bidStatus.HasValue)
                    getParams.Add("bidStatus", bidStatus.Value.ToString());
                if (startDate.HasValue)
                    getParams.Add("startDate", startDate.Value.ToString("u"));
                if (endDate.HasValue)
                    getParams.Add("endDate", endDate.Value.ToString("u"));
                if (partyId.HasValue)
                    getParams.Add("partyId", partyId.Value.ToString());

                HttpResponseMessage bidListResponse = await client.GetAsync(string.Format("api/v1/bids?PageNr={0}&PageSize={1}&{2}", pageNr, pageSize, GetQueryString(getParams)));

                if (bidListResponse.IsSuccessStatusCode)
                {
                    return await bidListResponse.Content.ReadAsAsync<ApiResultBids>();
                }
                else
                {
                    Console.WriteLine("Getting list of bids failed, Reason : " + await bidListResponse.Content.ReadAsStringAsync());
                }
                return null;
            }
        }
        #endregion

        #region Investments
        public async Task<ApiResultInvestments> GetInvestments(int pageNr = 1, int pageSize = 10)
        {
            using (var client = InitializeHttpClientWithAccessToken(AccessToken))
            {
                HttpResponseMessage auctionListResponse = await client.GetAsync(string.Format("api/v1/account/investments?PageNr={0}&PageSize={1}", pageNr, pageSize));
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    return await auctionListResponse.Content.ReadAsAsync<ApiResultInvestments>();
                }
                else
                {
                    Console.WriteLine("Getting list of investments failed, Reason : " + await auctionListResponse.Content.ReadAsStringAsync());
                }
            }
            return null;
        }
        #endregion

        #region SecondaryMarket
        public async Task<ApiResultSecondMarket> GetSecondMarketItems(int pageNr = 1, int pageSize = 10,
            decimal desiredDiscountRateMax = 100, List<string> countries = null)
        {
            string queryParams = string.Empty;
            if (countries != null && countries.Count > 0)
            {
                var getParams = new NameValueCollection();
                countries.ForEach(country => getParams.Add("Countries", country));
                queryParams = GetQueryString(getParams);
            }

            using (var client = InitializeHttpClientWithAccessToken(AccessToken))
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/secondarymarket?PageNr={0}&PageSize={1}&DesiredDiscountRateMax={2}&Countries={3}", pageNr, pageSize, desiredDiscountRateMax, queryParams));
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ApiResultSecondMarket>();
                }
                else if ((int)response.StatusCode == 429 && response.Headers.Contains("Retry-After"))
                {
                    var retryAfter = response.Headers.GetValues("Retry-After").First();
                    int seconds;
                    if (int.TryParse(retryAfter, out seconds))
                    {
                        // Wait for N seconds efore trying again
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                        return await GetSecondMarketItems(pageNr, pageSize);
                    }
                    else
                    {
                        Console.WriteLine("Too many requests made. Could not parse the 'Retry-After' header value '{0}'", retryAfter);
                    }
                }
                else
                {
                    Console.WriteLine("Getting list of secondary market items failed, Reason: {0}", await response.Content.ReadAsStringAsync());
                }
            }
            return null;
        }
        #endregion

        #region OAuth Access Token
        public async Task<string> GetAccessToken(string code, string clientId, string clientSecret, string redirectUri)
        {
            var request = new AccessTokenRequest
            {
                code = code,
                client_id = clientId,
                client_secret = clientSecret,
                grant_type = "authorization_code",
                redirect_uri = redirectUri
            };

            using (var client = InitializeHttpClientWithBaseUri())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("oauth/access_token", request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AccessTokenResult>();
                    return result != null ? result.access_token : null;
                }
                else
                {
                    Console.WriteLine("Access token request failed, Reason: " + await response.Content.ReadAsStringAsync());
                }
                return null;
            }
        }
        #endregion

        #region Initialization

        private static HttpClient InitializeHttpClientWithBaseUri()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiConfig.ApiBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static HttpClient InitializeHttpClientWithAccessToken(string accessToken)
        {
            var client = InitializeHttpClientWithBaseUri();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return client;
        }

        #endregion

        #region Utils

        public static string GetQueryString(NameValueCollection source)
        {
            if (source.Count == 0)
                return string.Empty;

            return String.Join("&", source.AllKeys
                .SelectMany(key => source.GetValues(key)
                    .Select(value => String.Format("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value))))
                .ToArray());
        }

        #endregion
    }
}
