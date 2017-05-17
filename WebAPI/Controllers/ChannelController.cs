using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.MediaServices.Client.DynamicEncryption;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ChannelController : ApiController
    {
        // Read values from the App.config file.
        private static readonly string _mediaServicesAccountName =
            ConfigurationManager.AppSettings["MediaServicesAccountName"];
        private static readonly string _mediaServicesAccountKey =
            ConfigurationManager.AppSettings["MediaServicesAccountKey"];

        // Media Service Channel informations define in App.config file
        private static readonly string _mediaServicesChannelName =
            ConfigurationManager.AppSettings["MediaServicesChannelName"];
        private static readonly string _mediaServicesAssetName =
            ConfigurationManager.AppSettings["MediaServicesAssetName"];
        private static readonly string _mediaServicesProgramName =
            ConfigurationManager.AppSettings["MediaServicesProgramName"];

        // Field for service context.
        private static CloudMediaContext _context = null;
        private static MediaServicesCredentials _cachedCredentials = null;

        private  IChannel channel;

        [HttpGet]
        public WTF Test()
        {
            WTF wtf = new WTF();

            wtf.Id = 1;
            wtf.Name = "Le C# ne respecte rien ni personne... #WTF";

            return wtf;
        }

        [HttpGet]
        public DataChannel Start()
        {

            // Create and cache the Media Services credentials in a static class variable.
            _cachedCredentials = new MediaServicesCredentials(
                            _mediaServicesAccountName,
                            _mediaServicesAccountKey);
            // Used the cached credentials to create CloudMediaContext.
            _context = new CloudMediaContext(_cachedCredentials);


            channel = CreateAndStartChannel();

            // The channel's input endpoint:
            //string ingestUrl = channel.Input.Endpoints.FirstOrDefault().Url.ToString();

            // Use the previewEndpoint to preview and verify 
            // that the input from the encoder is actually reaching the Channel. 
            //string previewEndpoint = channel.Preview.Endpoints.FirstOrDefault().Url.ToString();

            DataChannel dataChannel = new DataChannel();
            dataChannel.UrlIngest = channel.Input.Endpoints.FirstOrDefault().Url.ToString();
            dataChannel.UrlPreview = channel.Preview.Endpoints.FirstOrDefault().Url.ToString();

            // When Live Encoding is enabled, you can now get a preview of the live feed as it reaches the Channel. 
            // This can be a valuable tool to check whether your live feed is actually reaching the Channel. 
            // The thumbnail is exposed via the same end-point as the Channel Preview URL.
            string thumbnailUri = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = channel.Preview.Endpoints.FirstOrDefault().Url.Host,
                Path = "thumbnails/input.jpg"
            }.Uri.ToString();

            // Once you previewed your stream and verified that it is flowing into your Channel, 
            // you can create an event by creating an Asset, Program, and Streaming Locator. 
            IAsset asset = CreateAndConfigureAsset();

            IProgram program = CreateAndStartProgram(channel, asset);

            ILocator locator = CreateLocatorForAsset(program.Asset, program.ArchiveWindowLength);

            return dataChannel;
        }

        [HttpGet]
        public string Stop()
        {
            Cleanup(channel);

            return "Stop";
        }

        public static IChannel CreateAndStartChannel()
        {
            var channelInput = CreateChannelInput();
            var channePreview = CreateChannelPreview();
            var channelEncoding = CreateChannelEncoding();


            ChannelCreationOptions options = new ChannelCreationOptions
            {
                EncodingType = ChannelEncodingType.Standard,
                Name = _mediaServicesChannelName,
                Input = channelInput,
                Preview = channePreview,
                Encoding = channelEncoding
            };

            Log("Creating channel");
            IOperation channelCreateOperation = _context.Channels.SendCreateOperation(options);
            string channelId = TrackOperation(channelCreateOperation, "Channel create");

            IChannel channel = _context.Channels.Where(c => c.Id == channelId).FirstOrDefault();

            Log("Starting channel");
            var channelStartOperation = channel.SendStartOperation();
            TrackOperation(channelStartOperation, "Channel start");

            return channel;
        }

        /// <summary>
        /// Create channel input, used in channel creation options. 
        /// </summary>
        /// <returns></returns>
        private static ChannelInput CreateChannelInput()
        {
            return new ChannelInput
            {
                StreamingProtocol = StreamingProtocol.RTPMPEG2TS,
                AccessControl = new ChannelAccessControl
                {
                    IPAllowList = new List<IPRange>
                    {
                        new IPRange
                        {
                            Name = "TestChannelInput001",
                            Address = IPAddress.Parse("0.0.0.0"),
                            SubnetPrefixLength = 0
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Create channel preview, used in channel creation options. 
        /// </summary>
        /// <returns></returns>
        private static ChannelPreview CreateChannelPreview()
        {
            return new ChannelPreview
            {
                AccessControl = new ChannelAccessControl
                {
                    IPAllowList = new List<IPRange>
                    {
                        new IPRange
                        {
                            Name = "TestChannelPreview001",
                            Address = IPAddress.Parse("0.0.0.0"),
                            SubnetPrefixLength = 0
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Create channel encoding, used in channel creation options. 
        /// </summary>
        /// <returns></returns>
        private static ChannelEncoding CreateChannelEncoding()
        {
            return new ChannelEncoding
            {
                SystemPreset = "Default720p",
                IgnoreCea708ClosedCaptions = false,
                AdMarkerSource = AdMarkerSource.Api,
                // You can only set audio if streaming protocol is set to StreamingProtocol.RTPMPEG2TS.
                AudioStreams = new List<AudioStream> { new AudioStream { Index = 103, Language = "eng" } }.AsReadOnly()
            };
        }

        /// <summary>
        /// Create an asset and configure asset delivery policies.
        /// </summary>
        /// <returns></returns>
        public static IAsset CreateAndConfigureAsset()
        {
            IAsset asset = _context.Assets.Create(_mediaServicesAssetName, AssetCreationOptions.None);

            IAssetDeliveryPolicy policy =
                _context.AssetDeliveryPolicies.Create("Clear Policy",
                AssetDeliveryPolicyType.NoDynamicEncryption,
                AssetDeliveryProtocol.HLS | AssetDeliveryProtocol.SmoothStreaming | AssetDeliveryProtocol.Dash, null);

            asset.DeliveryPolicies.Add(policy);

            return asset;
        }

        /// <summary>
        /// Create a Program on the Channel. You can have multiple Programs that overlap or are sequential;
        /// however each Program must have a unique name within your Media Services account.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static IProgram CreateAndStartProgram(IChannel channel, IAsset asset)
        {
            IProgram program = channel.Programs.Create(_mediaServicesProgramName, TimeSpan.FromHours(3), asset.Id);
            Log("Program created", program.Id);

            Log("Starting program");
            var programStartOperation = program.SendStartOperation();
            TrackOperation(programStartOperation, "Program start");

            return program;
        }

        /// <summary>
        /// Create locators in order to be able to publish and stream the video.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="ArchiveWindowLength"></param>
        /// <returns></returns>
        public static ILocator CreateLocatorForAsset(IAsset asset, TimeSpan ArchiveWindowLength)
        {
            var locator = _context.Locators.CreateLocator
                (
                    LocatorType.OnDemandOrigin,
                    asset,
                    _context.AccessPolicies.Create
                        (
                            "Live Stream Policy",
                            ArchiveWindowLength,
                            AccessPermissions.Read
                        )
                );

            return locator;
        }

        /// <summary>
        /// Perform operations on slates.
        /// </summary>
        /// <param name="channel"></param>
        public static void StartStopAdsSlates(IChannel channel)
        {
            int cueId = new Random().Next(int.MaxValue);
            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\SlateJPG\\DefaultAzurePortalSlate.jpg"));

            Log("Creating asset");
            var slateAsset = _context.Assets.Create("Slate test asset " + DateTime.Now.ToString("yyyy-MM-dd HH-mm"), AssetCreationOptions.None);
            Log("Slate asset created", slateAsset.Id);

            Log("Uploading file");
            var assetFile = slateAsset.AssetFiles.Create("DefaultAzurePortalSlate.jpg");
            assetFile.Upload(path);
            assetFile.IsPrimary = true;
            assetFile.Update();

            Log("Showing slate");
            var showSlateOpeartion = channel.SendShowSlateOperation(TimeSpan.FromMinutes(1), slateAsset.Id);
            TrackOperation(showSlateOpeartion, "Show slate");

            Log("Hiding slate");
            var hideSlateOperation = channel.SendHideSlateOperation();
            TrackOperation(hideSlateOperation, "Hide slate");

            Log("Starting ad");
            var startAdOperation = channel.SendStartAdvertisementOperation(TimeSpan.FromMinutes(1), cueId, false);
            TrackOperation(startAdOperation, "Start ad");

            Log("Ending ad");
            var endAdOperation = channel.SendEndAdvertisementOperation(cueId);
            TrackOperation(endAdOperation, "End ad");

            Log("Deleting slate asset");
            slateAsset.Delete();
        }

        /// <summary>
        /// Clean up resources associated with the channel.
        /// </summary>
        /// <param name="channel"></param>
        public static void Cleanup(IChannel channel)
        {
            IAsset asset;
            if (channel != null)
            {
                foreach (var program in channel.Programs)
                {
                    asset = _context.Assets.Where(se => se.Id == program.AssetId)
                                            .FirstOrDefault();

                    Log("Stopping program");
                    var programStopOperation = program.SendStopOperation();
                    TrackOperation(programStopOperation, "Program stop");

                    program.Delete();

                    if (asset != null)
                    {
                        Log("Deleting locators");
                        foreach (var l in asset.Locators)
                            l.Delete();

                        Log("Deleting asset");
                        asset.Delete();
                    }
                }

                Log("Stopping channel");
                var channelStopOperation = channel.SendStopOperation();
                TrackOperation(channelStopOperation, "Channel stop");

                Log("Deleting channel");
                var channelDeleteOperation = channel.SendDeleteOperation();
                TrackOperation(channelDeleteOperation, "Channel delete");
            }
        }


        /// <summary>
        /// Track long running operations.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string TrackOperation(IOperation operation, string description)
        {
            string entityId = null;
            bool isCompleted = false;

            Log("starting to track ", null, operation.Id);
            while (isCompleted == false)
            {
                operation = _context.Operations.GetOperation(operation.Id);
                isCompleted = IsCompleted(operation, out entityId);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
            }
            // If we got here, the operation succeeded.
            Log(description + " in completed", operation.TargetEntityId, operation.Id);

            return entityId;
        }

        /// <summary> 
        /// Checks if the operation has been completed. 
        /// If the operation succeeded, the created entity Id is returned in the out parameter.
        /// </summary> 
        /// <param name="operationId">The operation Id.</param> 
        /// <param name="channel">
        /// If the operation succeeded, 
        /// the entity Id associated with the sucessful operation is returned in the out parameter.</param>
        /// <returns>Returns false if the operation is still in progress; otherwise, true.</returns> 
        private static bool IsCompleted(IOperation operation, out string entityId)
        {

            bool completed = false;

            entityId = null;

            switch (operation.State)
            {
                case OperationState.Failed:
                    // Handle the failure. 
                    // For example, throw an exception. 
                    // Use the following information in the exception: operationId, operation.ErrorMessage.
                    Log("operation failed", operation.TargetEntityId, operation.Id);
                    break;
                case OperationState.Succeeded:
                    completed = true;
                    entityId = operation.TargetEntityId;
                    break;
                case OperationState.InProgress:
                    completed = false;
                    Log("operation in progress", operation.TargetEntityId, operation.Id);
                    break;
            }
            return completed;
        }


        private static void Log(string action, string entityId = null, string operationId = null)
        {
            Console.WriteLine(
                "{0,-21}{1,-51}{2,-51}{3,-51}",
                DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss"),
                action,
                entityId ?? string.Empty,
                operationId ?? string.Empty);
        }
    }
}
