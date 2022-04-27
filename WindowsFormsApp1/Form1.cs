using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.Communication.Grpc;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private static async Task AppendItemsRequestMethods()
        {
            List<AppendItemsRequest> grpcItems = new List<AppendItemsRequest>();
            for (int i = 0; i < 200000; i++)
            {
                GrpcItem itm = new GrpcItem()
                {
                    Flags = "",
                    HelperCode = "",
                    Notification = 1,
                    NtinId = 1422,
                    PNtinId = null,
                    PSerial = "",
                    Sequence = i,
                    Serial = "12345" + i,
                    Status = 0,
                    Type = 100,
                };
                grpcItems.Add(new AppendItemsRequest(){Item = itm});
            }

            var chanel = GrpcChannel.ForAddress("http://172.25.0.100:4424", new GrpcChannelOptions
            {
                //Credentials = ChannelCredentials.Insecure,
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    PreAuthenticate = false,
                    UseCookies = false,
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                },
            });
            var service = new GrpcService.GrpcServiceClient(chanel);
            var call = service.AppendItems();
            foreach (var r in grpcItems)
            {
                await call.RequestStream.WriteAsync(r).ConfigureAwait(false);
            }
            await call.RequestStream.CompleteAsync();
        }

        private static async Task AppendItemsRequestMethodsOLD()
        {
            List<AppendItemsRequest> grpcItems = new List<AppendItemsRequest>();
            for (int i = 0; i < 200000; i++)
            {
                GrpcItem itm = new GrpcItem()
                {
                    Flags = "",
                    HelperCode = "",
                    Notification = 1,
                    NtinId = 1422,
                    PNtinId = null,
                    PSerial = "",
                    Sequence = i,
                    Serial = "12345" + i,
                    Status = 0,
                    Type = 100,
                };
                grpcItems.Add(new AppendItemsRequest(){Item = itm});
            }

            var chanel = new Channel("172.25.0.100", 4424, ChannelCredentials.Insecure);
            var service = new GrpcService.GrpcServiceClient(chanel);
            var call = service.AppendItems();
            foreach (var r in grpcItems)
            {
                await call.RequestStream.WriteAsync(r).ConfigureAwait(false);
            }
            await call.RequestStream.CompleteAsync();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            AppendItemsRequestMethods();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AppendItemsRequestMethodsOLD();
        }
    }
}
