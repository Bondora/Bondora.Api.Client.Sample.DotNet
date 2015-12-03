using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bondora.Api.Client.Sample.DotNet.Models;

namespace Bondora.Api.Client.Sample.DotNet
{
    class Program
    {
        private static ApiClient apiClient;
        
        static void Main(string[] args)
        {
            apiClient = new ApiClient(ApiConfig.ApiBaseUri);

            RunAsync().Wait();
        }

        static async Task<bool> Authorize()
        {
            Logger.LogInfo("Do you want to use OAuth authorization?");
            Logger.LogInfo("Insert [Y] to use and just press [Enter] for none.");
            
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer) || 
                !string.Equals(answer, "y", StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.LogWarning("Skipping Authorization Flow.");
                return true;
            }

            var scopes = string.Join("%20", ApiConfig.OAuthScopes.Split(','));
            string authUri = string.Format("{0}?client_id={1}&scope={2}&response_type=code&redirect_uri={3}",
                ApiConfig.OAuthAuthorizeUri, ApiConfig.ClientId, scopes, ApiConfig.RedirectUri);

            // Open the default Brower with Uri
            System.Diagnostics.Process.Start(authUri);

            Logger.LogInfo("Copy the received Uri from the Authorization Flow.");
            Logger.LogInfo("Insert the received {{code}} value and press [Enter]:");

            string code = Console.ReadLine();

            if (string.IsNullOrEmpty(code))
            {
                Logger.LogWarning("No input was given. Cannot continue with OAuth authorization.");
                return false;
            }

            var accessTokenResult = await apiClient.GetAccessTokenByCode(code, 
                ApiConfig.ClientId, ApiConfig.ClientSecret, ApiConfig.RedirectUri);

            if (accessTokenResult == null)
            {
                Logger.LogError("Getting the Access Token failed.");
                return false;
            }
            else
            {
                Logger.LogSuccess("Received Access Token: {0}", accessTokenResult.access_token);
            }

            apiClient.AccessToken = accessTokenResult.access_token;
            apiClient.RefreshToken = accessTokenResult.refresh_token;
            apiClient.TokenValidUntilUtc = accessTokenResult.valid_until > 0
                ? (DateTime?) GetDateTimeFromUnixTime(accessTokenResult.valid_until)
                : null;

            return true;
        }

        private static DateTime GetDateTimeFromUnixTime(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(unixTime);
        }

        static async Task RunAsync()
        {
            try
            {
                bool success = await Authorize();
                if (success)
                    await ExecuteApiCalls();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occured: " + ex.Message);
            }

            // Read line, so that the process is not closed
            Logger.LogInfo("Press [Enter] to exit");
            Console.ReadLine();
        }

        static async Task ExecuteApiCalls()
        {
            // If no accessToken is received, cannot continue
            if (string.IsNullOrEmpty(apiClient.AccessToken))
            {
                Logger.LogWarning("No Access Token set. Cannot continue!");
                return;
            }

            if (apiClient.RefreshToken != null)
            {
                Logger.LogInfo("Refresh token is set. Trying to get new access token for the refresh token.");

                if (!await GetNewAccessTokenFromRefreshToken())
                    Logger.LogError("Could not get new access token for refresh token! \n");
            }

            await ShowAuctions();

            await ShowBids();

            await ShowInvestements();

            await ShowSecondMarketItems();

            Logger.LogInfo("Trying to Revoke the access token.");
            if (!await RevokeAccessToken())
            {
                Logger.LogError("Revoking the access token failed!");
            }
            else
            {
                Logger.LogSuccess("Successfully Revoke the access token.");
            }
        }

        private static async Task<bool> RevokeAccessToken()
        {
            return await apiClient.RevokeAccessToken();
        }

        private static async Task<bool> GetNewAccessTokenFromRefreshToken()
        {
            var result = await apiClient.GetAccessTokenByRefreshToken(apiClient.RefreshToken, 
                ApiConfig.ClientId, ApiConfig.ClientSecret, ApiConfig.RedirectUri);

            if (result == null)
            {
                Logger.LogError("Getting the Access Token failed.");
                return false;
            }
            else
            {
                Logger.LogSuccess("Received New Access Token: {0}", result.access_token);
            }

            apiClient.AccessToken = result.access_token;
            apiClient.TokenValidUntilUtc = result.valid_until > 0
                ? (DateTime?)GetDateTimeFromUnixTime(result.valid_until)
                : null;

            return true;
        }

