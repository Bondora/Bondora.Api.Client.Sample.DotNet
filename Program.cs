using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bondora.Api.Client.Sample.DotNet.Models;
using Bondora.Api.Client.Sample.DotNet.Models.Enums;
using Newtonsoft.Json;

namespace Bondora.Api.Client.Sample.DotNet
{
    class Program
    {
        private const string ApiUsername = "apitestuser@bondora.com"; // Change to your Bondora username
        private const string ApiPassword = "ap1t3stpa$$w0rd"; // Change to your Bondora password
        private const string ApiBaseUri = "https://api-sandbox.bondora.com/"; // Base Uri for the API

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            try
            {
                //get token for login
                var token = await LoginToApi();

                // If no token is received, cannot continue
                if (string.IsNullOrEmpty(token))
                    return;

                // Get list auctions
                var auctions = await ListAuctions(token);

                // Check if there are any active auctions
                if (auctions.Count > 0)
                {
                    // Get first auction for bidding
                    int auctionIndex = new Random().Next(auctions.Count);
                    Guid auctionId = auctions[auctionIndex].AuctionId;
                    var auction = await GetAuction(token, auctionId);

                    // Construct 2 bid requests for this auction
                    var bids = new List<Bid> { new Bid(auction.AuctionId, 100M), new Bid(auction.AuctionId, 200M, 100M) };
                    var bidRequest = new BidRequest(bids);

                    // send the bid request
                    if (await BidOnAuction(token, bidRequest))
                        Console.WriteLine("Bid Request Succeeded! \n");
                }

                // Get list of pending API bids (bid status = 0 as example)
                var bidList = await ListBids(token, (int)ApiBidStatusCode.Pending);

                if (bidList.Count > 0)
                {
                    Console.WriteLine("\nFound Bids:\n");
                    foreach (var bid in bidList)
                    {
                        Console.WriteLine("AuctionId={0}, Amount={1}, Actual={2}, Status={3}", bid.AuctionId, bid.RequestedBidAmount, bid.ActualBidAmount, bid.Status);
                    }
                }
                else
                {
                    Console.WriteLine("\nNo bids found.\n");
                }

                // Logout from API
                if (await LogoutFromApi(token))
                    Console.WriteLine("\nLogged out from API!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
            }

            // Read line, so that the process is not closed
            Console.WriteLine("Press [Enter] to exit");
            System.Console.ReadLine();
        }

        #region Login
        static async Task<string> LoginToApi()
        {
            using (var client = InitializeHttpClientWithCredentials())
            {
                var content = new StringContent(string.Empty);
                var loginResponse = await client.PostAsync("api/v1/login", content);

                if (loginResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Login failed, Reason : " + loginResponse.Content.ReadAsStringAsync().Result);
                }

                Console.WriteLine("Login Succeeded! Below is your token information:\n");
                var loginResult = JsonConvert.DeserializeObject<ApiResultAuthentication>(loginResponse.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Token:" + loginResult.Payload.Token);
                Console.WriteLine("Valid Until:" + loginResult.Payload.ValidUntil);
                if (loginResult.Payload.UserOrganizations.Count > 0)
                {
                    Console.WriteLine("Represented Organizations:\n");
                    foreach (var party in loginResult.Payload.UserOrganizations)
                    {
                        Console.WriteLine("OrganizationId={0}, Name={1}, IsReadonly={2}, ActiveToDate={3}", party.Id, party.Name, party.IsReadonly, party.ActiveToDate);
                    }
                }
                return loginResult.Payload.Token;
            }
        }
        #endregion

        #region ListAuctions
        static async Task<IList<Auction>> ListAuctions(string token)
        {
            using (var client = InitializeHttpClientWithToken(token))
            {
                var auctionListResponse = await client.GetAsync("api/v1/auctions");
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n\nFound Auctions For Bidding: \n");
                    var listAuctionResult = JsonConvert.DeserializeObject<ApiResultAuctions>(auctionListResponse.Content.ReadAsStringAsync().Result);
                    foreach (var auction in listAuctionResult.Payload)
                    {
                        Console.WriteLine("AuctionId={0}, Amount={1}, Rating={2}", auction.AuctionId, auction.AppliedAmount, auction.Rating);
                    }
                    return listAuctionResult.Payload;
                }
                else
                {
                    throw new Exception("Getting list of auctions failed, Reason : " + auctionListResponse.Content.ReadAsStringAsync().Result);
                }
            }
            return null;
        }
        #endregion

        #region GetAuction
        static async Task<Auction> GetAuction(string token, Guid auctionId)
        {
            var auctionFound = new Auction();
            using (var client = InitializeHttpClientWithToken(token))
            {
                var auctionListResponse = await client.GetAsync("api/v1/auction/" + auctionId);
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n\nAuction chosen: \n");
                    var listAuctionResult = JsonConvert.DeserializeObject<ApiResultAuction>(auctionListResponse.Content.ReadAsStringAsync().Result);
                    auctionFound = listAuctionResult.Payload;
                    Console.WriteLine("AuctionId={0}, Amount={1}, Rating={2}", auctionFound.AuctionId, auctionFound.AppliedAmount, auctionFound.Rating);
                }
                else
                {
                    throw new Exception("Getting auction failed, Reason : " + auctionListResponse.Content.ReadAsStringAsync().Result);
                }
                return auctionFound;
            }
        }
        #endregion

        #region Bid On Auction
        static async Task<bool> BidOnAuction(string token, BidRequest bidRequest)
        {
            using (var client = InitializeHttpClientWithToken(token))
            {
                var content = new StringContent(JsonConvert.SerializeObject(bidRequest), Encoding.UTF8,
                    "application/json");
                var bidResponse = await client.PostAsync("api/v1/bid", content);
                if (bidResponse.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new Exception("Bid Request failed, Reason : " + bidResponse.Content.ReadAsStringAsync().Result);
                }
                return true;
            }
        }
        #endregion

        #region List Bids
        static async Task<IList<BidSummary>> ListBids(string token, int? bidStatus = null, DateTime? startDate = null, DateTime? endDate = null, Guid? partyId = null)
        {
            var listBidResult = new ApiResultBids();

            using (var client = InitializeHttpClientWithToken(token))
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

                var bidListResponse = await client.GetAsync("api/v1/bids?" + GetQueryString(getParams));

                if (bidListResponse.IsSuccessStatusCode)
                {
                    listBidResult = JsonConvert.DeserializeObject<ApiResultBids>(bidListResponse.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    throw new Exception("Getting list of bids failed, Reason : " + bidListResponse.Content.ReadAsStringAsync().Result);
                }
                return listBidResult.Payload;
            }
        }
        #endregion

        #region Logout
        static async Task<bool> LogoutFromApi(string token)
        {
            using (var client = InitializeHttpClientWithToken(token))
            {
                var content = new StringContent(string.Empty);
                var logoutResponse = await client.PostAsync("api/v1/logout", content);

                if (!logoutResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Logout failed, Reason : " + logoutResponse.Content.ReadAsStringAsync().Result);
                }
                return true;
            }
        }
        #endregion

        #region Initialization

        private static HttpClient InitializeHttpClientWithBaseUri()
        {
            var client = new HttpClient {BaseAddress = new Uri(ApiBaseUri)};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static HttpClient InitializeHttpClientWithCredentials()
        {
            var client = InitializeHttpClientWithBaseUri();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                Encoding.ASCII.GetBytes(string.Format("{0}:{1}", ApiUsername, ApiPassword))));
            return client;
        }

        static HttpClient InitializeHttpClientWithToken(string token)
        {
            var client = InitializeHttpClientWithBaseUri();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
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
