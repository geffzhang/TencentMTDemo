using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace TencentMTDemo
{
    class ImageTranslate
    {
        static void Main(string[] args)
        {
            try
            {
                // 必要步骤：
                // 实例化一个认证对象，入参需要传入腾讯云账户密钥对secretId，secretKey。
                // 这里采用的是从环境变量读取的方式，需要在环境变量中先设置这两个值。
                // 你也可以直接在代码中写死密钥对，但是小心不要将代码复制、上传或者分享给他人，
                // 以免泄露密钥对危及你的财产安全。
                Credential cred = new Credential
                {
                    SecretId = Environment.GetEnvironmentVariable("TENCENTCLOUD_SECRET_ID"),
                    SecretKey = Environment.GetEnvironmentVariable("TENCENTCLOUD_SECRET_KEY")
                };

                // 实例化一个client选项，可选的，没有特殊需求可以跳过
                ClientProfile clientProfile = new ClientProfile();
                // 指定签名算法(默认为HmacSHA256)
                clientProfile.SignMethod = ClientProfile.SIGN_SHA1;
                // 非必要步骤
                // 实例化一个客户端配置对象，可以指定超时时间等配置
                HttpProfile httpProfile = new HttpProfile();
                // SDK默认使用POST方法。
                // 如果你一定要使用GET方法，可以在这里设置。GET方法无法处理一些较大的请求。
                httpProfile.ReqMethod = "POST";
                // SDK有默认的超时时间，非必要请不要进行调整。
                // 如有需要请在代码中查阅以获取最新的默认值。
                httpProfile.Timeout = 10; // 请求连接超时时间，单位为秒(默认60秒)
                // SDK会自动指定域名。通常是不需要特地指定域名的，但是如果你访问的是金融区的服务，
                // 则必须手动指定域名，例如云服务器的上海金融区域名： cvm.ap-shanghai-fsi.tencentcloudapi.com
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                // 代理服务器，当你的环境下有代理服务器时设定
                httpProfile.WebProxy = Environment.GetEnvironmentVariable("HTTPS_PROXY");

                clientProfile.HttpProfile = httpProfile;

                // 实例化要请求产品的client对象
                // 第二个参数是地域信息，可以直接填写字符串ap-guangzhou，或者引用预设的常量，clientProfile是可选的
                TmtClient client = new TmtClient(cred, "ap-guangzhou", clientProfile);

                // 实例化一个请求对象，根据调用的接口和实际情况，可以进一步设置请求参数
                // 你可以直接查询SDK源码确定ImageTranslateRequest 有哪些属性可以设置，
                // 属性可能是基本类型，也可能引用了另一个数据结构。
                // 推荐使用IDE进行开发，可以方便的跳转查阅各个接口和数据结构的文档说明。
                ImageTranslateRequest req = new ImageTranslateRequest();
                req.SessionUuid = Guid.NewGuid().ToString();
                req.Scene = "doc";
                string imageData = ImageHelper.GetImageBase64(@"D:\workshop\Github\TencentMTDemo\20180928133022.jpg");
                req.Data = imageData;
                req.Source = "zh";
                req.Target = "en";
                req.ProjectId = 0;
                var resp = client.ImageTranslate(req).ConfigureAwait(false).GetAwaiter().GetResult();
                // 输出json格式的字符串回包
                Console.WriteLine(AbstractModel.ToJsonString(resp));

                // 也可以取出单个值。
                // 你可以通过官网接口文档或跳转到response对象的定义处查看返回字段的定义
                Console.WriteLine(resp.Target);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.Read();
        }

    }
}
