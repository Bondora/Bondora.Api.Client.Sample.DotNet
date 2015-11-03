using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bondora.Api.Client.Sample.DotNet.Models;
using Bondora.Api.Client.Sample.DotNet.Models.Enums;

namespace Bondora.Api.Client.Sample.DotNet
{
    class Program
    {
        private const string ApiBaseUri = "http://localhost:57390/"; // Base Uri for the API
        private const string ApiAccessToken = "koUqKoRAF7aMUB4jLvSIgWbSAVDpXO24XckdcNF7dvvMHNoz"; // OAuth Access Token

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            try
            {
                // If no accessToken is received, cannot continue
                if (string.IsNullOrEmpty(ApiAccessToken))
                {
                    Console.WriteLine("No Access Token set. Cannot continue! \n");
                    return;
                }

                // Get list auctions
                var auctions = await GetAuctions();

                foreach (Auction auction in auctions)
                {
                    Console.WriteLine("AuctionId={0}, Amount={1}, Rating={2}", auction.AuctionId, auction.AppliedAmount, auction.Rating);
                }

                // Check if there are any active auctions
                if (auctions.Count > 0)
                {
                    // Get first auction for bidding
                    int auctionIndex = new Random().Next(auctions.Count);
                    Guid auctionId = auctions[auctionIndex].AuctionId;
                    var auction = await GetAuction(auctionId);

                    // Construct 2 bid requests for this auction
                    var bids = new List<Bid> { new Bid(auction.AuctionId, 100M), new Bid(auction.AuctionId, 200M, 100M) };
                    var bidRequest = new BidRequest(bids);

                    // send the bid request
                    if (await BidOnAuction(bidRequest))
                        Console.WriteLine("Bid Request Succeeded! \n");
                }

                // Get list of pending API bids (bid status = 0 as example)
                var bidList = await GetBids((int)ApiBidStatusCode.Pending);

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

                // Get Investments
                var investments = await GetInvestments();

                foreach (Investment investment in investments)
                {
                    Console.WriteLine("LoanId={0}, LoanPartId={1}, Amount={2}, Interest={3}", investment.LoanId, investment.LoanPartId, investment.Amount, investment.Interest);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
            }

            // Read line, so that the process is not closed
            Console.WriteLine("Press [Enter] to exit");
            System.Console.ReadLine();
        }

        #region Auctions
        static async Task<IList<Auction>> GetAuctions()
        {
            using (var client = InitializeHttpClientWithAccessToken(ApiAccessToken))
            {
                HttpResponseMessage auctionListResponse = await client.GetAsync("api/v1/auctions");
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n\nFound Auctions For Bidding: \n");
                    var listAuctionResult = await auctionListResponse.Content.ReadAsAsync<ApiResultAuctions>();
                    
                    return listAuctionResult.Payload;
                }
                else
                {
                    throw new Exception("Getting list of auctions failed, Reason : " + await auctionListResponse.Content.ReadAsStringAsync());
                }
            }
            return null;
        }
        #endregion

        #region GetAuction
        static async Task<Auction> GetAuction(Guid auctionId)
        {
            var auctionFound = new Auction();
            using (var client = InitializeHttpClientWithAccessToken(ApiAccessToken))
            {
                HttpResponseMessage auctionListResponse = await client.GetAsync("api/v1/auction/" + auctionId);
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n\nAuction chosen: \n");
                    var listAuctionResult = await auctionListResponse.Content.ReadAsAsync<ApiResultAuction>();
                    auctionFound = listAuctionResult.Payload;
                    Console.WriteLine("AuctionId={0}, Amount={1}, Rating={2}", auctionFound.AuctionId, auctionFound.AppliedAmount, auctionFound.Rating);
                }
                else
                {
                    throw new Exception("Getting auction failed, Reason : " + await auctionListResponse.Content.ReadAsStringAsync());
                }
                return auctionFound;
            }
        }
        #endregion

        #region Bid On Auction
        static async Task<bool> BidOnAuction(BidRequest bidRequest)
        {
            using (var client = InitializeHttpClientWithAccessToken(ApiAccessToken))
            {
                HttpResponseMessage bidResponse = await client.PostAsJsonAsync("api/v1/bid", bidRequest);
                if (bidResponse.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new Exception("Bid Request failed, Reason : " + await bidResponse.Content.ReadAsStringAsync());
                }
                return true;
            }
        }
        #endregion

        #region Bids
        static async Task<IList<BidSummary>> GetBids(int? bidStatus = null, DateTime? startDate = null, DateTime? endDate = null, Guid? partyId = null)
        {
            var listBidResult = new ApiResultBids();

            using (var client = InitializeHttpClientWithAccessToken(ApiAccessToken))
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

                HttpResponseMessage bidListResponse = await client.GetAsync("api/v1/bids?" + GetQueryString(getParams));

                if (bidListResponse.IsSuccessStatusCode)
                {
                    listBidResult = await bidListResponse.Content.ReadAsAsync<ApiResultBids>();
                }
                else
                {
                    throw new Exception("Getting list of bids failed, Reason : " + await bidListResponse.Content.ReadAsStringAsync());
                }
                return listBidResult.Payload;
            }
        }
        #endregion

        #region Investments
        static async Task<IList<Investment>> GetInvestments()
        {
            using (var client = InitializeHttpClientWithAccessToken(ApiAccessToken))
            {
                HttpResponseMessage auctionListResponse = await client.GetAsync("api/v1/account/investments");
                if (auctionListResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n\nFound Investments: \n");
                    var investmentsResult = await auctionListResponse.Content.ReadAsAsync<ApiResultInvestments>();
                    
                    return investmentsResult.Payload;
                }
                else
                {
                    throw new Exception("Getting list of investments failed, Reason : " + await auctionListResponse.Content.ReadAsStringAsync());
                }
            }
            return null;
        }
        #endregion

        #region Initialization

        private static HttpClient InitializeHttpClientWithBaseUri()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        static HttpClient InitializeHttpClientWithAccessToken(string accessToken)
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