        private const bool FilterSecondaryMarketItems = false;
        private static async Task ShowSecondMarketItems()
        {
            List<SecondMarketItem> secondMarketItems = null;

            if (FilterSecondaryMarketItems)
            {
                int pageNr = 1;
                int pageSize = 1000;
                int totalCount = -1;
                var ratings = new List<string> {"EE", "FI"};
                secondMarketItems = new List<SecondMarketItem>();

                while (totalCount < 0 || pageNr < (totalCount/pageSize))
                {
                    var result = await apiClient.GetSecondMarketItems(pageNr, pageSize, -80, ratings);
                    if (result == null)
                        break;

                    if (pageNr == 1)
                        Logger.LogSuccess("\n\nFound Secondary Market items: \n");

                    var items = result != null ? result.Payload : null;
                    totalCount = result.TotalCount;

                    items
                        .Where(i => i.Price <= 3)
                        .ToList()
                        .ForEach(item =>
                        {
                            //Logger.LogInfo(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                            Logger.LogInfo("Id={4}, Amount={0}, Price={1}, XIRR={2}, LateAmount={3}, Discount={5}",
                                item.Amount, item.Price, item.Xirr, item.LateAmountTotal, item.Id,
                                item.DesiredDiscountRate);
                        });

                    pageNr++;
                }
            }
            else
            {
                var result = await apiClient.GetSecondMarketItems(1, 10);

                if (result != null && result.Payload != null)
                    secondMarketItems = result.Payload.ToList();

                if (secondMarketItems != null && secondMarketItems.Count > 0)
                {
                    Logger.LogSuccess("\n\nFound secondary market items: \n");

                    foreach (SecondMarketItem item in secondMarketItems)
                    {
                        Logger.LogInfo("Id={4}\nAmount={0}, Price={1}, XIRR={2}, LateAmount={3}, Discount={5}",
                            item.Amount, item.Price, item.Xirr, item.LateAmountTotal, item.Id, item.DesiredDiscountRate);
                    }
                }
                else
                {
                    Logger.LogWarning("\nNo secondary market items found.\n");
                }
            }
        }

        private static async Task ShowInvestements()
        {
            // Get Investments
            var investmentsResult = await apiClient.GetInvestments();
            var investments = investmentsResult != null ? investmentsResult.Payload : null;

            if (investments != null && investments.Count > 0)
            {
                Logger.LogSuccess("\n\nFound Investments: \n");

                foreach (Investment investment in investments)
                {
                    Logger.LogInfo("Amount={0}, Interest={1}, LateAmount={2}, UserName={3}",
                        investment.Amount, investment.Interest, investment.LateAmountTotal, investment.UserName);
                }
            }
            else
            {
                Logger.LogWarning("\nNo Investments found.\n");
            }
        }

        private static async Task ShowBids()
        {
            // Get list of pending API bids (bid status = 0 as example)
            var bidsResult = await apiClient.GetBids(1, 100);
            var bidList = bidsResult != null ? bidsResult.Payload : null;

            if (bidList != null && bidList.Count > 0)
            {
                Logger.LogSuccess("\nFound Bids:\n");
                foreach (var bid in bidList)
                {
                    Logger.LogInfo("Requested={0}, Amount={1}, Actual={2}, Status={3}", 
                        bid.BidRequestedDate, bid.RequestedBidAmount, bid.ActualBidAmount, bid.StatusCode);
                }
            }
            else
            {
                Logger.LogWarning("\nNo bids found.\n");
            }
        }

        private static async Task ShowAuctions()
        {
            // Get list auctions
            var ratings = new List<string> { "AA", "A", "B" };
            var auctionsResult = await apiClient.GetAuctions(1, 10, ratings);
            var auctions = auctionsResult != null ? auctionsResult.Payload : null;

            if (auctions == null)
                return;

            if (auctions.Count > 0)
            {
                Logger.LogSuccess("\n\nFound Auctions: \n");

                foreach (Auction auction in auctions)
                {
                    Logger.LogInfo("AuctionId={0}, Amount={1}, Rating={2}", auction.AuctionId,
                        auction.AppliedAmount, auction.Rating);
                }
            }
            else
            {
                Logger.LogWarning("\nNo auctions found.\n");
            }

            // Check if there are any active auctions
            if (auctions.Count > 0)
            {
                // Get first auction for bidding
                int auctionIndex = new Random().Next(auctions.Count);
                Guid auctionId = auctions[auctionIndex].AuctionId;
                var auction = await apiClient.GetAuction(auctionId);

                if (auction != null)
                    Logger.LogInfo("AuctionId={0}, Amount={1}, Rating={2}", auction.AuctionId, auction.AppliedAmount, auction.Rating);

                // Construct 2 bid requests for this auction
                var bids = new List<Bid> { new Bid(auction.AuctionId, 0), new Bid(auction.AuctionId, 0, 0) };
                
                // send the bid request
                if (await apiClient.BidOnAuction(bids))
                    Logger.LogSuccess("Bid Request Succeeded! \n");
                else
                    Logger.LogError("Bid Request Failed! \n");
            }
        } 

    }
}
