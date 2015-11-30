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
            Console.WriteLine("Do you want to use OAuth authorization?");
            Console.WriteLine("Insert [Y] to use and just press [Enter] for none.");
            
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer) || 
                !string.Equals(answer, "y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Skipping Authorization Flow.");
                return true;
            }

            var scopes = string.Join("%20", ApiConfig.OAuthScopes.Split(','));
            string authUri = string.Format("{0}?client_id={1}&scope={2}&response_type=code&redirect_uri={3}",
                ApiConfig.OAuthAuthorizeUri, ApiConfig.ClientId, scopes, ApiConfig.RedirectUri);

            // Open the default Brower with Uri
            System.Diagnostics.Process.Start(authUri);

            Console.WriteLine("Copy the received Uri from the Authorization Flow.");
            Console.WriteLine("Insert the received {code} value and press [Enter]:");

            string code = Console.ReadLine();

            if (string.IsNullOrEmpty(code))
            {
                Console.WriteLine("No input was given. Cannot continue with OAuth authorization.");
                return false;
            }

            string accessToken = await apiClient.GetAccessToken(code, ApiConfig.ClientId, ApiConfig.ClientSecret, ApiConfig.RedirectUri);
            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("Getting the Access Token failed.");
                return false;
            }
            else
            {
                Console.WriteLine("Received Access Token: {0}", accessToken);
            }

            apiClient.AccessToken = accessToken;

            return true;
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
                Console.WriteLine("Error occured: " + ex.Message);
            }

            // Read line, so that the process is not closed
            Console.WriteLine("Press [Enter] to exit");
            Console.ReadLine();
        }

        static async Task ExecuteApiCalls()
        {
            // If no accessToken is received, cannot continue
            if (string.IsNullOrEmpty(apiClient.AccessToken))
            {
                Console.WriteLine("No Access Token set. Cannot continue! \n");
                return;
            }

            await ShowAuctions();

            await ShowBids();

            await ShowInvestements();

            await ShowSecondMarketItems();
        }

        private static async Task ShowSecondMarketItems()
        {
            int pageNr = 1;
            int pageSize = 1000;
            int totalCount = -1;
            var ratings = new List<string> { "EE", "FI" };
            var secondMarketItems = new List<SecondMarketItem>();

            while (totalCount < 0 || pageNr < (totalCount / pageSize))
            {
                var result = await apiClient.GetSecondMarketItems(pageNr, pageSize, -80, ratings);
                if (result == null)
                    break;

                var items = result != null ? result.Payload : null;
                totalCount = result.TotalCount;

                items
                    .Where(i => i.Price <= 3)
                    .ToList()
                    .ForEach(item => 
                        {
                            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                            Console.WriteLine("Id={4}, Amount={0}, Price={1}, XIRR={2}, LateAmount={3}, Discount={5}",
                                item.Amount, item.Price, item.Xirr, item.LateAmountTotal, item.Id, item.DesiredDiscountRate);
                        });

                pageNr++;
            }
            /*
            if (secondMarketItems != null && secondMarketItems.Count > 0)
            {
                Console.WriteLine("\n\nFound secondary market items: \n");

                foreach (SecondMarketItem item in secondMarketItems)
                {
                    Console.WriteLine("Id={4}\nAmount={0}, Price={1}, XIRR={2}, LateAmount={3}, Discount={5}",
                        item.Amount, item.Price, item.Xirr, item.LateAmountTotal, item.Id, item.DesiredDiscountRate);
                }
            }
            else
            {
                Console.WriteLine("\nNo secondary market items found.\n");
            }*/
        }

        private static async Task ShowInvestements()
        {
            // Get Investments
            var investmentsResult = await apiClient.GetInvestments();
            var investments = investmentsResult != null ? investmentsResult.Payload : null;

            if (investments != null && investments.Count > 0)
            {
                Console.WriteLine("\n\nFound Investments: \n");

                foreach (Investment investment in investments)
                {
                    Console.WriteLine("Amount={0}, Interest={1}, LateAmount={2}, UserName={3}",
                        investment.Amount, investment.Interest, investment.LateAmountTotal, investment.UserName);
                }
            }
            else
            {
                Console.WriteLine("\nNo Investments found.\n");
            }
        }

        private static async Task ShowBids()
        {
            // Get list of pending API bids (bid status = 0 as example)
            var bidsResult = await apiClient.GetBids(1, 100);
            var bidList = bidsResult != null ? bidsResult.Payload : null;

            if (bidList != null && bidList.Count > 0)
            {
                Console.WriteLine("\nFound Bids:\n");
                foreach (var bid in bidList)
                {
                    Console.WriteLine("Requested={0}, Amount={1}, Actual={2}, Status={3}", 
                        bid.BidRequestedDate, bid.RequestedBidAmount, bid.ActualBidAmount, bid.StatusCode);
                }
            }
            else
            {
                Console.WriteLine("\nNo bids found.\n");
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
                Console.WriteLine("\n\nFound Auctions: \n");

                foreach (Auction auction in auctions)
                {
                    Console.WriteLine("AuctionId={0}, Amount={1}, Rating={2}", auction.AuctionId,
                        auction.AppliedAmount, auction.Rating);
                }
            }
            else
            {
                Console.WriteLine("\nNo auctions found.\n");
            }

            // Check if there are any active auctions
            if (auctions.Count > 0)
            {
                // Get first auction for bidding
                int auctionIndex = new Random().Next(auctions.Count);
                Guid auctionId = auctions[auctionIndex].AuctionId;
                var auction = await apiClient.GetAuction(auctionId);

                if (auction != null)
                    Console.WriteLine("AuctionId={0}, Amount={1}, Rating={2}", auction.AuctionId, auction.AppliedAmount, auction.Rating);

                // Construct 2 bid requests for this auction
                var bids = new List<Bid> { new Bid(auction.AuctionId, 0), new Bid(auction.AuctionId, 0, 0) };
                
                // send the bid request
                if (await apiClient.BidOnAuction(bids))
                    Console.WriteLine("Bid Request Succeeded! \n");
            }
        } 

    }
}
